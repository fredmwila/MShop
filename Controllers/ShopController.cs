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

        //public string culture = "en-NZ";

        //public string currency = "NZD";

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
            //System.Diagnostics.Debug.WriteLine("ID = " + ID);

            //if (HttpContext.Request.Query["culture"].ToString() != "") culture = HttpContext.Request.Query["culture"].ToString();

            //if (HttpContext.Request.Query["currency"].ToString() != "") currency = HttpContext.Request.Query["currency"].ToString();

            //culture = MShopClass.currencyList()[currency.ToUpper()];


            return this.View();
        }

        public ActionResult Product(string id)

        {
            ViewBag.ProductID = id;
            return this.View();
        }

        public async Task<IActionResult> Stockists()

        {
            ViewBag.Stockists = await this.GetAreas();

            return this.View();
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

       

        public IActionResult GetProductsViewComponent( string viewtype, string category)
        {
            return ViewComponent("Products", new { type = viewtype, category = category });
        }

        public IActionResult GetProductViewComponent(string id)
        {
            return ViewComponent("GetProduct", new { id = id});
        }

    }
}
