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
    public class CategoriesViewComponent : ViewComponent
    {
        private MySqlDB MySqlDB { get; set; }
        public CategoriesViewComponent(MySqlDB mySqlDatabase)
        {
            this.MySqlDB = mySqlDatabase;
        }
        private async Task<List<dto.CategoryModel>> GetCategories()
        {
            var ret = new List<dto.CategoryModel>();

            var cmd = this.MySqlDB.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT CategoryID, CategoryName, CategoryDesc, CategoryImage from ProductCategories order by categoryid asc;";

            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var t = new dto.CategoryModel()
                    {
                        CategoryID = reader.GetFieldValue<int>(0),
                        CategoryName = reader.GetFieldValue<string>(1).ToUpper(),
                        CategoryDesc = reader.GetFieldValue<string>(2),
                        CategoryImage = reader.GetFieldValue<string>(3)
                    };

                    ret.Add(t);
                }
            return ret;
        }

        public async Task<IViewComponentResult> InvokeAsync(string type, string category)
        {
            return View(await this.GetCategories());
        }
    }
}
