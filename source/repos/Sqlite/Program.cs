using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
namespace Sqlite
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

            
            string connectionString = $"DataSource = D:\\NYPMigrationDB\\New folder\\NypMigrtation.db;";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
}
    }
}
