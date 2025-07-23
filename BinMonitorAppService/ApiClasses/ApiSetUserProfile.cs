using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BinMonitorAppService.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Constants;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data;
using System.Data.SqlClient;
using BinMonitorAppService.ApiClasses;
using BinMonitorAppService.Logging;
using System.Text.RegularExpressions;
using BinMonitor.BinInterfaces;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.Schemas;
using ITfoxtec.Identity.Saml2.MvcCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using System.Security.Authentication;
using BinMonitorAppService.Pages.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens.Saml2;
using System.Xml;

namespace BinMonitorAppService.ApiClasses
{
    public class ApiSetUserProfile
    {

        private static ApiSetUserProfile instance = null;
        ApiSetUserProfile()
        {
        }


        public static ApiSetUserProfile UserProfileInstance
        {
            get
            {
                if (instance == null)
                    instance = new ApiSetUserProfile();
                return instance;
            }
        }

        public async Task<SpectrumMonitorMenuRightsModel> GetBMMenuRights(string cwid,ISession session, ILog auditLogs,string weburl)
        {
            SpectrumMonitorMenuRightsModel menuRightsModel = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<SpectrumMonitorMenuRightsModel>(session, SqlConstants.CachMenuRights).ConfigureAwait(false).GetAwaiter().GetResult();
            if (menuRightsModel == null)
            {
                ApiSetUserProfile.UserProfileInstance.GetUserProfile(cwid, weburl, auditLogs,session).ConfigureAwait(false).GetAwaiter().GetResult();
                menuRightsModel = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<SpectrumMonitorMenuRightsModel>(session, SqlConstants.CachMenuRights).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            return menuRightsModel;

        }
        public async Task<int> GetSessionTime(ISession session,int minTime)
        {
            minTime = minTime * 60;
            
            DateTimeOffset dtValidTo = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<DateTimeOffset>(session, SqlConstants.CacheSecurityTokenValidTo).ConfigureAwait(false).GetAwaiter().GetResult();
            //DateTimeOffset dtValidFRom9 = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<DateTimeOffset>(session, SqlConstants.CacheSecurityTokenValidFrom).ConfigureAwait(false).GetAwaiter().GetResult();
            DateTimeOffset dtValidFRom = DateTime.Now.ToUniversalTime();
            if (dtValidFRom > dtValidTo)
                return -1;
            TimeSpan ts = dtValidTo - dtValidFRom;
            int totSeconds = (Math.Abs((int)ts.TotalSeconds - minTime));
            if ((int)totSeconds <= minTime)
                return -1;
            totSeconds = totSeconds * 1000;
           // totSeconds = Math.Abs(ts.TotalSeconds) * 1000;
            return (int)totSeconds;
        }
        public async Task GetUserProfile(string cwid, string webApiUrl, ILog auditLogs, ISession session)
        {
            //HttpContext.Session.SetString(GetSessionVariables.SessionKeyCwid, cwid);
            //  HttpContext.Session.SetString(GetSessionVariables.SessionKeyUserProfile, "1");

            SpecMonitorFormUserPre MonitorFormUserPre = new SpecMonitorFormUserPre();
            UsersModel UsersModelEdit = null;
            IDictionary<string, SpecMonitorUserProfileRights> SpecMonUserRigths = null;
            SpectrumMonitorMenuRightsModel spectrumMonitorMenuRights = new SpectrumMonitorMenuRightsModel();
            MonitorFormUserPre.Cwid = cwid;
            string spinfo = string.Format(SqlConstants.ApiUserInformation, cwid, "NA");
            UsersModelEdit = await GetApis.GetApisInctance.ApiUserProfileInfo(spinfo, $"{webApiUrl}{SqlConstants.ApiSpecMonitorUserController}", auditLogs);
            SpecMonUserRigths = await BinsInformation.BinsApisInctance.ApiGetUserRightsModel($"{webApiUrl}{SqlConstants.WebApiBinMonitor}", SqlConstants.SpGetSpUserRightsModel, auditLogs);
            foreach (KeyValuePair<string, SpecMonitorUserProfileRights> keyValuePair in SpecMonUserRigths)
            {
                if (string.Compare(keyValuePair.Key, UsersModelEdit.UserProfileName, true) == 0)
                {
                    MonitorFormUserPre.ProfileName = UsersModelEdit.UserProfileName;
                    if (string.Compare(MonitorFormUserPre.ProfileName, "ADMIN", true) == 0)
                    {
                        MonitorFormUserPre.Admin = true;
                    }
                    if (keyValuePair.Value.CreateNewProfiles.ToLower().Trim().StartsWith("true"))
                    {
                        MonitorFormUserPre.CreateNewProfiles = true;
                    }
                    if (keyValuePair.Value.ChangeUsersPasswords.ToLower().Trim().StartsWith("true"))
                    {
                        MonitorFormUserPre.ChangeUsersPasswords = true;
                    }
                    if (keyValuePair.Value.DeleteUsers.ToLower().Trim().StartsWith("true"))
                    {
                        MonitorFormUserPre.DeleteUsers = true;
                    }

                    if (keyValuePair.Value.EditUsers.ToLower().Trim().StartsWith("true"))
                    {
                        MonitorFormUserPre.EditUsers = true;
                    }

                    if (keyValuePair.Value.RunReports.ToLower().Trim().StartsWith("true"))
                    {
                        MonitorFormUserPre.RunReports = true;
                        spectrumMonitorMenuRights.RunReports = true;
                    }
                    if (keyValuePair.Value.TransFerBins.ToLower().Trim().StartsWith("true"))
                    {
                        MonitorFormUserPre.TransFerBins = true;
                        spectrumMonitorMenuRights.TransFerBins = true;
                    }
                    if (keyValuePair.Value.EmailReports.ToLower().Trim().StartsWith("true"))
                    {
                        MonitorFormUserPre.EmailReports = true;
                        spectrumMonitorMenuRights.EmailReports = true;
                    }
                    if (keyValuePair.Value.Categories.ToLower().Trim().StartsWith("true"))
                    {
                        MonitorFormUserPre.Categories = true;
                        spectrumMonitorMenuRights.Categories = true;
                    }
                    if (keyValuePair.Value.TransFerCategories.ToLower().Trim().StartsWith("true"))
                    {
                        MonitorFormUserPre.TransFerCategories = true;
                        spectrumMonitorMenuRights.TransFerCategories = true;
                    }

                    if (keyValuePair.Value.ManageUserProfiles.ToLower().Trim().StartsWith("true"))
                    {
                        MonitorFormUserPre.ManageUserProfiles = true;
                    }
                    if (keyValuePair.Value.CreateUsers.ToLower().Trim().StartsWith("true"))
                    {
                        MonitorFormUserPre.CreateUsers = true;
                    }

                    
                   
                }

            }
            await GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(session, "ProfileUser", MonitorFormUserPre);
            await GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(session, SqlConstants.CachMenuRights, spectrumMonitorMenuRights);
        }
    }
}
