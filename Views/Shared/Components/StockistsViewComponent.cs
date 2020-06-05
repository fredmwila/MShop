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
    public class StockistsViewComponent : ViewComponent
    {
        private MySqlDB MySqlDB { get; set; }
        public StockistsViewComponent(MySqlDB mySqlDatabase)
        {
            this.MySqlDB = mySqlDatabase;
        }
        private async Task<List<dto.StockistModel>> GetStockists(string area)
        {
            var ret = new List<dto.StockistModel>();

            var cmd = this.MySqlDB.Connection.CreateCommand() as MySqlCommand;

            string query = @"SELECT StockistID, StockistName, StockistStreet, StockistSuburb, StockistCity, StockistPhoneNumber, StockistURL  FROM stockists where stockistcity = '" + area + "'";

            System.Diagnostics.Debug.WriteLine(query);
            
            cmd.CommandText = query;
            


            using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    var t = new dto.StockistModel()
                    {
                        StockistId = reader.GetFieldValue<int>(0),
                        StockistName = reader.GetFieldValue<string>(1),
                        StockistStreet = reader.GetFieldValue<string>(2),
                        StockistSuburb = reader.GetFieldValue<string>(3),
                        StockistCity = reader.GetFieldValue<string>(4),
                        StockistPhoneNumber = reader.GetFieldValue<string>(5),
                        StockistURL = reader.GetFieldValue<string>(6)
                    };

                    ret.Add(t);
                }
            return ret;
        }

        public async Task<IViewComponentResult> InvokeAsync(string area)
        {
            return View(await this.GetStockists(area));
        }
    }
}
