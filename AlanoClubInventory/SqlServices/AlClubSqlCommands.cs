using AlanoClubInventory.Interfaces;
using AlanoClubInventory.Models;
using AlanoClubInventory.SqlServices;
using AlanoClubInventory.Utilites;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Microsoft.ReportingServices.RdlExpressions.ExpressionHostObjectModel;
using Microsoft.SqlServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static OpenTK.Graphics.OpenGL.GL;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace AlanoClubInventory.SqlServices
{
    public class AlClubSqlCommands :  IDisposable
 
    {
        private static AlClubSqlCommands sqlCmdInstance;
        private static readonly object _lock = new object();
        private AlClubSqlCommands() { }
        public static AlClubSqlCommands SqlCmdInstance
        {
            get
            {
                // Double-checked locking for thread safety
                if (sqlCmdInstance == null)
                {
                    lock (_lock)
                    {
                        if (sqlCmdInstance == null)
                        {
                            sqlCmdInstance = new AlClubSqlCommands();
                        }
                    }
                }
                return sqlCmdInstance;
            }
        }

        public async Task AddEditDeleteCategories(string sqlConnectionStr, CategoryModel category, string storedProcedueName, int delete = 0)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaID, category.ID));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaCategoryName, category.CategoryName));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaDelete, delete));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaBarItem, category.BarItem));
                        await sqlCmd.ExecuteNonQueryAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error AddEditDeleteCategories running sp {storedProcedueName} for delete {delete} for Category table {ex.Message}");
            }



        }
        public async Task AddEditDeleteInventory(string sqlConnectionStr, AddEditInventoryModel inventory, string storedProcedueName, int delete = 0, int barItem = 0)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaID, inventory.ID));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaProductName, inventory.ProductName));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaCategoryID, inventory.CategoryID));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaPrice, inventory.Price));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaBarItem, barItem));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaQuantity, inventory.Quantity));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaDelete, delete));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaItemsPerCase, inventory.ItemsPerCase));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaInventoryItem, inventory.InventoryItem));

                        await sqlCmd.ExecuteNonQueryAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"AddEditDeleteInventory for {storedProcedueName} for delete {delete} for baritem {barItem} message: {ex.Message}");

            }
        }
        public async Task AlanoCLubTillPrices(string sqlConnectionStr, AlanoClubTillPricesModel alanoClubTillPrices, string storedProcedueName, int delete = 0)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaID, alanoClubTillPrices.ID));

                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaMemberPrice, alanoClubTillPrices.ClubPrice));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaNonMemberPrice, alanoClubTillPrices.ClubNonMemberPrice));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaDelete, delete));

                        await sqlCmd.ExecuteNonQueryAsync();

                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error Method AlanoCLubTillPrices for {storedProcedueName} for delete {delete} {ex.Message}");

            }
        }
        public async Task AlanoCLubDailyTillRecepits(string sqlConnectionStr, AlanoClubTillPricesModel dailyTillReceipt, string storedProcedueName, int delete = 0)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaID, dailyTillReceipt.ID));

                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaMemberPrice, dailyTillReceipt.ClubPrice));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaNonMemberPrice, dailyTillReceipt.ClubNonMemberPrice));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaMemberITems, dailyTillReceipt.TotalMemberSold));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaNonMemberITems, dailyTillReceipt.TotalNonMemberSold));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaDailyProductTotal, dailyTillReceipt.DailyProductTotal));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaDateTillReceipt, dailyTillReceipt.DateCreated));
                        // sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaDelete, delete));

                        await sqlCmd.ExecuteNonQueryAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method AlanoCLubDailyTillRecepits for {storedProcedueName} for delete {delete} {ex.Message}");

            }
        }
        public async Task<IList<CategoryModel>> GetCategoriesBarItems(string sqlConnectionStr, string storedProcedueName)
        {
            IList<CategoryModel> categories = new List<CategoryModel>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                await reader.ReadAsync();

                                if (!(reader.IsDBNull(0)))
                                {

                                    //SqlDataAdapter adapter = new SqlDataAdapter();
                                    //  DataTable dataTable = new DataTable();
                                    //dataTable.Load(reader);

                                    // Convert DataTable to JSON
                                    // string cat = JsonConvert.SerializeObject(dataTable);
                                    // categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(cat).ToList();

                                    string json = reader.GetString(0);
                                    categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(json).ToList();



                                }
                            }
                            await reader.CloseAsync();

                        }





                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method GetCategoriesBarItems for {storedProcedueName}  {ex.Message}");
            }
            return categories;
        }
        public async Task<IList<CategoryModel>> GetCategories(string sqlConnectionStr, string storedProcedueName)
        {
            IList<CategoryModel> categories = new List<CategoryModel>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                //SqlDataAdapter adapter = new SqlDataAdapter();
                                //  DataTable dataTable = new DataTable();
                                //dataTable.Load(reader);

                                // Convert DataTable to JSON
                                // string cat = JsonConvert.SerializeObject(dataTable);
                                // categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(cat).ToList();

                                string json = reader.GetString(0);
                                categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(json).ToList();



                            }
                            await reader.CloseAsync();

                        }





                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method GetCategories for {storedProcedueName}  {ex.Message}");
            }
            return categories;
        }
        public async Task<IList<T>> GetACProductsList<T>(string sqlConnectionStr, string storedProcedueName, int barItem = 0)
        {
            IList<T> retList = new List<T>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaBarItem, barItem));
                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                await reader.ReadAsync();

                                if (!(reader.IsDBNull(0)))
                                {
                                    //SqlDataAdapter adapter = new SqlDataAdapter();
                                    //  DataTable dataTable = new DataTable();
                                    //dataTable.Load(reader);

                                    // Convert DataTable to JSON
                                    // string cat = JsonConvert.SerializeObject(dataTable);
                                    // categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(cat).ToList();

                                    string json = reader.GetString(0);
                                    retList = JsonConvert.DeserializeObject<IList<T>>(json).ToList();



                                }
                            }
                            await reader.CloseAsync();

                        }





                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method GetACProductsList<T> for {storedProcedueName} parm baritem {barItem} {typeof(T)} {ex.Message}");
            }
            return retList;
        }

        public async Task<IList<T>> GetACProductsList<T>(string sqlConnectionStr, string storedProcedueName)
        {
            //ALanoClubUtilites.ShowMessageBox("Connection to database established successfully.", "Information", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            IList<T> retList = new List<T>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {


                                if (reader.ReadAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                                {

                                    if (!(reader.IsDBNull(0)))
                                    {
                                        //SqlDataAdapter adapter = new SqlDataAdapter();
                                        //  DataTable dataTable = new DataTable();
                                        //dataTable.Load(reader);

                                        // Convert DataTable to JSON
                                        // string cat = JsonConvert.SerializeObject(dataTable);
                                        // categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(cat).ToList();

                                        string json = reader.GetString(0);
                                        retList = JsonConvert.DeserializeObject<IList<T>>(json).ToList();
                                    }


                                }
                            }
                            reader.CloseAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        }





                    }


                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error Method GetACProductsList<T> for {storedProcedueName} {typeof(T)} {ex.Message}");
            }
            return retList;
        }
        public async Task<bool> CheckDailyTillDate(string sqlConnectionStr, string storedProcedueName, string tillDate)
        {

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaDateTillReceipt, tillDate));
                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                //SqlDataAdapter adapter = new SqlDataAdapter();
                                //  DataTable dataTable = new DataTable();
                                //dataTable.Load(reader);

                                // Convert DataTable to JSON
                                // string cat = JsonConvert.SerializeObject(dataTable);
                                // categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(cat).ToList();

                                if (reader.GetInt32(0) > 0)
                                {
                                    return true;
                                }



                            }
                            await reader.CloseAsync();

                        }





                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method CheckDailyTillDate for {storedProcedueName} parm tilldate {tillDate} {ex.Message}");
            }
            return false;
        }

        public async Task<bool> AddDailyTapTotal(string sqlConnectionStr, string storedProcedueName, AlanoCLubDailyTillTapeModel cLubDailyTillTapeModel)
        {

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        if (string.Compare(storedProcedueName, SqlConstProp.SPUpDateAlanoClubOldTillDrop, true) == 0)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaID, cLubDailyTillTapeModel.ID));
                        }
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaDailyTotalSales, cLubDailyTillTapeModel.DailyTillTotal));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaTapeTotal, cLubDailyTillTapeModel.DailyTillTape));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaDepsoit, cLubDailyTillTapeModel.Depsoit));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaDateTillReceipt, cLubDailyTillTapeModel.DateCreated));
                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                //SqlDataAdapter adapter = new SqlDataAdapter();
                                //  DataTable dataTable = new DataTable();
                                //dataTable.Load(reader);

                                // Convert DataTable to JSON
                                // string cat = JsonConvert.SerializeObject(dataTable);
                                // categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(cat).ToList();

                                if (reader.GetInt32(0) > 0)
                                {
                                    return true;
                                }



                            }
                            await reader.CloseAsync();

                        }





                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method AddDailyTapTotal for {storedProcedueName} {ex.Message}");
            }
            return false;
        }


        private void GetReportStartEndDate(string sqlConnectionStr, string storedProcedueName, ref DateTime repSDate, ref DateTime repEdate, int id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaID, id));
                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {

                                    //SqlDataAdapter adapter = new SqlDataAdapter();
                                    //  DataTable dataTable = new DataTable();
                                    //dataTable.Load(reader);

                                    // Convert DataTable to JSON
                                    // string cat = JsonConvert.SerializeObject(dataTable);
                                    // categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(cat).ToList();


                                    if (id == 0)
                                    {
                                        repEdate = DateTime.Now;
                                        repSDate = reader.GetDateTime(0);
                                        repSDate = repSDate.AddDays(1);
                                        if ((repSDate.ToString("MM-dd-yyyy")) == (DateTime.Now.ToString("MM-dd-yyyy")))
                                        {
                                            repEdate = repSDate.AddDays(1);
                                        }
                                        if (repSDate > repEdate)
                                        {
                                            repEdate = repSDate.AddDays(1);
                                        }


                                    }
                                    else
                                    {
                                        repSDate = reader.GetDateTime(0);
                                        repEdate = reader.GetDateTime(1);
                                    }




                                }
                            }
                            reader.Close();



                        }


                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method GetReportStartEndDate for {storedProcedueName} repsdate {repSDate.ToString()} repedate {repEdate.ToString()} id {id} {ex.Message}");
            }

        }






        public void GetReportDate(string sqlConnectionStr, string storedProcedueName, ref DateTime repSDate, ref DateTime repEdate)
        {
            try
            {
                var currentRepDate = repSDate;

                GetReportStartEndDate(sqlConnectionStr, storedProcedueName, ref repSDate, ref repEdate, 0);
                if (repSDate == currentRepDate)
                {
                    GetReportStartEndDate(sqlConnectionStr, storedProcedueName, ref repSDate, ref repEdate, 1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method GetReportDate for {storedProcedueName} repsdate {repSDate.ToString()} repedate {repEdate.ToString()} {ex.Message}");
            }

        }
        public async Task<IList<AlanoClubReportModel>> GetReport(string sqlConnectionStr, string storedProcedueName, DateTime repSDate, DateTime repEDate)
        {
            IList<AlanoClubReportModel> retList = new List<AlanoClubReportModel>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaRepSDate, repSDate));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaRepEDate, repEDate));

                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                //SqlDataAdapter adapter = new SqlDataAdapter();
                                //  DataTable dataTable = new DataTable();
                                //dataTable.Load(reader);

                                // Convert DataTable to JSON
                                // string cat = JsonConvert.SerializeObject(dataTable);
                                // categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(cat).ToList();
                                while (reader.Read())
                                {
                                    AlanoClubReportModel alanoClubReportModel = new AlanoClubReportModel();


                                    alanoClubReportModel.ID = reader.GetInt32("ID");
                                    alanoClubReportModel.BarItem = reader.GetBoolean("BarItem");
                                    alanoClubReportModel.ClubPrice = float.Parse(reader.GetString("ClubPrice"));
                                    alanoClubReportModel.ClubNonMemberPrice = float.Parse(reader.GetString("ClubNonMemberPrice"));
                                    alanoClubReportModel.TotalMemberSold = reader.GetInt32("TotalMemberSold");
                                    alanoClubReportModel.TotalNonMemberSold = reader.GetInt32("TotalNonMemberSold");
                                    alanoClubReportModel.DailyProductTotal = float.Parse(reader.GetString("DailyProductTotal"));
                                    var dt = reader.GetDateTime("DateCreated").ToString("MM-dd-yyyy");
                                    alanoClubReportModel.DateCreated = DateTime.Parse(dt);
                                    alanoClubReportModel.ProductName = reader.GetString("ProductName");
                                    alanoClubReportModel.CategoryName = reader.GetString("CategoryName");
                                    alanoClubReportModel.CategoryID = reader.GetInt32("CategoryID");

                                    retList.Add(alanoClubReportModel);
                                    // var json = reader.GetString(0);
                                }
                                // string json = reader.GetString(0);
                                // retList = JsonConvert.DeserializeObject<IList<AlanoClubReportModel>>(json).ToList();



                            }
                            await reader.CloseAsync();

                        }





                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method GetReport for {storedProcedueName} repsdate {repSDate.ToString()} repedate {repEDate.ToString()} {ex.Message}");
            }
            return retList;
        }

        public async Task<IList<T>> GetDailyTillDateEDate<T>(string sqlConnectionStr, string storedProcedueName, DateTime sDate, DateTime eDate)
        {
            IList<T> retList = new List<T>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaRepSDate, sDate));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaRepEDate, eDate));

                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {

                                await reader.ReadAsync();
                                //while( await reader.ReadAsync())
                                //{
                                //    AlanoCLubDailyTillTapeModel alanoClubReportModel = new AlanoCLubDailyTillTapeModel();


                                //    alanoClubReportModel.ID = reader.GetInt32("ID");
                                //    //var totalSales = reader.GetString(3);
                                //    //var formatFloat = Utilites.ALanoClubUtilites.ConvertToFloat(totalSales);
                                //    alanoClubReportModel.Depsoit = float.Parse(reader.GetString("Depsoit"));
                                //    alanoClubReportModel.DailyTillTape= float.Parse(reader.GetString("DailyTillTape"));
                                //    alanoClubReportModel.DailyTillTotal = float.Parse(reader.GetString("DailyTillTotal"));
                                //    var dt = reader.GetDateTime("DateCreated").ToString("MM-dd-yyyy");
                                //    alanoClubReportModel.DateCreated = DateTime.Parse(dt);
                                //    retList.Add(alanoClubReportModel);
                                //    // var json = reader.GetString(0);
                                //}
                                //SqlDataAdapter adapter = new SqlDataAdapter();
                                //  DataTable dataTable = new DataTable();
                                //dataTable.Load(reader);

                                // Convert DataTable to JSON
                                // string cat = JsonConvert.SerializeObject(dataTable);
                                // categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(cat).ToList();

                                string json = reader.GetString(0);

                                retList = JsonConvert.DeserializeObject<IList<T>>(json).ToList();


                            }
                            await reader.CloseAsync();

                        }


                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method GetDailyTillDateEDate for {storedProcedueName} repsdate {sDate.ToString()} repedate {eDate.ToString()} {ex.Message}");
            }

            return retList;
        }
        public async Task<IList<AlanoClubPricesModel>> GetAlanoCLubProducts(string sqlConnectionStr, string storedProcedueName)
        {
            IList<AlanoClubPricesModel> retList = new List<AlanoClubPricesModel>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;


                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                //SqlDataAdapter adapter = new SqlDataAdapter();
                                //  DataTable dataTable = new DataTable();
                                //dataTable.Load(reader);

                                // Convert DataTable to JSON
                                // string cat = JsonConvert.SerializeObject(dataTable);
                                // categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(cat).ToList();
                                while (reader.Read())
                                {
                                    AlanoClubPricesModel pm = new AlanoClubPricesModel();


                                    pm.ID = reader.GetInt32("ID");

                                    pm.BarItem = reader.GetBoolean("BarItem");
                                    pm.ClubPrice = float.Parse(reader.GetString("ClubPrice"));
                                    pm.ClubNonMemberPrice = float.Parse(reader.GetString("ClubNonMemberPrice"));
                                    pm.ProductName = reader.GetString("ProductName");
                                    pm.CategoryName = reader.GetString("CategoryName");

                                    retList.Add(pm);
                                    // var json = reader.GetString(0);
                                }
                                // string json = reader.GetString(0);
                                // retList = JsonConvert.DeserializeObject<IList<AlanoClubReportModel>>(json).ToList();



                            }
                            await reader.CloseAsync();

                        }





                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method GetAlanoCLubProducts for {storedProcedueName} {ex.Message}");
            }
            return retList;
        }

        public async Task<IList<T>> GetAlanoTotalsByYear<T>(string sqlConnectionStr, string storedProcedueName, int year) where T : new()
        {
            IList<T> retList = new List<T>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaYear, year));

                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                //SqlDataAdapter adapter = new SqlDataAdapter();
                                //  DataTable dataTable = new DataTable();
                                //dataTable.Load(reader);
                                await reader.ReadAsync();
                                // Convert DataTable to JSON
                                // string cat = JsonConvert.SerializeObject(dataTable);
                                // categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(cat).ToList();
                                //while (reader.Read())
                                //{
                                //    AlanoClubPricesModel pm = new AlanoClubPricesModel();


                                //    pm.ID = reader.GetInt32("ID");

                                //    pm.BarItem = reader.GetBoolean("BarItem");
                                //    pm.ClubPrice = float.Parse(reader.GetString("ClubPrice"));
                                //    pm.ClubNonMemberPrice = float.Parse(reader.GetString("ClubNonMemberPrice"));
                                //    pm.ProductName = reader.GetString("ProductName");
                                //    pm.CategoryName = reader.GetString("CategoryName");

                                //    retList.Add(pm);
                                //    // var json = reader.GetString(0);
                                //}
                                string json = reader.GetString(0);
                                retList = JsonConvert.DeserializeObject<IList<T>>(json).ToList();



                            }
                            await reader.CloseAsync();

                        }





                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method GetAlanoTotalsByYear for {storedProcedueName} for year {year} {ex.Message}");
            }

            return retList;

        }

        public async Task<IList<AlanoClubReportModel>> GetReportMonthlyTotalsByYear(string sqlConnectionStr, string storedProcedueName, string sMonthYear, string eMonthYear)
        {
            IList<AlanoClubReportModel> retList = new List<AlanoClubReportModel>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaStartMonthYear, sMonthYear));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaEndMonthYear, eMonthYear));



                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                //SqlDataAdapter adapter = new SqlDataAdapter();
                                //  DataTable dataTable = new DataTable();
                                //dataTable.Load(reader);

                                // Convert DataTable to JSON
                                // string cat = JsonConvert.SerializeObject(dataTable);
                                // categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(cat).ToList();
                                while (await reader.ReadAsync())
                                {
                                    AlanoClubReportModel alanoClubReportModel = new AlanoClubReportModel();


                                    alanoClubReportModel.ID = reader.GetInt32("ID");
                                    alanoClubReportModel.BarItem = reader.GetBoolean("BarItem");
                                    alanoClubReportModel.ClubPrice = float.Parse(reader.GetString("ClubPrice"));
                                    alanoClubReportModel.ClubNonMemberPrice = float.Parse(reader.GetString("ClubNonMemberPrice"));
                                    alanoClubReportModel.TotalMemberSold = reader.GetInt32("TotalMemberSold");
                                    alanoClubReportModel.TotalNonMemberSold = reader.GetInt32("TotalNonMemberSold");
                                    alanoClubReportModel.DailyProductTotal = float.Parse(reader.GetString("DailyProductTotal"));
                                    var dt = reader.GetDateTime("DateCreated").ToString("MM-dd-yyyy");
                                    alanoClubReportModel.DateCreated = DateTime.Parse(dt);
                                    alanoClubReportModel.ProductName = reader.GetString("ProductName");
                                    alanoClubReportModel.CategoryName = reader.GetString("CategoryName");
                                    alanoClubReportModel.CategoryID = reader.GetInt32("CategoryID");
                                    alanoClubReportModel.MemBerPriceTotal = float.Parse(reader.GetString("MemBerPriceTotal"));
                                    alanoClubReportModel.NonMemBerPriceTotal = float.Parse(reader.GetString("NonMemBerPriceTotal"));
                                    retList.Add(alanoClubReportModel);
                                    // var json = reader.GetString(0);
                                }
                                // string json = reader.GetString(0);
                                // retList = JsonConvert.DeserializeObject<IList<AlanoClubReportModel>>(json).ToList();



                            }
                            await reader.CloseAsync();

                        }





                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method GetReportMonthlyTotalsByYear for {storedProcedueName} start month year {sMonthYear} and end month year {eMonthYear} {ex.Message}");
            }
            return retList;
        }


        public async Task UpdateReportRundDate(string sqlConnectionStr, string storedProcedueName, DateTime repEdate, int id = -1)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaID, id));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaRepEDate, repEdate));
                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {



                            if (reader.HasRows)
                            {
                                reader.Read();
                            }
                            reader.Close();
                        }


                    }


                }



            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method UpdateReportRundDate for {storedProcedueName} report date {repEdate.ToString()} id {id} {ex.Message}");
            }

        }
        public async Task<DateTime> GetAlanoClubTillReciptSDate(string sqlConnectionStr, string storedProcedueName)
        {
            DateTime repEdate = DateTime.Now;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {



                            if (reader.HasRows)
                            {
                                reader.Read();
                                if (!(reader.IsDBNull(0)))
                                {
                                    repEdate = reader.GetDateTime("DateTillReceipt");
                                    repEdate = repEdate.AddDays(1);
                                }
                            }
                            reader.Close();
                        }


                    }


                }



            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method GetAlanoClubTillReciptSDate for {storedProcedueName} report date {repEdate.ToString()}  {ex.Message}");
            }
            return repEdate;

        }



        public async Task<IList<AlanoCLubDailyTillTapeModel>> GetDailyTillReport(string sqlConnectionStr, string storedProcedueName, DateTime sDate, DateTime eDate)
        {
            IList<AlanoCLubDailyTillTapeModel> retList = new List<AlanoCLubDailyTillTapeModel>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaRepSDate, sDate));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaRepEDate, eDate));

                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {

                                //  await reader.ReadAsync();
                                while (await reader.ReadAsync())
                                {
                                    AlanoCLubDailyTillTapeModel alanoClubReportModel = new AlanoCLubDailyTillTapeModel();


                                    alanoClubReportModel.ID = reader.GetInt32("ID");
                                    //var totalSales = reader.GetString(3);
                                    //var formatFloat = Utilites.ALanoClubUtilites.ConvertToFloat(totalSales);
                                    alanoClubReportModel.Depsoit = float.Parse(reader.GetString("Depsoit"));
                                    alanoClubReportModel.DailyTillTape = float.Parse(reader.GetString("DailyTillTape"));
                                    alanoClubReportModel.DailyTillTotal = float.Parse(reader.GetString("DailyTillTotal"));
                                    var dt = reader.GetDateTime("DateCreated").ToString("MM-dd-yyyy");
                                    alanoClubReportModel.DateCreated = DateTime.Parse(dt);
                                    retList.Add(alanoClubReportModel);
                                    //    // var json = reader.GetString(0);
                                }
                                //SqlDataAdapter adapter = new SqlDataAdapter();
                                //  DataTable dataTable = new DataTable();
                                //dataTable.Load(reader);

                                // Convert DataTable to JSON
                                // string cat = JsonConvert.SerializeObject(dataTable);
                                // categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(cat).ToList();

                                //  string json = reader.GetString(0);

                                //    retList = JsonConvert.DeserializeObject<IList<T>>(json).ToList();


                            }
                            await reader.CloseAsync();

                        }


                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method GetDailyTillDateEDate for {storedProcedueName} repsdate {sDate.ToString()} repedate {eDate.ToString()} {ex.Message}");
            }

            return retList;
        }

        public async Task UpDateInventory(string sqlConnectionStr, string storedProcedueName, AlanoClubCurrentInventoryModel currentInventoryModel)
        {

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaID, currentInventoryModel.ID));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaQuantity, currentInventoryModel.Quantity));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaInStock, currentInventoryModel.InStock));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaItemsSold, currentInventoryModel.ItemsSold));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaInventoryCurrent, currentInventoryModel.InventoryCurrent));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaDate, DateTime.Now));

                        await sqlCmd.ExecuteReaderAsync();



                    }

                    sqlConnection.Close();

                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method UpDateInventory for {storedProcedueName} for Product ID {currentInventoryModel.ID} {ex.Message}");
            }


        }
        public async Task<IList<T>> CallStoreProdByParmaters<T>(string sqlConnectionStr, string storedProcedueName, IList<StoredParValuesModel> storedParValues) where T : new()
        {
            IList<T> retList = new List<T>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        foreach (var item in storedParValues)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(item.ParmaName, item.ParmaValue));
                        }

                        using (SqlDataReader reader = sqlCmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                        {
                            if (reader.HasRows)
                            {
                                //SqlDataAdapter adapter = new SqlDataAdapter();
                                //  DataTable dataTable = new DataTable();
                                //dataTable.Load(reader);
                                reader.Read();
                                if (!reader.IsDBNull(0))
                                {
                                    // Convert DataTable to JSON
                                    // string cat = JsonConvert.SerializeObject(dataTable);
                                    // categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(cat).ToList();
                                    //while (reader.Read())
                                    //{
                                    //    AlanoClubPricesModel pm = new AlanoClubPricesModel();


                                    //    pm.ID = reader.GetInt32("ID");

                                    //    pm.BarItem = reader.GetBoolean("BarItem");
                                    //    pm.ClubPrice = float.Parse(reader.GetString("ClubPrice"));
                                    //    pm.ClubNonMemberPrice = float.Parse(reader.GetString("ClubNonMemberPrice"));
                                    //    pm.ProductName = reader.GetString("ProductName");
                                    //    pm.CategoryName = reader.GetString("CategoryName");

                                    //    retList.Add(pm);
                                    //    // var json = reader.GetString(0);
                                    //}
                                    string json = reader.GetString(0);
                                    retList = JsonConvert.DeserializeObject<IList<T>>(json).ToList();
                                }


                            }
                            await reader.CloseAsync();

                        }





                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method CallStoreProdByParmaters for {storedProcedueName}  {ex.Message}");
            }
            await Task.CompletedTask;
            return retList;

        }
        public async Task<T> JsonDesObject<T>(string sqlConnectionStr, string model, string storedProcedueName, IList<StoredParValuesModel> storedParValues) where T : new()
        {

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        foreach (var item in storedParValues)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(item.ParmaName, item.ParmaValue));
                        }

                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                //SqlDataAdapter adapter = new SqlDataAdapter();
                                //  DataTable dataTable = new DataTable();
                                //dataTable.Load(reader);
                                await reader.ReadAsync();
                                if (!reader.IsDBNull(0))
                                {
                                    // Convert DataTable to JSON
                                    // string cat = JsonConvert.SerializeObject(dataTable);
                                    // categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(cat).ToList();
                                    //while (reader.Read())
                                    //{
                                    //    AlanoClubPricesModel pm = new AlanoClubPricesModel();


                                    //    pm.ID = reader.GetInt32("ID");

                                    //    pm.BarItem = reader.GetBoolean("BarItem");
                                    //    pm.ClubPrice = float.Parse(reader.GetString("ClubPrice"));
                                    //    pm.ClubNonMemberPrice = float.Parse(reader.GetString("ClubNonMemberPrice"));
                                    //    pm.ProductName = reader.GetString("ProductName");
                                    //    pm.CategoryName = reader.GetString("CategoryName");

                                    //    retList.Add(pm);
                                    //    // var json = reader.GetString(0);
                                    //}
                                    string json = reader.GetString(0);
                                    await reader.CloseAsync();
                                    //var jSonObj = JObject.Parse(json);
                                    //var str = jSonObj[model]?.ToString();
                                    var retJson = JsonConvert.DeserializeObject<T>(json);

                                    return retJson;
                                }


                            }
                            await reader.CloseAsync();

                        }





                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method CallStoreProdByParmaters for {storedProcedueName}  {ex.Message}");
            }

            return default(T);

        }
        public async Task UpDateInsertWithParma(string sqlConnectionStr, string storedProcedueName, IList<StoredParValuesModel> storedParValues)
        {

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        foreach (var item in storedParValues)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(item.ParmaName, item.ParmaValue));
                        }

                        sqlCmd.ExecuteNonQueryAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        await sqlCmd.Connection.CloseAsync();






                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method CallStoreProdByParmaters for {storedProcedueName}  {ex.Message}");
            }

            await Task.CompletedTask;

        }

        public async Task<bool> CheckMemberID(string sqlConnectionStr, string storedProcedueName, int memberID)
        {
            bool memberExists = false;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaMemberID, memberID));
                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                await reader.ReadAsync();

                                if (!(reader.IsDBNull(0)))
                                {

                                    memberExists = true;


                                }
                            }
                            await reader.CloseAsync();

                        }





                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method CheckMemberID for {storedProcedueName} for memnber id {memberID}  {ex.Message}");
            }
            return memberExists;
        }

        public async Task DeleteByID(string sqlConnectionStr, int id, string storedProcedueName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaID, id));


                        await sqlCmd.ExecuteNonQueryAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"DeleteByID for {storedProcedueName} for delete id {id} message: {ex.Message}");

            }
        }
        public IList<T> GetACProductsListNonAsc<T>(string sqlConnectionStr, string storedProcedueName)
        {
            //ALanoClubUtilites.ShowMessageBox("Connection to database established successfully.", "Information", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            IList<T> retList = new List<T>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {


                                if (reader.Read())
                                {

                                    if (!(reader.IsDBNull(0)))
                                    {
                                        //SqlDataAdapter adapter = new SqlDataAdapter();
                                        //  DataTable dataTable = new DataTable();
                                        //dataTable.Load(reader);

                                        // Convert DataTable to JSON
                                        // string cat = JsonConvert.SerializeObject(dataTable);
                                        // categories = JsonConvert.DeserializeObject<IList<CategoryModel>>(cat).ToList();

                                        string json = reader.GetString(0);
                                        retList = JsonConvert.DeserializeObject<IList<T>>(json).ToList();
                                    }


                                }
                            }
                            reader.Close();

                        }





                    }


                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error Method GetACProductsList<T> for {storedProcedueName} {typeof(T)} {ex.Message}");
            }
            return retList;
        }

        public int GetMaxID(string sqlConnectionStr, string storedProcedueName)
        {
            int retid = 0;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                if (!(reader.IsDBNull(0)))
                                {

                                    retid = reader.GetInt32(0);


                                }
                            }
                            reader.Close();

                        }





                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Method GetMaxID for {storedProcedueName}  {ex.Message}");
            }
            return retid;
        }

        public async Task AddSig(string sqlConnectionStr, int recnumber, byte[] sig, string storedProcedueName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaReceiptNumber, recnumber));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaReceiptSignature, sig));


                        await sqlCmd.ExecuteNonQueryAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"AddSig for {storedProcedueName} for recnumber {recnumber} message: {ex.Message}");

            }
        }
        public async Task<int> LogVolHrs(string sqlConnectionStr, string storedProcedueName)
        {

            //ALanoClubUtilites.ShowMessageBox("Connection to database established successfully.", "Information", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            int retid = 0;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaID, Utilites.ALanoClubUtilites.UserVolHrsID));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaUserID, LoginUserModel.LoginInstance.ID));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstProp.SPParmaDate, DateTime.Now));
                        using (SqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {


                                if (reader.ReadAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                                {

                                    if (!(reader.IsDBNull(0)))
                                    {
                                        retid = reader.GetInt32(0);
                                    }
                                }

                            }
                            reader.CloseAsync().ConfigureAwait(false).GetAwaiter().GetResult();


                        }


                    }
                }

            }


            catch (Exception ex)
            {

                throw new Exception($"Error Method LogVolHrs for {storedProcedueName}  {ex.Message}");
            }
            return retid;
        }
        public async Task AddItemStoreProd(string sqlConnectionStr, string storedProcedueName, string spParma, string spValue)
        {

            //ALanoClubUtilites.ShowMessageBox("Connection to database established successfully.", "Information", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            int retid = 0;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(spParma, spValue));
                        await sqlCmd.ExecuteReaderAsync();


                    }
                }

            }


            catch (Exception ex)
            {

                throw new Exception($"Error Method AddItemStoreProd for {storedProcedueName} for parma {spParma} value {spValue} {ex.Message}");
            }

        }
        public async Task<IList<string>> GetAlanoClubDataBaseNames(string sqlConnectionStr,string retDbNames=null)
        {
            IList<string> dbNames = new List<string>();

            try
            {
                using (var conn = new SqlConnection(sqlConnectionStr))
                {
                    await conn.OpenAsync();

                    using (var cmd = new SqlCommand(SqlConstProp.QueryGetDatabaseNames, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            var dName = reader["DatabaseName"].ToString();

                            dbNames.Add(dName);
                        }
                        if((dbNames != null) && (dbNames.Count > 0))
                        {
                            if(!(string.IsNullOrWhiteSpace(retDbNames)))
                                return dbNames.Where(p => p.ToLower().StartsWith(retDbNames.ToLower())).ToList();

                        }


                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error GetAlanoClubDataBaseNames {ex.Message}");
            }
            return dbNames;

        }
        public void Dispose()
        {


            // Suppress finalization to avoid calling the finalizer if already disposed
            GC.SuppressFinalize(this);
        }


    }
}

//  GetDailyInventoryByMonth<AlanoCLubDailyTillTapeModel>(SqlConnectionStr, Scmd.SqlConstProp.SPGetAlanoCLubReportItemsSoldByMonth, ReportSDate.Year);



