using System;using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MShop.Helpers;
using MySql.Data.MySqlClient;
using dto = MShop.Models;
using MShop;
using System.Globalization;

namespace MShop.Controllers
{

    public class CartController : Controller
    {
        [HttpGet]
        public IActionResult Index(string culture, string currency)
        {
            return this.View();
        }

        [Route("cart/buy/{id}")]
        public IActionResult Buy(int id, int quantity, bool setQuantity = false)
        {
            System.Diagnostics.Debug.WriteLine("id:" + id + "; quantity: " + quantity);
            ProductModel product = new ProductModel();
            List<ShoppingCartItemModel> cart = new List<ShoppingCartItemModel>();
            if (SessionHelper.GetObjectFromJson<List<ShoppingCartItemModel>>(HttpContext.Session, "cart") == null)
            {
                
                cart.Add(new ShoppingCartItemModel { Product = GetProduct(id), Quantity = quantity });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                cart = SessionHelper.GetObjectFromJson<List<ShoppingCartItemModel>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity = (setQuantity == true) ? quantity : cart[index].Quantity + quantity;

                }
                else
                {
                    cart.Add(new ShoppingCartItemModel { Product = GetProduct(id), Quantity = quantity });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

                
            }
            int cartTotal = 0;
            cartTotal = cart.Sum(item => item.Quantity);
            HttpContext.Session.SetInt32("CartCount", cartTotal);


            return RedirectToAction("Index");

        }

        [Route("cart/remove/{id}")]
        public IActionResult Remove(int id ,string culture, string currency)
        {
            List<ShoppingCartItemModel> cart = SessionHelper.GetObjectFromJson<List<ShoppingCartItemModel>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index", new { Culture = culture, Currency = currency });
        }

        private int isExist(int id)
        {
            List<ShoppingCartItemModel> cart = SessionHelper.GetObjectFromJson<List<ShoppingCartItemModel>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine("Index:" + i + "; ProductID:" + cart[i].Product.ProductId + "; ID:" + id + ";");
                if (cart[i].Product.ProductId == id)
                {
                    return i;
                }
            }
            return -1;
        }

        private MySqlDB MySqlDB { get; set; }

        public CartController(MySqlDB mySqlDatabase)
        {
            this.MySqlDB = mySqlDatabase;
        }

        private dto.ProductModel GetProduct(int id)
        {
            var ret = new dto.ProductModel();

            var cmd = this.MySqlDB.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT ProductId, ProductName, ProductPrice, ProductImage, ProductCode, ProductLongDesc, ProductUses, ProductIngredients, ProductPackaging FROM Products Where ProductID='" + id + "';";

            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                {
                    var t = new dto.ProductModel()
                    {
                        ProductId = reader.GetFieldValue<int>(0),
                        ProductName = reader.GetFieldValue<string>(1).ToUpper(),
                        ProductPrice = reader.GetFieldValue<Single>(2),
                        ProductImage = reader.GetFieldValue<string>(3),
                        ProductCode = reader.GetFieldValue<string>(4),
                        ProductLongDesc = reader.GetFieldValue<string>(5),
                        ProductUses = reader.GetFieldValue<string>(6),
                        ProductIngredients = reader.GetFieldValue<string>(7),
                        ProductPackaging = reader.GetFieldValue<string>(8)
                    };
                    ret = t;
                }
            return ret;
        }

        [Route("{culture}/cart/GetCart")]
        public IActionResult GetShoppingCartViewComponent()
        {
            return ViewComponent("ShoppingCart");
        }

        [Route("cart/GetCartTotal")]
        [HttpPost]
        public JsonResult GetCartTotal()
        {
            int cartTotal = 0;
            List<ShoppingCartItemModel> cart = new List<ShoppingCartItemModel>();

            cart = SessionHelper.GetObjectFromJson<List<ShoppingCartItemModel>>(HttpContext.Session, "cart");
            
            cartTotal=cart.Sum(item => item.Quantity);
            HttpContext.Session.SetInt32("CartCount", cartTotal);

            System.Diagnostics.Debug.WriteLine("Cart Total: " + Json(new { cartTotal }).ToString());

            return Json(new { cartTotal });

        }

    }
}