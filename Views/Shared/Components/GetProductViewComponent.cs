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
    public class GetProductViewComponent : ViewComponent
    {
        private MySqlDB MySqlDB { get; set; }
        public GetProductViewComponent(MySqlDB mySqlDatabase)
        {
            this.MySqlDB = mySqlDatabase;
        }


    

        private dto.ProductModel GetProductData(string productCode)
        {
            var ret = new dto.ProductModel();

            var cmd = this.MySqlDB.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT ProductId, ProductName, ProductPrice, ProductImage, ProductCode, ProductLongDesc, ProductUses, ProductIngredients, ProductPackaging FROM Products Where ProductCode='" + productCode + "';";

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

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            return View(GetProductData(id));
        }
    }
}
