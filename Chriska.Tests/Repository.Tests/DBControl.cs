using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chriska.Tests.Repository.Tests
{
    internal class DBControl
    {
        protected static string connectionString = LoadConnectionString();
        private static string LoadConnectionString()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("./appsettings.Development.json", optional: false)
                .Build();

            return config.GetConnectionString("TestDatabase");
        }
        public static void ClearDatabase()
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            using var command = new SqlCommand(@"
            DELETE FROM Roles_Permissions;
            DELETE FROM Roles;
            ", connection);

            command.ExecuteNonQuery();
        }
    }
}
