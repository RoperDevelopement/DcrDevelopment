using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace SqlCommands
{
    interface SqlInterface
    {
        SqlConnection SqlConnection(string dbName, string dbUserName, string dbpassword);
        SqlDataReader SqlDataReader(string spName, Dictionary<string, string> sqlParams,SqlConnection sqlConnection,Guid binId);
        DataTable GetCategories(SqlConnection sqlConnection);
    }
}
