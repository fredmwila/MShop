using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MShop.Models;
using MySql.Data.MySqlClient;
using dto = MShop.Models;
using MShop;
using System.Threading;

namespace MShop.Views.Shared.Components
{
    public class ProductsViewComponent : ViewComponent
    {
        private MySqlDB MySqlDB { get; set; }
        public ProductsViewComponent(MySqlDB mySqlDatabase)
        {
            this.MySqlDB = mySqlDatabase;
        }

        public string GetCulture = CultureInfo.CurrentCulture.Name;
        

        private async Task<List<dto.ProductModel>> GetProducts(string type, string category, string dollar)
        {
            string toCurrency = dollar;
            Single price = 0;
            float exchangeRate = 1;
            if ((toCurrency != "")&&(toCurrency.ToUpper() !="NZD")) exchangeRate = CurrencyConvertor.GetExchangeRate(toCurrency);

            if (category != null) category = category.Replace("-", " ");
            var ret = new List<dto.ProductModel>();

            var cmd = this.MySqlDB.Connection.CreateCommand() as MySqlCommand;

            string query = @"SELECT p.ProductId, p.ProductName, p.ProductPrice, p.ProductImage, p.ProductCode,  c.CategoryID, c.categoryname from products p inner join productcategories c on p.ProductCategoryID=c.categoryid ";
            
            
            if (type=="featured") query += "where productID = 15 or productID = 8 or productID = 9";
            if ((type == "category") && (category.ToLower() != "all")) query += "where categoryname = '" + category + "'";

            query += ";";
            System.Diagnostics.Debug.WriteLine("Type: " + type);
            System.Diagnostics.Debug.WriteLine("Category: " + category);
            System.Diagnostics.Debug.WriteLine("Query: " + query);
            cmd.CommandText = query;
            


            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    price = reader.GetFieldValue<Single>(2);
                    var t = new dto.ProductModel()
                    {
                        
                    ProductId = reader.GetFieldValue<int>(0),
                        ProductName = reader.GetFieldValue<string>(1).ToUpper(),
                        ProductPrice = price*exchangeRate,
                        ProductImage = reader.GetFieldValue<string>(3),
                        ProductCode = reader.GetFieldValue<string>(4),
                        CategoryID = reader.GetFieldValue<int>(5),
                        CategoryName = reader.GetFieldValue<string>(6)
                    };

                    ret.Add(t);
                }
            return ret;
        }

        public async Task<IViewComponentResult> InvokeAsync(string type, string category)
        {

            string culture = "";

            string currency = "";

            //currency = HttpContext.Request.Query["currency"].ToString().ToUpper();

            //currency = MShopClass.getCurrency(currency);

            //culture = MShopClass.currencyList()[currency.ToUpper()];

            //var cultureSet = CultureInfo.GetCultureInfo(culture);
            //Thread.CurrentThread.CurrentCulture = cultureSet;
            //Thread.CurrentThread.CurrentUICulture = cultureSet;

            culture = CultureInfo.CurrentCulture.Name.ToLower();

            currency = MShopClass.getCurrency(culture);

            ViewBag.Culture = CultureInfo.CurrentCulture.Name;



            //System.Diagnostics.Debug.Print("Current Culture: " + culture + ";");
            ViewBag.Dollar = currency;
            return View(await this.GetProducts(type, category, currency));
        }
    }
}
