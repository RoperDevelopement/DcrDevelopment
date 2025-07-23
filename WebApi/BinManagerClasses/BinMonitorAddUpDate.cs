using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Models;
using BinMonitorAppService.Constants;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
using Edocs.WebApi.ApiClasses;
using System.Net.NetworkInformation;

namespace Edocs.WebApi.BinManagerClasses
{
    public class BinMonitorAddUpDate
    {

        public async Task<JsonResult> GetSpecMonitorUserInfo(string sqlConnectionString, string cwid)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpGetSpecModelUserInfo, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCwid, cwid));
                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"For sql connection string {sqlConnectionString}  {ex.Message}");
            }
        }
        public async Task<JsonResult> LoginSpecMonitorUser(string sqlConnectionString, string cwid, string passWord)
        {
            if (string.Compare(passWord, "na", true) == 0)
            {
                return await GetSpecMonitorUserInfo(sqlConnectionString, cwid);
            }
            else
            {
                try
                {

                    using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                    {
                        using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpSpectrumMonitorUserLogIn, sqlConnection))
                        {
                            sqlCmd.CommandTimeout = 180;
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCwid, cwid));
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserPassword, passWord));
                            return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"connection string {sqlConnectionString} {ex.Message}");
                }
            }
        }
        public async Task<JsonResult> GetSpecMonitorReprts(string sqlConnectionString, string binID, string labReq, string regCreatedBY, string processCreatedBY, string binCLosedBY, string categoryName, DateTime labReqRegStDate, DateTime labReqRegEndDate)

        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpGetBinsReports, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        if (!(string.IsNullOrWhiteSpace(binID)))
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinId, binID));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinId, DBNull.Value));
                        if (!(string.IsNullOrWhiteSpace(labReq)))
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaLabRecNumber, labReq));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaLabRecNumber, DBNull.Value));
                        if (!(string.IsNullOrWhiteSpace(regCreatedBY)))

                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCreatedBy, regCreatedBY));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCreatedBy, DBNull.Value));
                        if (!(string.IsNullOrWhiteSpace(processCreatedBY)))
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaAssignedTo, processCreatedBY));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaAssignedTo, DBNull.Value));
                        if (!(string.IsNullOrWhiteSpace(binCLosedBY)))
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCompletedBy, binCLosedBY));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCompletedBy, DBNull.Value));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusStartDate, labReqRegStDate));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusEndDate, labReqRegEndDate));
                        if (!(string.IsNullOrWhiteSpace(categoryName)))
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategoryName, categoryName));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategoryName, DBNull.Value));
                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);

                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<JsonResult> GetSpecMonitorReprtsByCwid(string sqlConnectionString, string cwid, string binStatus, string spName, DateTime sDate, DateTime eDate, string categoryName)

        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        //if ((string.Compare(cwid.Trim(),"All Cwid",true) == 0))
                        // sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCWID, DBNull.Value));
                        //else

                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCWID, cwid));

                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatus, binStatus));



                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusStartDate, sDate));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusEndDate, eDate));
                        //  if (string.Compare(categoryName.Trim(), "", true) ==0)
                        //     sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategoryName, DBNull.Value));
                        //   else
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategoryName, categoryName));

                        return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);

                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task TrasFerBin(TransFerModel transFer, string sqlConnectionStr)
        {
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
            {
                using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpTransFerByBinId, sqlConnection))
                {
                    sqlCmd.CommandTimeout = 180;
                              sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinId, transFer.BinID));
                    sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaOldBinId, transFer.OldBinId));
                    if(string.IsNullOrEmpty(transFer.Comments))
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaComments,$"Transfer bind id {transFer.OldBinId} to new bid id {transFer.BinID}"));
                    else
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaComments, transFer.Comments));
                    sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaTransFerBy, transFer.TransferBy));
                    sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaTransFerType, transFer.TransFerType));
                    await sqlConnection.OpenAsync();
                    await sqlCmd.ExecuteReaderAsync();
                }
            }
        }
        public async Task TransFer(TransFerModel transFer, string sqlConnectionStr)
        {
            try
            {
                switch (Enum.Parse(typeof(TransferType), transFer.TransFerType))
                {
                    case TransferType.TB:
                        TrasFerBin(transFer, sqlConnectionStr).ConfigureAwait(false).GetAwaiter().GetResult();
                        break;
                    case TransferType.TC:
                        TransFerCategories(transFer,sqlConnectionStr).GetAwaiter().GetResult(); 
                        break;
                    case TransferType.TL:
                        TransFerLabReqs(transFer, sqlConnectionStr).GetAwaiter().GetResult();
                        break;
                    case TransferType.TP:
                        TransFerProcess(transFer, sqlConnectionStr).GetAwaiter().GetResult();
                        break;
                    default: throw new Exception($"Transfer type {transFer.TransFerType} not found for bin id {transFer.OldBinId}");
                }
 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
    }
}

public async Task TransFerCategories(TransFerModel transFer, string sqlConnectionStr)
{
    try
    {
        using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
        {
            using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpSpectrumMonitorTransFerCategory, sqlConnection))
            {
                sqlCmd.CommandTimeout = 180;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinId, transFer.BinID));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategoryName, transFer.CategoryName));
                         
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.OldCategoryName, transFer.OldBinId));
                

                        if (string.IsNullOrEmpty(transFer.Comments))
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaComments, $"Transfer category id {transFer.OldBinId}  to new category {transFer.CategoryName} bid id {transFer.BinID}"));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaComments, transFer.Comments));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaTransFerBy, transFer.TransferBy));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaTransFerType, transFer.TransFerType));
                        await sqlConnection.OpenAsync();
                SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();
            }
        }
    }
    catch (Exception ex)
    {
        throw new Exception(ex.Message);
    }
}
        public async Task TransFerLabReqs(TransFerModel transFer, string sqlConnectionStr)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpTansFerByLabReqNum, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinId, transFer.BinID));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaOldBinId, transFer.OldBinId));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaLabRecNumber, transFer.LabReqNumber));

                       


                        if (string.IsNullOrEmpty(transFer.Comments))
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaComments, $"Transfer Labreq id {transFer.LabReqNumber} for bid id {transFer.BinID}"));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaComments, transFer.Comments));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaTransFerBy, transFer.TransferBy));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaTransFerType, transFer.TransFerType));
                        await sqlConnection.OpenAsync();
                        SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task TransFerProcess(TransFerModel transFer, string sqlConnectionStr)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpTransFerProces, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinId, transFer.BinID));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaAssignedTo, transFer.Processing));
                         




                        if (string.IsNullOrEmpty(transFer.Comments))
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaComments, $"Transfer Process bin id {transFer.BinID} to {transFer.Processing}"));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaComments, transFer.Comments));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaTransFerBy, transFer.TransferBy));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaTransFerType, transFer.TransFerType));
                        await sqlConnection.OpenAsync();
                        SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task TransFerByBinId(TransFerModel transFer, string sqlConnectionStr)
{
    try
    {
        using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
        {
            using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpTransFerByBinId, sqlConnection))
            {
                sqlCmd.CommandTimeout = 180;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinId, transFer.BinID));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaOldBinId, transFer.OldBinId));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaComments, transFer.Comments));
                await sqlConnection.OpenAsync();
                SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();
            }
        }
    }
    catch (Exception ex)
    {
        throw new Exception(ex.Message);
    }
}


public async Task AddUpdateUserPermissions(UpdateProfilesModel profilesModel, string sqlConnectionStr)
{
    try
    {
        using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
        {
            using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpAddUpdateUserProfiles, sqlConnection))
            {
                sqlCmd.CommandTimeout = 180;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaProfileName, profilesModel.ProfileName));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaRunReports, profilesModel.RunReports));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaTransFerBins, profilesModel.TransFerBins));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaTransFerCategories, profilesModel.TransFerCategories));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCreateUsers, profilesModel.CreateUsers));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaEditUsers, profilesModel.EditUsers));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaChangeUsersPasswords, profilesModel.ChangeUsersPasswords));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaDeleteUsers, profilesModel.DeleteUsers));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaManageUserProfiles, profilesModel.ManageUserProfiles));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCreateNewProfiles, profilesModel.CreateNewProfiles));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaChangeEmail, profilesModel.EmailReports));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaChangeCategores, profilesModel.Categories));
                await sqlConnection.OpenAsync();
                SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();
            }
        }
    }
    catch (Exception ex)
    {
        throw new Exception(ex.Message);
    }
}
public async Task UpdateCategoryCheckPoints(CategoryCheckPointModel checkPointModel, string sqlConnectionStr)
{
    try
    {
        using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
        {
            using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpUpDateCategoryCheckPointModel, sqlConnection))
            {
                sqlCmd.CommandTimeout = 180;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategoryName, checkPointModel.CategoryName));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategorieDurOne, checkPointModel.CategoryCheckPointOneDuration));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategorieColor1, checkPointModel.CategoryColorCheckPointOne));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategorieDurTwo, checkPointModel.CategoryCheckPointTwoDuration));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategorieColor2, checkPointModel.CategoryColorCheckPointTwo));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategorieDurThree, checkPointModel.CategoryCheckPointThreeDuration));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategorieColor3, checkPointModel.CategoryColorCheckPointThree));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategorieDurFour, checkPointModel.CategoryCheckPointFourDuration));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCategorieColor4, checkPointModel.CategoryColorCheckPointFour));
                await sqlConnection.OpenAsync();
                SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();

            }
        }
    }
    catch (Exception ex)
    {
        throw new Exception(ex.Message);
    }
}

public async Task UpdateSpecMonSettings(SpectrumMonitorSettings jsonFile, string sqlConnectionStr, string spName)
{
    try
    {
        var js = JsonConvert.SerializeObject(jsonFile);



        using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
        {
            using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
            {
                sqlCmd.CommandTimeout = 180;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaJasonFile, js));
                await sqlConnection.OpenAsync();
                SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();
                if (dr.HasRows)
                {
                    dr.Read();
                    string err = dr[0].ToString();
                    err = $"Error Updating SpectrumMonitorSettings for sqlconnection {sqlConnection} for store procedure {spName} error message {err}";
                    throw new Exception(err);
                }
            }
        }
    }
    catch (SqlException sqlEx)
    {
        throw new Exception(sqlEx.Message);
    }
    catch (Exception ex)
    {
        throw new Exception(ex.Message);
    }
}

public async Task AddSpectrumUsersLogin(UsersModel usersModel, string sqlConnectionStr, string storedPD)
{
    try
    {
        using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
        {
            using (SqlCommand sqlCmd = new SqlCommand(storedPD, sqlConnection))
            {
                sqlCmd.CommandTimeout = 180;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCwid, usersModel.Cwid));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserPassword, usersModel.UserPW));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserFirstName, usersModel.FirstName));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserLastName, usersModel.LastName));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserEmailAddress, usersModel.EmailAddress));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserProfile, usersModel.UserProfile));
                await sqlConnection.OpenAsync();
                SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();

            }
        }
    }
    catch (Exception ex)
    {
        throw new Exception(ex.Message);
    }
}
public async Task<JsonResult> AddSpectrumUsersLogin(UsersModel usersModel, string sqlConnectionStr, string storedPD, string oldPW, string modifyPW)
{
    JsonResult jsonResult = null;
    try
    {
        using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
        {
            using (SqlCommand sqlCmd = new SqlCommand(storedPD, sqlConnection))
            {
                sqlCmd.CommandTimeout = 180;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCwid, usersModel.Cwid));
                if (!(string.IsNullOrWhiteSpace(usersModel.UserPW)))
                    sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserPassword, usersModel.UserPW));
                else
                    sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserPassword, DBNull.Value));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserFirstName, usersModel.FirstName));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserLastName, usersModel.LastName));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserEmailAddress, usersModel.EmailAddress));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserProfile, usersModel.UserProfile));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaPwModify, modifyPW));
                if (string.Compare(oldPW, "na", true) == 0)
                    sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserOldPw, DBNull.Value));
                else
                    sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaUserOldPw, oldPW));


                await sqlConnection.OpenAsync();
                SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();

                if (dr.HasRows)
                {
                    dr.Read();
                    jsonResult = new JsonResult(dr[0].ToString());
                }
                else
                    throw new Exception("No results");



            }
        }
    }
    catch (Exception ex)
    {
        throw new Exception(ex.Message);

    }
    return jsonResult;
}

public async Task DelSpecMonitorUser(string sqlConnectionString, string cwid)
{

    try
    {

        using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
        {
            using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpDeleteSpUsers, sqlConnection))
            {
                sqlCmd.CommandTimeout = 180;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCwid, cwid));

                await sqlConnection.OpenAsync();
                SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();
            }
        }
    }

    catch (Exception ex)
    {
        throw new Exception(ex.Message);
    }
}

public async Task<JsonResult> GetUsageReport(string sqlConnectionString, string cwid, string binStatus, string repStDate, string repEDate)
{
    try
    {
        using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
        {
            using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpUsageReportByCWID, sqlConnection))
            {
                sqlCmd.CommandTimeout = 180;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                if ((string.Compare(cwid, "na", true) == 0) || (string.IsNullOrWhiteSpace(cwid)))
                    sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCwid, DBNull.Value));
                else
                    sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCwid, cwid));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatus, binStatus));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusStartDate, repStDate));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusEndDate, repEDate));
                return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);

            }
        }
    }
    catch (Exception ex)
    {
        throw new Exception($"For sql connection string {sqlConnectionString}  {ex.Message}");
    }
}

public async Task<JsonResult> GetDelLabReqs(string sqlConnectionString, string repStDate, string repEDate)
{
    try
    {
        using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
        {
            using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpGetDeletedLabReqs, sqlConnection))
            {
                sqlCmd.CommandTimeout = 180;
                sqlCmd.CommandType = CommandType.StoredProcedure;


                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusStartDate, repStDate));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusEndDate, repEDate));
                return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);

            }
        }
    }
    catch (Exception ex)
    {
        throw new Exception($"For sql connection string {sqlConnectionString}  {ex.Message}");
    }
}
public async Task<JsonResult> GetChartReport(string sqlConnectionString, string repStDate, string repEDate, string binStatus)
{
    try
    {
        using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
        {
            using (SqlCommand sqlCmd = new SqlCommand(SqlConstants.SpUsageChartReportByCWID, sqlConnection))
            {
                sqlCmd.CommandTimeout = 180;
                sqlCmd.CommandType = CommandType.StoredProcedure;


                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatus, binStatus));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusStartDate, repStDate));
                sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusEndDate, repEDate));
                return await JsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);

            }
        }
    }
    catch (Exception ex)
    {
        throw new Exception($"For sql connection string {sqlConnectionString}  {ex.Message}");
    }
}
    }

}

