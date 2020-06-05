using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MShop.Models;
using MySql.Data.MySqlClient;
using dto = MShop.Models;

namespace MShop.Views.Shared.Components
{
    public class ProductsViewComponent : ViewComponent
    {
        private MySqlDB MySqlDB { get; set; }
        public ProductsViewComponent(MySqlDB mySqlDatabase)
        {
            this.MySqlDB = mySqlDatabase;
        }
        private async Task<List<dto.ProductModel>> GetProducts(string type, string category)
        {
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

        public async Task<IViewComponentResult> InvokeAsync(string type, string category)
        {
            return View(await this.GetProducts(type, category));
        }
    }
}
