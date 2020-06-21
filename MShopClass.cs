using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using dto = MShop.Models;
using MShop;
using MShop.Models;
using Microsoft.AspNetCore.Http;

namespace MShop
{
    public class MShopClass
    {
        private MySqlDB MySqlDB { get; set; }
        

        private async Task<List<dto.ProductModel>> GetProducts(string type, string category, MySqlDB MySqlDB)
        {
            if (category != null) category = category.Replace("-", " ");
            var ret = new List<dto.ProductModel>();

            var cmd = this.MySqlDB.Connection.CreateCommand() as MySqlCommand;

            string query = @"SELECT p.ProductId, p.ProductName, p.ProductPrice, p.ProductImage, p.ProductCode,  c.CategoryID, c.categoryname from products p inner join productcategories c on p.ProductCategoryID=c.categoryid ";


            if (type == "featured") query += "where productID = 15 or productID = 8 or productID = 9";
            if (type == "category") query += "where categoryname = '" + category + "'";

            query += ";";
            System.Diagnostics.Debug.WriteLine("Type: " + type);
            System.Diagnostics.Debug.WriteLine("Category: " + category);
            System.Diagnostics.Debug.WriteLine("Query: " + query);
            cmd.CommandText = query;



            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var t = new dto.ProductModel()
                    {
                        ProductId = reader.GetFieldValue<int>(0),
                        ProductName = reader.GetFieldValue<string>(1).ToUpper(),
                        ProductPrice = reader.GetFieldValue<Single>(2),
                        ProductImage = reader.GetFieldValue<string>(3),
                        ProductCode = reader.GetFieldValue<string>(4),
                        CategoryID = reader.GetFieldValue<int>(5),
                        CategoryName = reader.GetFieldValue<string>(6)
                    };

                    ret.Add(t);
                }
            return ret;
        }

        public static float RoundUp(float value)
        {
            
            return Convert.ToSingle(Math.Ceiling(value * 20) / 20);
        }

        public static int RoundDown(int toRound)
        {
            return toRound - toRound % 10;
        }

        public static Dictionary<string,string> currencyList()
        {
            Dictionary<string, string> currencyDictionary = new Dictionary<string, string>();

            currencyDictionary.Add("en-nz", "NZD");
            currencyDictionary.Add("en-au","AUD");
            currencyDictionary.Add("en-us","USD");
            currencyDictionary.Add("en-gb","GBP");
            currencyDictionary.Add("fr-fr","EUR");
            currencyDictionary.Add("zh-cn", "CNY");
            currencyDictionary.Add("ja-jp", "JPY");
            return currencyDictionary;
        }

        public static string getCurrency(string culture)
        {
            if (currencyList().ContainsKey(culture))
            {
                return currencyList()[culture];
            }
            else
            {
                return "NZD";
            }

        }

        //public HomeViewModel GetView()
        //{
        //    string culture = "en-NZ";

        //    string currency = "NZD";

        //    if (HttpContext.Request.Query["culture"].ToString() != "") culture = HttpContext.Request.Query["culture"].ToString();

        //    if (HttpContext.Request.Query["currency"].ToString() != "") currency = HttpContext.Request.Query["currency"].ToString();

        //    culture = MShopClass.currencyList()[currency.ToUpper()];

        //    HomeViewModel model = new HomeViewModel { Culture = culture, Currency = currency } ;

        //    return model;
        //}

    }
}
