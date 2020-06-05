using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MShop.Models;
using MySql.Data.MySqlClient;
using dto = MShop.Models;

namespace MShop.Controllers
{
    public class  ShopController : Controller
    {
        [HttpGet]
        public IActionResult Index(string ID)
        {
            string type = "";
            if (ID == null)
            {
                ID = "all";
            }
            else
            {
                type = "category";
            }
            ViewBag.Category = ID;
            ViewBag.Type = type;
            System.Diagnostics.Debug.WriteLine("ID = " + ID);
            return View();
        }

        public ActionResult Product(string id)

        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Product = GetProduct(id);
           // mymodel.ShoppingCart = GetShoppingCart();
            Debug.WriteLine("Product Code:" + id);
            return View(mymodel);
        }

        public async Task<IActionResult> Stockists()

        {

            return View(await this.GetAreas());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private MySqlDB MySqlDB { get; set; }
        public ShopController(MySqlDB mySqlDatabase)
        {
            this.MySqlDB = mySqlDatabase;
        }

        private async Task<List<dto.StockistModel>> GetAreas()
        {
            var ret = new List<dto.StockistModel>();

            var cmd = this.MySqlDB.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT Distinct StockistCity FROM Stockists;";

            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var t = new dto.StockistModel()
                    {
                        StockistCity = reader.GetFieldValue<string>(0)

                    };

                    ret.Add(t);
                }
            return ret;
        }

        private dto.ProductModel GetProduct(string productCode)
        {
            var ret = new dto.ProductModel();

            var cmd = this.MySqlDB.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT ProductId, ProductName, ProductPrice, ProductImage, ProductCode, ProductLongDesc, ProductUses, ProductIngredients, ProductPackaging FROM Products Where ProductCode='" + productCode +"';";

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

        public IActionResult GetProductsViewComponent( string viewtype, string category)
        {
            return ViewComponent("Products", new { type = viewtype, category = category });
        }

    }
}
