using System;
using MySql.Data.MySqlClient;

namespace MShop
{
    public class MySqlDB : IDisposable
    {
        public MySqlConnection Connection;

        public MySqlDB(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
            this.Connection.Open();
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}