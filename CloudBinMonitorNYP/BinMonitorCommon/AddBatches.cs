using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using SqlCommands;
namespace BinMonitor.Common
{
  public  class AddBatches
    {
        public static AddBatches AddBatchesCloud = null;

        AddBatches()
        {
        }
        static AddBatches()
        {
            if (AddBatchesCloud == null)
                AddBatchesCloud = new AddBatches();
        }

        public void GetAllBinXmlFiles()
        {

            GetXmlFile(SqlCmd.SpGetUsersXmlFile, BinUtilities.BinMonUserFolder);
            GetXmlFile(SqlCmd.SpGetXmlBinUserProfiles, BinUtilities.BinMonUserProfilesFolder);
            GetXmlFile(SqlCmd.SpGetXmlFileBinCategories, BinUtilities.BinMonCategoriesFolder);
                GetXmlFile(SqlCmd.SpGetXmlFileBIns, BinUtilities.BinMonBinsFolder);
                GetXmlFile(SqlCmd.SpGetXmlFileBinsMasterCategories, BinUtilities.BinMonMasterCategoriesFolder);
        }
        public void GetXmlFile(string spName,string xmlPath)
        {
            SqlCmd CmdSql = new SqlCmd();
            SqlConnection sqlConnection = CmdSql.SqlConnection(string.Empty, string.Empty, string.Empty);
            BinUtilities.BinMointorUtilties.CreateDirectory(xmlPath);
           // xmlFile = string.Format("{0}.xml", xmlFile);
            using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpGetBatchXmlFile, null, sqlConnection, Guid.Empty))
            {
                while (dr.Read())
                {
                    string xmlFileName = dr[0].ToString();
                    string xmlFileData = dr[1].ToString();
                    string outXmlFileName = string.Format("{0}{1}.xml", xmlPath, xmlFileName);
                    BinUtilities.BinMointorUtilties.WriteOutPut(xmlFileName, xmlFileData);
                        
                }
            }

            //CmdSql.SqlDataReader(CmdSql.)
            if (sqlConnection.State == ConnectionState.Open)
                sqlConnection.Close();

        }

        public void UploadBatchXmlFile(string batchId)
        {
            SqlCmd CmdSql = new SqlCmd();
            SqlConnection sqlConnection = CmdSql.SqlConnection(string.Empty, string.Empty, string.Empty);
            string xmlFile = string.Format("{0}\\Local\\EdocsUsaBmC\\Data\\Specimen Batches\\{1}.xml",Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), batchId);
            //  string xmlFile = @"C:\Archives\Data\Specimen Batches\" + batchId + ".xml";
            string xmlFileInfo = string.Empty;
            using (StreamReader sr = new StreamReader(xmlFile))
            {
                xmlFileInfo = sr.ReadToEnd();
            }
            Dictionary<string, string> dicNewCat = new Dictionary<string, string>();

            dicNewCat.Add(SqlCmd.SpParmaBatchId, batchId);
            dicNewCat.Add(SqlCmd.SpParmaXmlFile, xmlFileInfo);
            dicNewCat.Add(SqlCmd.SpParmaProcessing, SqlCmd.ProcessingBatchTrue);
            SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpUploadBinXmlFile, dicNewCat, sqlConnection, Guid.Empty);
            dr.Close();
            //CmdSql.SqlDataReader(CmdSql.)
            if (sqlConnection.State == ConnectionState.Open)
                sqlConnection.Close();

        }
        public void AddNewCategory(SpecimenBatch SpecBatch)
        {
            SqlCmd CmdSql = new SqlCmd();
            SqlConnection sqlConnection = CmdSql.SqlConnection(string.Empty, string.Empty, string.Empty);
            Dictionary<string, string> dicNewCat = new Dictionary<string, string>();
            Guid batchGuid = Guid.NewGuid();
            dicNewCat.Add(SqlCmd.SpParmaBatchId, batchGuid.ToString());
            dicNewCat.Add(SqlCmd.SpParmaBinId, SpecBatch.BinId);
            dicNewCat.Add(SqlCmd.SpParmaCategorieId, SpecBatch.CategoryId);
            dicNewCat.Add(SqlCmd.SpParmaStartedAt, SpecBatch.CreatedAt.ToString());
            dicNewCat.Add(SqlCmd.SpParmaAssignedBy, SpecBatch.CreatedBy);
            dicNewCat.Add(SqlCmd.SpParmaAssignedTo, SpecBatch.CreatedBy);
            dicNewCat.Add(SqlCmd.SpParmaCompletedBy, SpecBatch.CreatedBy);
            SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpAddBinRegistration, dicNewCat, sqlConnection, Guid.Empty);
            dr.Close();
            dicNewCat.Clear();
            dicNewCat.Add(SqlCmd.SpParmaBatchId, batchGuid.ToString());
            if (string.IsNullOrEmpty(SpecBatch.Comments))
                dicNewCat.Add(SqlCmd.SpParmaComments, string.Empty);
            else
                dicNewCat.Add(SqlCmd.SpParmaComments, SpecBatch.Comments);
            if (SpecBatch.Specimens.Count == 0)
                dicNewCat.Add(SqlCmd.SpParmaContents, string.Empty);
            else
            {
                dicNewCat.Add(SqlCmd.SpParmaContents, string.Join(Environment.NewLine, SpecBatch.Specimens.ToArray()));
            }
            CmdSql.SqlDataReader(SqlCmd.SpAddUpDateBinCommentsContents, dicNewCat, sqlConnection, Guid.Empty);

            //CmdSql.SqlDataReader(CmdSql.)
            if (sqlConnection.State == ConnectionState.Open)
                sqlConnection.Close();
        }
    }
}
