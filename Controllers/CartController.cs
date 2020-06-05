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

namespace MShop.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        [Route("index")]
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<ShoppingCartItemModel>>(HttpContext.Session, "cart");
            if (cart == null) cart = new List<ShoppingCartItemModel>();

            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(ShoppingCartItem => ShoppingCartItem.Product.ProductPrice * ShoppingCartItem.Quantity);
            return View();
        }

        [Route("buy/{id}")]
        public IActionResult Buy(int id, int quantity)
        {
            System.Diagnostics.Debug.WriteLine("id:" + id + "; quantity: " + quantity);
            ProductModel product = new ProductModel();
            if (SessionHelper.GetObjectFromJson<List<ShoppingCartItemModel>>(HttpContext.Session, "cart") == null)
            {
                List<ShoppingCartItemModel> cart = new List<ShoppingCartItemModel>();
                cart.Add(new ShoppingCartItemModel { Product = GetProduct(id), Quantity = quantity });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<ShoppingCartItemModel> cart = SessionHelper.GetObjectFromJson<List<ShoppingCartItemModel>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity += quantity;
                }
                else
                {
                    cart.Add(new ShoppingCartItemModel { Product = GetProduct(id), Quantity = quantity });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            List<ShoppingCartItemModel> cart = SessionHelper.GetObjectFromJson<List<ShoppingCartItemModel>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
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

    }
}