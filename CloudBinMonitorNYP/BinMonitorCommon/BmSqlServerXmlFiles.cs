using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using SqlCommands;
namespace BinMonitor.Common
{
    public class BmSqlServerXmlFiles
    {
        public static BmSqlServerXmlFiles AddBatchesCloud = null;

        BmSqlServerXmlFiles()
        {
        }
        static BmSqlServerXmlFiles()
        {
            if (AddBatchesCloud == null)
                AddBatchesCloud = new BmSqlServerXmlFiles();
        }
       
        public void LoadBinUsersXmlFiles()
        {
            GetXmlFile(SqlCmd.SpGetUsersXmlFile, BinUtilities.BinMonUserFolder);
        }

        public void LoadBinUserProfiles()
        {
            GetXmlFile(SqlCmd.SpGetXmlBinUserProfiles, BinUtilities.BinMonUserProfilesFolder);
        }

        public void LoadBinXmlFileBinCategories()
        {
            GetXmlFile(SqlCmd.SpGetXmlFileBinCategories, BinUtilities.BinMonCategoriesFolder);
        }

        public void LoadBinXmlFileBIns()
        {
            GetXmlFile(SqlCmd.SpGetXmlFileBIns, BinUtilities.BinMonBinsFolder);
        }

        public void LoadXmlFileBinsMasterCategories()
        {
            GetXmlFile(SqlCmd.SpGetXmlFileBinsMasterCategories, BinUtilities.BinMonMasterCategoriesFolder);
        }
        public void LoadXmlFileSpecimenBatches()
        {

            GetXmlFile(SqlCmd.SpGetXmlSpecimenBatches, BinUtilities.BinMonSpecimenBatchesFolder);
        }
        public void AddUpDateBmUsersXmlFiles(string userName,string cardID)
        {
            string xmlFile = string.Format(@"{0}\{1}.xml", BinUtilities.BinMonUserFolder,userName);
            string xmlData = BinUtilities.BinMointorUtilties.ReadFile(xmlFile);
            Dictionary<string, string> dicUserInfor = new Dictionary<string, string>();
            dicUserInfor.Add(SqlCmd.SpParmaCwid, cardID);
            dicUserInfor.Add(SqlCmd.SpParmaUserName, userName);
            dicUserInfor.Add(SqlCmd.SpParmaXmlFile, xmlData);
            SqlCmd CmdSql = new SqlCmd();
            try
            { 
            using (SqlConnection sqlConnection = CmdSql.SqlConnection())
            {
                using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpUploadXmlFileUsers, dicUserInfor, sqlConnection, Guid.Empty))
                {
                }
              //  var sd =  new SerializedObjectDictionary<T>(BinUtilities.BinMonUserFolder);

            }
            }
            catch(Exception ex)
            {
                Trace.TraceError(string.Format("Adding user:{0} for cardID:{1} message:{2}", userName, cardID, ex.Message));
                throw new Exception (string.Format("Adding user:{0} for cardID:{1} message:{2}", userName, cardID, ex.Message));
            }
        }
        public void ReloadAllBinMomitorXmlFiles(bool reload)
        {

            LoadBinUsersXmlFiles();
            LoadBinUserProfiles();
            LoadBinXmlFileBinCategories();
            LoadBinXmlFileBIns();
            LoadXmlFileBinsMasterCategories();
            LoadXmlFileSpecimenBatches();
            if (reload)
            {
                Users.Instance.DirectoryPath = BinUtilities.BinMonUserFolder;
                Users.Instance.Reload();
                UserProfiles.Instance.DirectoryPath = BinUtilities.BinMonUserProfilesFolder;
                UserProfiles.Instance.Reload();
                MasterCategories.Instance.DirectoryPath = BinUtilities.BinMonMasterCategoriesFolder;
                MasterCategories.Instance.Reload();
                Bins.Instance.DirectoryPath = BinUtilities.BinMonBinsFolder;
                Bins.Instance.Reload();
                SpecimenBatches.Instance.DirectoryPath = BinUtilities.BinMonSpecimenBatchesFolder;
                SpecimenBatches.Instance.Reload();

            }

        }

        public void UpdateCategorieXmlFile(string catName)
        {
            SqlCmd CmdSql = new SqlCmd();
            Trace.TraceInformation(string.Format("Method public void UpdateCategorieXmlFile(string catName) for cat:{0}", catName));
            try
            {
                string catID = catName;
                     
                catName = string.Format(@"{0}\{1}.xml", BinUtilities.BinMonCategoriesFolder, catName);
                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {
                    string catXml = BinUtilities.BinMointorUtilties.ReadFile(catName);
                    Dictionary<string, string> dicCat = new Dictionary<string, string>();
                    dicCat.Add(SqlCmd.SpParmaCategorieId, catID);
                    dicCat.Add(SqlCmd.SpParmaXmlFile,catXml);

                    // xmlFile = string.Format("{0}.xml", xmlFile);
                    using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpUpDateXmlFileBinCategorie,dicCat, sqlConnection, Guid.Empty))
                    { }
                        }
            }
            catch(Exception ex)
            {
                Trace.TraceError(string.Format("Updating categorie:{0} message:{1}", catName, ex.Message));
                throw new Exception(string.Format("Updating categorie:{0} message:{1}", catName, ex.Message));
            }
        }
        public void XmlFileUserProfiles(string profileName,string addDelete)
        {
            SqlCmd CmdSql = new SqlCmd();
            Trace.TraceInformation(string.Format("Method public void XmlFileUserProfiles(string profileName,string addDelete) profilename:{0} adddelete:{1}", profileName, addDelete));
            try
            {
               string proXmlFileName = string.Format(@"{0}\{1}.xml", BinUtilities.BinMonUserProfilesFolder,profileName);
                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {
                    string proXml = BinUtilities.BinMointorUtilties.ReadFile(proXmlFileName);
                    Dictionary<string, string> dicProf = new Dictionary<string, string>();
                    dicProf.Add(SqlCmd.SpParmaUserProfile, profileName.ToUpper());
                    dicProf.Add(SqlCmd.SpParmaXmlFile, proXml);
                    dicProf.Add(SqlCmd.SpParmaDelete, addDelete);

                    // xmlFile = string.Format("{0}.xml", xmlFile);
                    using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpXmlBinUserProfiles, dicProf, sqlConnection, Guid.Empty))
                    { }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Updating profile:{0} for adddelete: {1} message:{2}", profileName,addDelete, ex.Message));
                throw new Exception(string.Format("Updating profile:{0} for adddelete: {1} message:{2}", profileName, addDelete, ex.Message));
            }
        }
        public void GetXmlFile(string spName, string xmlPath)
        {
            Trace.TraceInformation(string.Format("Methodpublic void GetXmlFile(string spName, string xmlPath) for spname:{0} xmlpath:{1}", spName, xmlPath));
            SqlCmd CmdSql = new SqlCmd();
            try
            { 
            using (SqlConnection sqlConnection = CmdSql.SqlConnection())
            {
                BinUtilities.BinMointorUtilties.CreateDirectory(xmlPath);
                // xmlFile = string.Format("{0}.xml", xmlFile);
                using (SqlDataReader dr = CmdSql.SqlDataReader(spName, null, sqlConnection, Guid.Empty))
                {
                    while (dr.Read())
                    {
                        string xmlFileName = dr[0].ToString();
                        string xmlFileData = dr[1].ToString();
                        string outXmlFileName = string.Format("{0}\\{1}.xml", xmlPath, xmlFileName);
                        BinUtilities.BinMointorUtilties.WriteOutPut(outXmlFileName, xmlFileData);

                    }
                }
            }
            }
            catch(Exception ex)
            {
                Trace.TraceError(string.Format("Getting xml file stored procedure:{0} file path:{1} message:{2}", spName, xmlPath, ex.Message));
                throw new Exception(string.Format("Getting xml file stored procedure:{0} file path:{1} message:{2}", spName,xmlPath, ex.Message));
            }
            //CmdSql.SqlDataReader(CmdSql.)

        }

        public void UploadSpecimenBatchesXmlFile(string batchId,string binId,string processing)
        {
            Trace.TraceInformation(string.Format("Method public void UploadSpecimenBatchesXmlFile(string batchId,string binId,string processing) batchid:{0} binid{1} processing:{2}", batchId, binId, processing));
            SqlCmd CmdSql = new SqlCmd();
            try
            {
                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {
                    string xmlFile = string.Format("{0}\\{1}.xml", BinUtilities.BinMonSpecimenBatchesFolder, batchId);
                    string xmlFileInfo = BinUtilities.BinMointorUtilties.ReadFile(xmlFile);
                    Dictionary<string, string> dicNewCat = new Dictionary<string, string>();

                    dicNewCat.Add(SqlCmd.SpParmaBatchId, batchId);
                    dicNewCat.Add(SqlCmd.SpParmaXmlFile, xmlFileInfo);
                    dicNewCat.Add(SqlCmd.SpParmaBinId, binId);
                    dicNewCat.Add(SqlCmd.SpParmaProcessing, processing);
                    using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpUploadSpecimenBatchesXmlFile, dicNewCat, sqlConnection, Guid.Empty))
                    { }
                    // SpecimenBatches.Instance.DirectoryPath = BinUtilities.BinMonSpecimenBatchesFolder;
                    // SpecimenBatches.Instance.Reload(batchId);
                }
            }
            catch(Exception ex)
            {
                Trace.TraceError(string.Format("UploadSpecimenBatchesXmlFile batchid:{0} bind id:{1} processing:{2} message:{3}",batchId,binId,processing, ex.Message));
                throw new Exception(string.Format("UploadSpecimenBatchesXmlFile batchid:{0} bind id:{1} processing:{2} message:{3}", batchId, binId, processing, ex.Message));
            }

            
          //  Bins.Instance.DirectoryPath = BinUtilities.BinMonBinsFolder;
           // Bins.Instance.Reload();

        }

    
        public void UpDateBinXmlFile(string binId)
        {
            Trace.TraceInformation(string.Format("Method public void UpDateBinXmlFile(string binId) binid:{0}", binId));
            SqlCmd CmdSql = new SqlCmd();
            try
            {
                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {
                    string xmlFile = string.Format("{0}\\{1}.xml", BinUtilities.BinMonBinsFolder, binId);
                    //  string xmlFile = @"C:\Archives\Data\Specimen Batches\" + batchId + ".xml";
                    string xmlFileInfo = BinUtilities.BinMointorUtilties.ReadFile(xmlFile);
                    Dictionary<string, string> dicNewCat = new Dictionary<string, string>();
                    dicNewCat.Add(SqlCmd.SpParmaBinId, binId);
                    dicNewCat.Add(SqlCmd.SpParmaXmlFile, xmlFileInfo);
                    using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpUpdateXmlFileBIns, dicNewCat, sqlConnection, Guid.Empty))
                    { }
                    //UserBinsBatches();

                    //Bins.Instance.DirectoryPath = BinUtilities.BinMonBinsFolder;
                    ///   Bins.Instance.Reload(binId);
                }
            }

            catch(Exception ex)
            {
                Trace.TraceError(string.Format("Updating bin xml file bind id:{0} message:{1}",binId, ex.Message));
                throw new Exception(string.Format("Updating bin xml file bind id:{0} message:{1}", binId, ex.Message));
            }


        }

        public void GetActiveBinsXmlFile(string excludeBin)
        {
            Trace.TraceInformation(string.Format("method public void GetActiveBinsXmlFile(string excludeBin:{0})", excludeBin));
            SqlCmd CmdSql = new SqlCmd();
            try
            {
                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {
                    BinUtilities.BinMointorUtilties.CreateDirectory(BinUtilities.BinMonBinsFolder);
                    Dictionary<string, string> dicActiveBins = new Dictionary<string, string>();
                    dicActiveBins.Add(SqlCmd.SpParmaBinId, excludeBin);
                    using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpActiveBins, dicActiveBins, sqlConnection, Guid.Empty))
                    {
                        while (dr.Read())
                        {
                            string xmlFileName = dr[0].ToString();
                            string xmlFileData = dr[1].ToString();
                            string outXmlFileName = string.Format("{0}\\{1}.xml", BinUtilities.BinMonBinsFolder, xmlFileName);
                            BinUtilities.BinMointorUtilties.WriteOutPut(outXmlFileName, xmlFileData);

                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Trace.TraceError(string.Format("Getting active bins message:{0}",ex.Message));
                throw new Exception(string.Format("Getting active bins message:{0}", ex.Message));
            }
            //CmdSql.SqlDataReader(CmdSql.)

        }

     

  
    }
}
