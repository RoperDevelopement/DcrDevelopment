using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using NYPMigration.Models;
using NYPMigration.ProcessRecords;
using NYPMigration.Utilities;
using System.Data.SQLite;
using edl = EdocsUSA.Utilities.Logging;
//https://zetcode.com/csharp/sqlite/
namespace NYPMigration.SQLCmds
{
  public  class SqlLiteCmds
    {
        private static SqlLiteCmds instance = null;
        public static SqlLiteCmds SqlLiteCmdsInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SqlLiteCmds();
                }
                return instance;
            }
        }
        private SqlLiteCmds()
        { }
        public async Task<int> AddLabReqs(string pdfFolder, NYPLabReqsModel labReqs,string sqliteDB,string tableName)
        {
            int  result=0;
            try
            {
                if (string.IsNullOrWhiteSpace(labReqs.MRN))
                    labReqs.MRN = labReqs.PatientID;
                if (string.IsNullOrWhiteSpace(labReqs.RequisitionNumber))
                    labReqs.RequisitionNumber = labReqs.FinancialNumber;
                string connectionString = $"Data Source={System.IO.Path.Combine(Utilities.PropertiesConst.PropertiesConstInstance.SqlLightDatabase,sqliteDB)}";
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string sql = $"INSERT INTO {tableName} (ID,FileName,FileDirectory,BatchID,IndexNumber,FinancialNumber,DateOfService,DrID,PatientID,ClientID,ReqisitionNumber,ClientCode,ScanDate,MRN) VALUES ('{labReqs.ID}','{labReqs.FileName}.pdf','{pdfFolder}','{labReqs.BatchID}','{labReqs.IndexNumber}','{labReqs.FinancialNumber}','{labReqs.DateOfService.ToString("MM-dd-yyyyy")}','{labReqs.DrID}','{labReqs.PatientID}','{labReqs.ClientID}','{labReqs.RequisitionNumber}','{labReqs.ClientCode}','{labReqs.ScanDate.ToString("MM-dd-yyyyy")}','{labReqs.MRN}')";
                    // Your code to interact with the database goes here
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.CommandText = sql;
                         result = command.ExecuteNonQueryAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                      
                    }
                }
            }
            catch (Exception ex)
            {
                if((ex.Message.ToLower().Contains("locked")))

                    PropertiesConst.PropertiesConstInstance.WriteWarnings($"Warning Adding records to sqlite table for labreqs for id {labReqs.ID} for file {labReqs.FileName} for save folder {pdfFolder} {ex.Message}");
                else
                    PropertiesConst.PropertiesConstInstance.UpdateErrors($"Error Adding records to sqlite table for labreqs for id {labReqs.ID} for file {labReqs.FileName} for save folder {pdfFolder} {ex.Message}");
                
            }
            return result;
        }

        public async Task<int> AddLabReqsPatID(LabReqsPatientIDModel patientID,string sqliteDB)
        {
            int result = 0;
            string connectionString = $"Data Source={System.IO.Path.Combine(Utilities.PropertiesConst.PropertiesConstInstance.SqlLightDatabase, sqliteDB)}";
            try
            {
                 
               
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string sql = $"INSERT INTO Patient (PatientID,PatientFirstName,PatientLastName,ClientCode) VALUES ('{patientID.PatientID}','{patientID.PatientFirstName.Replace("'"," ")}','{patientID.PatientLastName.Replace("'", " ")}','{patientID.ClientCode}')";
                    // Your code to interact with the database goes here
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.CommandText = sql;
                        result = command.ExecuteNonQueryAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        // Add parameters and their values
                        //command.Parameters.AddWithValue("@FileName", $"{labReqs}.pdf");
                        //command.Parameters.AddWithValue("@FileDirectory", pdfFolder);
                        //command.Parameters.AddWithValue("@BatchID", labReqs.BatchID);
                        //command.Parameters.AddWithValue("@IndexNumber", labReqs.IndexNumber);
                        //command.Parameters.AddWithValue("@FinancialNumber", labReqs.FinancialNumber);
                        //command.Parameters.AddWithValue("@DateOfService", labReqs.DateOfService.ToString());

                        //command.Parameters.AddWithValue("@DrID", labReqs.DrID);
                        //command.Parameters.AddWithValue("@PatientID", labReqs.PatientID);
                        //command.Parameters.AddWithValue("@ClientID", labReqs.ClientID);
                        //command.Parameters.AddWithValue("@Client Code", labReqs.ClientCode);
                        //command.Parameters.AddWithValue("@ScanDate", labReqs.ScanDate.ToString());
                        //command.Parameters.AddWithValue("@MRN", labReqs.MRN);


                        // Execute the command
                        //  var result = command.ExecuteNonQuery();

                        // Output the result
                        //  Console.WriteLine($"Number of rows inserted: {result}");
                    }
                }
            }
            catch (Exception ex)
            {
                if ((ex.Message.ToLower().Contains("locked")))
                    PropertiesConst.PropertiesConstInstance.WriteWarnings($"Warning Adding records to SqlLite Table For patient id {patientID.PatientID} for connection string {connectionString} {ex.Message}");
                else
                            PropertiesConst.PropertiesConstInstance.UpdateErrors($"Error Adding records to SqlLite Table For patient id {patientID.PatientID} for connection string {connectionString} {ex.Message}");
                
            }
            return result;
        }
        public async Task<int> AddRecordsSqliteTabe(string insertCMd,string sqliteDB)
        {
            int result = 0;
            string connectionString = $"Data Source={System.IO.Path.Combine(Utilities.PropertiesConst.PropertiesConstInstance.SqlLightDatabase, sqliteDB)}";
            try
            {
               
               
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                  //  string sql = $"INSERT INTO LabReqisitions (ID,FileName,FileDirectory,BatchID,IndexNumber,FinancialNumber,DateOfService,DrID,PatientID,ClientID,ReqisitionNumber,ClientCode,ScanDate,MRN) VALUES ('{labReqs.ID}','{labReqs.FileName}.pdf','{pdfFolder}','{labReqs.BatchID}','{labReqs.IndexNumber}','{labReqs.FinancialNumber}','{labReqs.DateOfService.ToString("MM-dd-yyyyy")}','{labReqs.DrID}','{labReqs.PatientID}','{labReqs.ClientID}','{labReqs.RequisitionNumber}','{labReqs.ClientCode}','{labReqs.ScanDate.ToString("MM-dd-yyyyy")}','{labReqs.MRN}')";
                    // Your code to interact with the database goes here
                    using (var command = new SQLiteCommand(insertCMd, connection))
                    {
                        command.CommandText = insertCMd;
                        result = command.ExecuteNonQueryAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                         
                    }
                }
            }
            catch (Exception ex)
            {
                if ((ex.Message.ToLower().Contains("locked")))
                    PropertiesConst.PropertiesConstInstance.WriteWarnings($"Warning Adding Sqlite records insert cmd {insertCMd} connection string  {connectionString}  {ex.Message}");
                
                else
                    PropertiesConst.PropertiesConstInstance.UpdateErrors($"Error Adding Sqlite records insert cmd {insertCMd} connection string  {connectionString} {ex.Message}");
            }
            return result;
        }
    }
}
