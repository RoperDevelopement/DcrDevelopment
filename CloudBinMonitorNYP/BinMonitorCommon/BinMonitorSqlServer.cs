using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SqlCommands;
using System.Diagnostics;

namespace BinMonitor.Common
{
    public class BinMonitorSqlServer
    {
        public static BinMonitorSqlServer SqlServerInstance = null;

        BinMonitorSqlServer()
        {
        }
        static BinMonitorSqlServer()
        {
            if (SqlServerInstance == null)
                SqlServerInstance = new BinMonitorSqlServer();
        }

        #region methods

        public void UserBinsBatches()
        {
            Trace.TraceInformation("Running method public void UserBinsBatches()");
            SqlCmd CmdSql = new SqlCmd();
            try
            {
                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {
                    using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpActiveBins, sqlConnection))
                    {
                        while (dr.Read())
                        {
                            string binIdXmlFileName = dr[SqlCmd.SqlAliasBinId].ToString();
                            string binIdXmlFile = dr[SqlCmd.SqlAliasBinXmlFile].ToString();
                            string batchesId = dr[SqlCmd.SqlAliasBatchId].ToString();
                            string batchesXmlFile = dr[SqlCmd.SqlAliasBatchXmlFile].ToString();
                            Bins.Instance.DirectoryPath = BinUtilities.BinMonBinsFolder;
                            SpecimenBatches.Instance.DirectoryPath = BinUtilities.BinMonSpecimenBatchesFolder;

                            if (!(SpecimenBatches.Instance.ContainsKey(batchesId)))
                            {
                                SpecimenBatch value = null;
                                User userValue = null;
                                string outXmlFileSpecBatches = string.Format("{0}\\{1}.xml", BinUtilities.BinMonSpecimenBatchesFolder, batchesId);
                                string outXmlFileBins = string.Format("{0}\\{1}.xml", BinUtilities.BinMonBinsFolder, binIdXmlFileName);
                                BinUtilities.BinMointorUtilties.WriteOutPut(outXmlFileSpecBatches, batchesXmlFile);
                                BinUtilities.BinMointorUtilties.WriteOutPut(outXmlFileBins, binIdXmlFile);

                                if (SpecimenBatches.Instance.TryDeserialize(outXmlFileSpecBatches, out value))
                                {
                                    if (string.IsNullOrEmpty(value.CreatedBy))
                                        return;
                                    Dictionary<string, string> dicUserInfo = new Dictionary<string, string>();
                                    dicUserInfo.Add(SqlCmd.SpParmaCwid, value.CreatedBy);
                                    using (SqlDataReader drUserInfo = CmdSql.SqlDataReader(SqlCmd.SpGetUsersXmlFileByCwid, dicUserInfo, sqlConnection))
                                    {

                                        while (drUserInfo.Read())
                                        {

                                            string userFile = string.Format("{0}\\{1}.xml", BinUtilities.BinMonUserFolder, drUserInfo[0].ToString());
                                            if (!(Users.Instance.TryDeserialize(userFile, out userValue)))
                                                return;
                                        }
                                        SpecimenBatches.Instance.Add(value.Id, value);
                                        SpecimenBatchManager.AssignNewBatch(value, value.BinId, userValue);
                                    }


                                }
                                //}
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Getting active bins message:{0}", ex.Message));
                throw new Exception(string.Format("Getting active bins message:{0}", ex.Message));
            }
        }
        public void UpLoadLogFile()
        {
            SqlCmd CmdSql = new SqlCmd();


            using (SqlConnection sqlConnection = CmdSql.SqlConnection())
            {
                string userName = Environment.UserName;
                string MachineName = Environment.MachineName;

                foreach (string fName in BinUtilities.BinMointorUtilties.GetDirFilesName(BinUtilities.BinMonLogFolder))
                {
                    try
                    {

                        string fileLocation = string.Format("{0}\\{1}", BinUtilities.BinMonLogFolder, fName);
                        string logInfo = BinUtilities.BinMointorUtilties.ReadFile(fileLocation);
                        Dictionary<string, string> dicLogFile = new Dictionary<string, string>();
                        dicLogFile.Add(SqlCmd.SpParmaLogFileName, fName);
                        dicLogFile.Add(SqlCmd.SpParmaUserName, userName);
                        dicLogFile.Add(SqlCmd.SpParmaComputerName, MachineName);
                        dicLogFile.Add(SqlCmd.SpParmaLogFile, logInfo);
                        using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpUpLoadLogFiles, dicLogFile, sqlConnection, Guid.Empty))
                        {

                        }

                    }
                    catch { }
                }
            }

        }
        public string AddBinMonitorChages(string emailAddress, string emailSubject, string issuesFound)
        {
            string retStr = string.Empty;
            try
            {
                SqlCmd CmdSql = new SqlCmd();


                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {



                    Dictionary<string, string> dicIssues = new Dictionary<string, string>();
                    dicIssues.Add(SqlCmd.SpParmaEmailTo, emailAddress);
                    dicIssues.Add(SqlCmd.SpParmaEmailSubject, emailSubject);
                    dicIssues.Add(SqlCmd.SpParmaComments, issuesFound);

                    using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpBinMonitorChanges, dicIssues, sqlConnection, Guid.Empty))
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(String.Format("Adding issues found for email address:{0} email subject:{1} issues:{2} meessage:{3}", emailAddress, emailSubject, issuesFound, ex.Message));
                retStr = String.Format("Error adding issues found for email address:{0} email subject:{1} error meessage:{2} contact edocs support", emailAddress, emailSubject, ex.Message);
            }
            return retStr;
        }




        public void GetUnactiveBatches()
        {
            string batchesIds = string.Empty;

            Trace.TraceInformation("In method public void GetUnactiveBatches()");
            SqlCmd CmdSql = new SqlCmd();
            try
            {
                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {
                    foreach (var batch in SpecimenBatches.Instance.Values)
                    {
                        batchesIds = batch.Id;
                        if (!(string.IsNullOrEmpty(batchesIds)))
                        {
                            Dictionary<string, string> dicUnActiveBins = new Dictionary<string, string>();
                            dicUnActiveBins.Add(SqlCmd.SpParmaBatchId, batchesIds);
                            using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpGetUnactiveBatches, dicUnActiveBins, sqlConnection, Guid.Empty))
                            {
                                if (dr.HasRows)
                                {
                                    dr.Read();
                                    if ((SpecimenBatches.Instance.ContainsKey(dr[0].ToString())))
                                    {
                                        Trace.TraceInformation(string.Format("Found closed spectrum batch:{0}", batchesIds));
                                        string outXmlFileSpecBatches = string.Format("{0}\\{1}.xml", BinUtilities.BinMonSpecimenBatchesFolder, batchesIds);
                                        BinUtilities.BinMointorUtilties.WriteOutPut(outXmlFileSpecBatches, dr[1].ToString());
                                        SpecimenBatch spBatch = null;
                                        if (SpecimenBatches.Instance.TryGetValue(dr[0].ToString(), out spBatch))
                                        {
                                            Bin bin = Bins.Instance.EnsureGetValue(spBatch.BinId);
                                            SpecimenBatches.Instance.EnsureMoveToArchive(spBatch.Id);
                                            spBatch.NotifyClosed();
                                            bin.Clear();
                                            Trace.TraceInformation(string.Format("Closed bin:{0}", spBatch.BinId));
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(String.Format("Getting unactive bactches for batch id:{0} message:{1}", batchesIds, ex.Message));
                throw new Exception(String.Format("Getting unactive bactches for batch id:{0} message:{1}", batchesIds, ex.Message));

            }
        }
        private void CategoryFreq(Category categoryFreq)
        {
            {
                Dictionary<string, string> catFreq = new Dictionary<string, string>();
                catFreq.Add(SqlConstants.SpParmaCategorieId, categoryFreq.Title);
                if(string.IsNullOrWhiteSpace(categoryFreq.CheckPoint1Configuration.Duration.ToString()))
                    catFreq.Add(SqlConstants.SpParmaCategorieDurOne, SqlConstants.DefaultFreq);
                else
                    catFreq.Add(SqlConstants.SpParmaCategorieDurOne, categoryFreq.CheckPoint1Configuration.Duration.ToString());
                catFreq.Add(SqlConstants.SpParmaCategorieFlashOne, categoryFreq.CheckPoint1Configuration.Flash.ToString());

                if (string.IsNullOrWhiteSpace(categoryFreq.CheckPoint2Configuration.Duration.ToString()))
                    catFreq.Add(SqlConstants.SpParmaCategorieDurTwo, SqlConstants.DefaultFreq);
                else
                    catFreq.Add(SqlConstants.SpParmaCategorieDurTwo, categoryFreq.CheckPoint2Configuration.Duration.ToString());
                catFreq.Add(SqlConstants.SpParmaCategorieFlashTwo, categoryFreq.CheckPoint2Configuration.Flash.ToString());

                if (string.IsNullOrWhiteSpace(categoryFreq.CheckPoint3Configuration.Duration.ToString()))
                    catFreq.Add(SqlConstants.SpParmaCategorieDurThree, SqlConstants.DefaultFreq);
                else
                    catFreq.Add(SqlConstants.SpParmaCategorieDurThree, categoryFreq.CheckPoint3Configuration.Duration.ToString());
                catFreq.Add(SqlConstants.SpParmaCategorieFlashThree, categoryFreq.CheckPoint3Configuration.Flash.ToString());

                if (string.IsNullOrWhiteSpace(categoryFreq.CheckPoint4Configuration.Duration.ToString()))
                    catFreq.Add(SqlConstants.SpParmaCategorieDurFour, SqlConstants.DefaultFreq);
                else
                    catFreq.Add(SqlConstants.SpParmaCategorieDurFour, categoryFreq.CheckPoint4Configuration.Duration.ToString());
                catFreq.Add(SqlConstants.SpParmaCategorieFlashFour, categoryFreq.CheckPoint4Configuration.Flash.ToString());

                SqlCmd CmdSql = new SqlCmd();
                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {
                     using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpCategoryDurations, catFreq, sqlConnection, Guid.Empty))
                    { }
                }
            }

        }
        public void CategoryColors(Category category)
        {
            Dictionary<string, string> catColor = new Dictionary<string, string>();
            catColor.Add(SqlConstants.SpParmaCategorieId, category.Title);

            catColor.Add(SqlConstants.SpParmaCategorieColor, category.Color.Value.Name);
            catColor.Add(SqlConstants.SpParmaCategorieColorHex, $"{category.Color.Value.A},{category.Color.Value.B},{category.Color.Value.G},{category.Color.Value.R}");
            SqlCmd CmdSql = new SqlCmd();
            using (SqlConnection sqlConnection = CmdSql.SqlConnection())
            {
                using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpCategoryColors, catColor, sqlConnection, Guid.Empty))
                { }
            }
            CategoryFreq(category);

        }
        public void RegisterBinSqlServer(SpecimenBatch regBatch)
        {
            SqlCmd CmdSql = new SqlCmd();
            Trace.TraceInformation(string.Format("Method public void RegisterBinSqlServer(SpecimenBatch regBatch) for batchid:{0} binid:{1}", regBatch.Id, regBatch.BinId));
            try
            {
                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {
                    string completedAt = BinUtilities.BinMointorUtilties.ReplaceString(regBatch.Registration.CompletedAt.ToString(), "/0001", "/1989");
                    string startedAt = BinUtilities.BinMointorUtilties.ReplaceString(regBatch.Registration.StartedAt.ToString(), "/0001", "/1989");
                    Dictionary<string, string> dicRegBin = new Dictionary<string, string>();
                    dicRegBin.Add(SqlCmd.SpParmaBatchId, regBatch.Id);
                    dicRegBin.Add(SqlCmd.SpParmaBinId, regBatch.BinId);
                    dicRegBin.Add(SqlCmd.SpParmaCategorieId, regBatch.CategoryId);
                    if (string.IsNullOrEmpty(regBatch.CreatedBy))
                        dicRegBin.Add(SqlCmd.SpParmaAssignedBy, SqlCmd.NoValue);
                    else
                        dicRegBin.Add(SqlCmd.SpParmaCreatedBy, regBatch.CreatedBy);
                    if (string.IsNullOrEmpty(regBatch.Registration.AssignedBy))
                        dicRegBin.Add(SqlCmd.SpParmaAssignedBy, SqlCmd.NoValue);
                    else
                        dicRegBin.Add(SqlCmd.SpParmaAssignedBy, regBatch.Registration.AssignedBy);
                    if (string.IsNullOrEmpty(regBatch.Registration.AssignedTo))
                        dicRegBin.Add(SqlCmd.SpParmaAssignedTo, SqlCmd.NoValue);
                    else
                        dicRegBin.Add(SqlCmd.SpParmaAssignedTo, regBatch.Registration.AssignedTo);

                    dicRegBin.Add(SqlCmd.SpParmaStartedAt, startedAt);
                    if (string.IsNullOrEmpty(regBatch.Registration.CompletedBy))
                        dicRegBin.Add(SqlCmd.SpParmaCompletedBy, SqlCmd.NoValue);
                    else
                        dicRegBin.Add(SqlCmd.SpParmaCompletedBy, regBatch.Registration.CompletedBy);
                    dicRegBin.Add(SqlCmd.SpParmaCreatedAt, completedAt);
                    using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpAddBinRegistration, dicRegBin, sqlConnection, Guid.Empty))
                    { }
                    ProcessingBinSqlServer(regBatch);
                    BinCommentsContentsSqlServer(regBatch);
                    BinColsedSqlServer(regBatch);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Updating xml file bin batches message:{0}", ex.Message));
                throw new Exception(string.Format("Updating xml file bin batches message:{0}", ex.Message));
            }

        }

        public void BinColsedSqlServer(SpecimenBatch closeBatch)
        {
            Trace.TraceInformation(string.Format("in method  public void BinColsedSqlServer(SpecimenBatch closeBatch) for batchid:{0} binid:{1}", closeBatch.Id, closeBatch.BinId));
            string completedAt = BinUtilities.BinMointorUtilties.ReplaceString(closeBatch.ClosedAt.ToString(), "/0001", "/1989");
            string closedBy = string.Empty;
            if (!(string.IsNullOrWhiteSpace(closeBatch.ClosedBy)))
            {
                closedBy = closeBatch.ClosedBy;
            }
            SqlCmd CmdSql = new SqlCmd();
            try
            {
                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {

                    Dictionary<string, string> dicRegBin = new Dictionary<string, string>();
                    dicRegBin.Add(SqlCmd.SpParmaBatchId, closeBatch.Id);
                    dicRegBin.Add(SqlCmd.SpParmaAssignedTo, closedBy);
                    dicRegBin.Add(SqlCmd.SpParmaCompletedAt, completedAt);

                    using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpBinClosed, dicRegBin, sqlConnection, Guid.Empty))
                    { }


                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Closiong batch message:{0}", ex.Message));
                throw new Exception(string.Format("Closiong batch message:{0}", ex.Message));
            }


        }



        public void BinCommentsContentsSqlServer(SpecimenBatch contentComments)
        {
            SqlCmd CmdSql = new SqlCmd();
            Trace.TraceInformation("Method public void BinCommentsContentsSqlServer(SpecimenBatch contentComments)");
            try
            {
                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {

                    Dictionary<string, string> dicRegBin = new Dictionary<string, string>();
                    string strContents = string.Empty;
                    dicRegBin.Add(SqlCmd.SpParmaBatchId, contentComments.Id);
                    if (string.IsNullOrEmpty(contentComments.Comments))
                        dicRegBin.Add(SqlCmd.SpParmaComments, string.Empty);
                    else
                        dicRegBin.Add(SqlCmd.SpParmaComments, contentComments.Comments);
                    if (contentComments.Specimens.Count > 0)
                    {
                        foreach (string cont in contentComments.Specimens)
                        {
                            strContents += strContents + "\r\n";
                        }
                    }
                    dicRegBin.Add(SqlCmd.SpParmaContents, strContents);

                    using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpBinCommentsContents, dicRegBin, sqlConnection, Guid.Empty))
                    { }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Adding comments message:{0}", ex.Message));
                throw new Exception(string.Format("Adding comments message:{0}", ex.Message));
            }

        }

        public void AddUpdateUsers(User useInfo)
        {
            Trace.TraceInformation(string.Format("For user:{0}", useInfo.DisplayName));
            SqlCmd CmdSql = new SqlCmd();
            try
            {
                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {
                    Dictionary<string, string> dicUserInfo = new Dictionary<string, string>();
                    dicUserInfo.Add(SqlCmd.SpParmaCwid, useInfo.Id);
                    dicUserInfo.Add(SqlCmd.SpParmaUserFirstName, useInfo.FirstName);
                    dicUserInfo.Add(SqlCmd.SpParmaUserLastName, useInfo.LastName);
                    dicUserInfo.Add(SqlCmd.SpParmaUserPassword, useInfo.CardId);
                    dicUserInfo.Add(SqlCmd.SpParmaUserEmailAddress, useInfo.EmailAddress);
                    dicUserInfo.Add(SqlCmd.SpParmaUserProfile, useInfo.UserProfile.Id);

                    using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpAddUpDateBinUsers, dicUserInfo, sqlConnection, Guid.Empty))
                    { }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Updating user:{0} message:{1}", useInfo.DisplayName, ex.Message));
                throw new Exception(string.Format("Updating user:{0} message:{1}", useInfo.DisplayName, ex.Message));

            }
        }

        public void DeleteUser(string cwid, string displayName)
        {
            Trace.TraceInformation(string.Format("public void DeleteUser(string cwid, string displayName) cwid:{0} displacyName:{1}", cwid, displayName));
            SqlCmd CmdSql = new SqlCmd();
            try
            {
                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {
                    Dictionary<string, string> dicUserInfo = new Dictionary<string, string>();
                    dicUserInfo.Add(SqlCmd.SpParmaCwid, cwid);
                    dicUserInfo.Add(SqlCmd.SpParmaDisplayName, displayName);
                    using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpDeleteBinUsers, dicUserInfo, sqlConnection, Guid.Empty))
                    { }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Deleting user cwid:{0} displacyName:{1} message:{2}", cwid, displayName, ex.Message));
                throw new Exception(string.Format("Deleting user cwid:{0} displacyName:{1} message:{2}", cwid, displayName, ex.Message));
            }
        }
        public void ProcessingBinSqlServer(SpecimenBatch regBatch)
        {
            Trace.TraceInformation(string.Format("Method public void ProcessingBinSqlServer(SpecimenBatch regBatch) for batch:{0} binid:{1}", regBatch.Id, regBatch.BinId));
            SqlCmd CmdSql = new SqlCmd();
            try
            {
                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {
                    string completedAt = BinUtilities.BinMointorUtilties.ReplaceString(regBatch.Processing.CompletedAt.ToString(), "/0001", "/1989");
                    string startedAt = BinUtilities.BinMointorUtilties.ReplaceString(regBatch.Processing.StartedAt.ToString(), "/0001", "/1989");
                    Dictionary<string, string> dicRegBin = new Dictionary<string, string>();
                    dicRegBin.Add(SqlCmd.SpParmaBatchId, regBatch.Id);
                    dicRegBin.Add(SqlCmd.SpParmaStartedAt, startedAt);
                    if (string.IsNullOrEmpty(regBatch.Processing.AssignedBy))
                        dicRegBin.Add(SqlCmd.SpParmaAssignedBy, SqlCmd.NoValue);
                    else
                        dicRegBin.Add(SqlCmd.SpParmaAssignedBy, regBatch.Processing.AssignedBy);
                    if (string.IsNullOrEmpty(regBatch.Processing.AssignedTo))
                        dicRegBin.Add(SqlCmd.SpParmaAssignedTo, SqlCmd.NoValue);
                    else
                        dicRegBin.Add(SqlCmd.SpParmaAssignedTo, regBatch.Processing.AssignedTo);
                    if (string.IsNullOrEmpty(regBatch.Processing.CompletedBy))
                        dicRegBin.Add(SqlCmd.SpParmaCompletedBy, SqlCmd.NoValue);
                    else
                        dicRegBin.Add(SqlCmd.SpParmaCompletedBy, regBatch.Processing.CompletedBy);
                    dicRegBin.Add(SqlCmd.SpParmaCompletedAt, completedAt);

                    using (SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpBinProcessing, dicRegBin, sqlConnection, Guid.Empty))
                    { }


                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Adding processing batch:{0} binid:{1} message:{2}", regBatch.Id, regBatch.BinId, ex.Message));
                throw new Exception(string.Format("Adding processing batch:{0} binid:{1} message:{2}", regBatch.Id, regBatch.BinId, ex.Message));
            }
        }


        //public void AddNewCategory(SpecimenBatch SpecBatch)
        //{
        //    SqlCmd CmdSql = new SqlCmd();
        //    SqlConnection sqlConnection = CmdSql.SqlConnection(CloudServerName, CloudDbName, CloudDbUserName, CloudUserPw);
        //    Dictionary<string, string> dicNewCat = new Dictionary<string, string>();
        //    Guid batchGuid = Guid.NewGuid();
        //    dicNewCat.Add(SqlCmd.SpParmaBatchId, batchGuid.ToString());
        //    dicNewCat.Add(SqlCmd.SpParmaBinId, SpecBatch.BinId);
        //    dicNewCat.Add(SqlCmd.SpParmaCategorieId, SpecBatch.CategoryId);
        //    dicNewCat.Add(SqlCmd.SpParmaStartedAt, SpecBatch.CreatedAt.ToString());
        //    dicNewCat.Add(SqlCmd.SpParmaAssignedBy, SpecBatch.CreatedBy);
        //    dicNewCat.Add(SqlCmd.SpParmaAssignedTo, SpecBatch.CreatedBy);
        //    dicNewCat.Add(SqlCmd.SpParmaCompletedBy, SpecBatch.CreatedBy);
        //    SqlDataReader dr = CmdSql.SqlDataReader(SqlCmd.SpAddBinRegistration, dicNewCat, sqlConnection, Guid.Empty);
        //    dr.Close();
        //    dicNewCat.Clear();
        //    dicNewCat.Add(SqlCmd.SpParmaBatchId, batchGuid.ToString());
        //    if (string.IsNullOrEmpty(SpecBatch.Comments))
        //        dicNewCat.Add(SqlCmd.SpParmaComments, string.Empty);
        //    else
        //        dicNewCat.Add(SqlCmd.SpParmaComments, SpecBatch.Comments);
        //    if (SpecBatch.Specimens.Count == 0)
        //        dicNewCat.Add(SqlCmd.SpParmaContents, string.Empty);
        //    else
        //    {
        //        dicNewCat.Add(SqlCmd.SpParmaContents, string.Join(Environment.NewLine, SpecBatch.Specimens.ToArray()));
        //    }
        //    CmdSql.SqlDataReader(SqlCmd.SpAddUpDateBinCommentsContents, dicNewCat, sqlConnection, Guid.Empty);

        //    //CmdSql.SqlDataReader(CmdSql.)
        //    if (sqlConnection.State == ConnectionState.Open)
        //        sqlConnection.Close();
        //}
        public void SpecEmails(string emailTable, string batchId, string emailRecp, string emailStart, string emailComplete, string emailContents)
        {
            Trace.TraceInformation(string.Format("Method public void SpecEmails(string emailTable, string batchId, string emailRecp, string emailStart, string emailComplete, string emailContents) emailtable:{0} batchid:{1} recp{2}", emailTable, batchId, emailRecp));
            try
            {
                Dictionary<string, string> dicEmailSpec = new Dictionary<string, string>();
                dicEmailSpec.Add(SqlConstants.SpParmaBatchId, batchId);
                dicEmailSpec.Add(SqlConstants.SpParmaSendEmailStart, emailStart);
                dicEmailSpec.Add(SqlConstants.SpParmaSendEmailComplete, emailComplete);
                dicEmailSpec.Add(SqlConstants.SpParmaSendEmailContents, emailContents);
                dicEmailSpec.Add(SqlConstants.SpParmaEmailTo, emailRecp);
                dicEmailSpec.Add(SqlConstants.SpParmaUpdateEmailTable, emailTable);
                SqlCmd CmdSql = new SqlCmd();
                using (SqlConnection sqlConnection = CmdSql.SqlConnection())
                {
                    using (SqlDataReader dr = CmdSql.SqlDataReader(SqlConstants.SpSendSpecBatchsEmails, dicEmailSpec, sqlConnection))
                    { }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation(string.Format("UpDating emails emailtable:{0} batchid:{1} recp{2} message:{3}", emailTable, batchId, emailRecp, ex.Message));
                throw new Exception(string.Format("UpDating emails emailtable:{0} batchid:{1} recp{2} message:{3}", emailTable, batchId, emailRecp, ex.Message));

            }


        }
        #endregion
    }
}
