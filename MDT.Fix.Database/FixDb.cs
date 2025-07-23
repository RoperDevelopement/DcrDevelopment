using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using MDT.Fix.Database.Properties;
using MDT.Fix.Database.Models;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using static System.Net.WebRequestMethods;
using System.Xml.Linq;

namespace MDT.Fix.Database
{
    class FixDb
    {
        static readonly string MDTFolder = @"M:\MDTPDFFiles\";
        static readonly string Header = "Message,Path,ID,TrackingID,BatchID,ScanDate";
        static readonly string SaveFolderFile = @"d:\MissingDBMDTFiles\MissingDbFiles.csv";
        static StringBuilder sb = new StringBuilder();
        static void Main(string[] args)
        {

            try
            {
                sb.AppendLine(Header);
                if (args.Length == 0)
                    ProcessUploadFiles().ConfigureAwait(false).GetAwaiter().GetResult();
                else
                    CompareFoldersDB(args[0]).ConfigureAwait(false).GetAwaiter().GetResult(); ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static async Task<SqlConnection> OpenSqlConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(GetConnectionString().ConfigureAwait(false).GetAwaiter().GetResult());
            sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            return sqlConnection;
        }
        private static async Task CompareFoldersDB(string srcFolder)
        {
            IDictionary<string, string> dicFilesFolders = new Dictionary<string, string>();
            IDictionary<string, string> dicDBFilesFolders = new Dictionary<string, string>();
            IDictionary<string, MDTModel> dicDBRecs = new Dictionary<string, MDTModel>();
            IList<string> listFolders = new List<string>();
            foreach (var files in Directory.GetFiles(srcFolder, "*.pdf", SearchOption.AllDirectories))
            {
                string fName = Path.GetFileName(files);
                if (!(dicFilesFolders.ContainsKey(fName)))
                    dicFilesFolders.Add(fName, files);
                else
                    sb.AppendLine($"Dup file in folder found {fName} dir path  {files},ID,TrackingID,BatchID,ScanDate");
                listFolders.Add(files);
            }
            foreach (var recs in GetUploadRecords())
            {
                string fName = Path.GetFileName(recs.FileName).Trim();
                if (!(dicDBFilesFolders.ContainsKey(fName)))
                {
                    dicDBFilesFolders.Add(fName, recs.FileName);
                    dicDBRecs.Add(fName, recs);
                }
                  
                else
                    sb.AppendLine($"Dup file database found {fName}, {recs.FileName},{recs.ID},{recs.TrackingID},{recs.BatchID},{recs.DateUploaded}");
               
            }
            CompareDictonary(dicFilesFolders, dicDBRecs).GetAwaiter().GetResult();
            SaveFile();
        }
        public static async Task CompareDictonary(IDictionary<string, string> sourceDic, IDictionary<string, MDTModel> destDic)
        {
            foreach (KeyValuePair<string, string> key in sourceDic)
            {
                if (destDic.TryGetValue(key.Key, out MDTModel dest))
                {
                    if (string.Compare(key.Value, dest.FileName, true) != 0)
                    {
                        Console.WriteLine($"Dir Path in db {dest.FileName} dir path on drive {key.Value},{dest.ID},{dest.TrackingID},{dest.BatchID},{dest.DateUploaded}");
                        UpdateDrivePathInDB(dest.ID,key.Value).GetAwaiter().GetResult();
                         sb.AppendLine($"Dir Path in db {dest.FileName} dir path on drive {key.Value},{dest.ID},{dest.TrackingID},{dest.BatchID},{dest.DateUploaded}");
                        sb.AppendLine("");
                    }
                    //else
                    //    sb.Append($"Dir file {key.Key} path {key.Value} path not same in db {dest},ID,TrackingID,BatchID,ScanDate");
                }
                else
                {
                    sb.AppendLine($"File {key.Key} not found in db,{key.Value}, , , ,");
                    sb.AppendLine("");
                }
                     
            }
        }
        public static Dictionary<K, V> Merge<K, V>(IEnumerable<Dictionary<K, V>> dictionaries)
        {
            return dictionaries.SelectMany(x => x)
                            .ToDictionary(x => x.Key, y => y.Value);
        }
        private static async Task<string> GetConnectionString()
        {


            //return string.Format("Server={0}; Database={1};Trusted_Connection = True; User Id={2};Integrated Security=true;Connect Timeout=120;", Settings.Default.SqlSever, Settings.Default.SqlDataBase, Settings.Default.DbUserName);

            return string.Format("Server={0}; Database={1};Trusted_Connection = True; User Id={2};PassWord={3};Connect Timeout={4};", Settings.Default.SqlSever, Settings.Default.SqlDataBase, Settings.Default.DbUserName, Settings.Default.DbPassWord, Settings.Default.SqlServerTimeOut);

        }
        private static async Task ProcessUploadFiles()
        {

            List<string> files = new List<string>();

            foreach (var recs in GetUploadRecords())
            {
                string retStr = string.Empty;
                Console.WriteLine($"Processing file {recs.FileName}");
                if (!(System.IO.File.Exists(recs.FileName)))
                {
                    sb.AppendLine($"File not found,{recs.FileName},{recs.ID},{recs.TrackingID},{recs.BatchID},{recs.DateUploaded.ToString()}");
                    retStr = SearchForFile(recs.FileName, recs.ID.ToString(), recs.TrackingID, recs.BatchID, recs.DateUploaded).ConfigureAwait(false).GetAwaiter().GetResult();
                    Console.WriteLine($"{recs.FileName} file not found");
                }
                else
                    SearchForFile(recs.FileName, recs.ID.ToString(), recs.TrackingID, recs.BatchID, recs.DateUploaded).ConfigureAwait(false).GetAwaiter().GetResult();
                string fName = Path.GetFileName(recs.FileName);
                if (!(files.Contains(fName)))
                    files.Add(fName);
                else
                {
                    sb.AppendLine($"Duplicate file found,{recs.FileName},{recs.ID},{recs.TrackingID},{recs.BatchID},{recs.DateUploaded.ToString()}");
                    // sb.AppendLine($"{recs.FileName} file not found");
                    retStr = SearchForFile(recs.FileName, recs.ID.ToString(), recs.TrackingID, recs.BatchID, recs.DateUploaded).ConfigureAwait(false).GetAwaiter().GetResult();
                }

                if (!(string.IsNullOrWhiteSpace(retStr)))
                    sb.AppendLine(retStr);
            }
            SaveFile();
        }
        static void SaveFile()
        {
            if (!(Directory.Exists(Path.GetDirectoryName(SaveFolderFile))))
                Directory.CreateDirectory(Path.GetDirectoryName(SaveFolderFile));
            if (System.IO.File.Exists(SaveFolderFile))
                System.IO.File.Delete(SaveFolderFile);


            System.IO.File.WriteAllText(SaveFolderFile, sb.ToString());
        }
        static async Task<string> SearchForFile(string path, string id, string tID, string batchID, DateTime dateTime)
        {
            string file = Path.GetFileName(path);
            string[] files = Directory.GetFiles(MDTFolder, file, SearchOption.AllDirectories);
            if (files.Count() == 0)
            {
                return $"Search for File Not Found,{path},{id},{tID},{batchID},{dateTime.ToString()}";
            }
            else if (files.Count() == 1)
            {
                return $"Search  for file Found, {files[0]},{id},{tID},{batchID},{dateTime.ToString()}";
            }
            else
            {
                string dupFiles = string.Empty;
                string foundFiles = string.Empty;
                foreach (var f in files)
                {
                    dupFiles += $"{f} ";
                }
                foundFiles = $"Search for dup files path {path} total dup files {files.Count().ToString()} ,Dup files found path {dupFiles},{id},{tID},{batchID},{dateTime.ToString()}";
                return foundFiles;
            }

        }
        private static async Task  UpdateDrivePathInDB(int id, string filePath)
        {
            SqlConnection sqlConn = OpenSqlConnection().ConfigureAwait(false).GetAwaiter().GetResult();
            using (SqlCommand cmd = new SqlCommand("sp_GetMDTUploadRecords", sqlConn))
            {
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"USE [EdocsITS] UPDATE[dbo].[EdocsITSTrackingIDByProjectName] SET [FileName] = '{filePath}' WHERE id = {id}  ";
                using (SqlDataReader reader = cmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    reader.ReadAsync().GetAwaiter().GetResult();
                }

            }
        }
        private static IEnumerable<MDTModel> GetUploadRecords()
        {
            SqlConnection sqlConn = OpenSqlConnection().ConfigureAwait(false).GetAwaiter().GetResult();
            using (SqlCommand cmd = new SqlCommand("sp_GetMDTUploadRecords", sqlConn))
            {
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = cmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    while (reader.ReadAsync().GetAwaiter().GetResult())
                    {
                        MDTModel mDT = new MDTModel();
                        mDT.BatchID = reader["BatchID"].ToString();
                        mDT.ID = int.Parse(reader["ID"].ToString());
                        mDT.DateUploaded = DateTime.Parse(reader["DateUploaded"].ToString());
                        mDT.EdocsCustomerID = reader["EdocsCustomerID"].ToString();
                        mDT.FileName = reader["FileName"].ToString();
                        mDT.TrackingID = reader["TrackingID"].ToString();
                        mDT.NumberDocsScanned = int.Parse(reader["NumberDocsScanned"].ToString());
                        mDT.NumberDocsUploaded = int.Parse(reader["NumberDocsUploaded"].ToString());
                        mDT.NumberDocOCR = int.Parse(reader["NumberDocOCR"].ToString());
                        yield return mDT;
                    }
                }

            }
        }
    }
}
