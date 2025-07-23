
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



namespace BinMonitorAppService.Pages.BinUsers
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    [AllowAnonymous]

    public class LoginViewModel : PageModel
    {
        const string relayStateReturnUrl = "ReturnUrl";
        //const string relayStateReturnUrl = "TargetResource"
        private readonly Saml2Configuration config;
        private readonly IConfiguration configuration;
        private readonly IEmailSettings emailSettings;
        private readonly IWebHostEnvironment env;
        private ILog auditLogs;
        private string WebApiUrl
        { get { return configuration.GetValue<string>("BinMonitorApi").ToString(); } }

        private string NypEmailTo
        { get { return configuration.GetSection("NypContacts").GetValue<string>("EmailTo").ToString(); } }
        private string NypEmailCC
        { get { return configuration.GetSection("NypContacts").GetValue<string>("EmailCC").ToString(); } }
        private string EmailHtmlFile
        { get { return configuration.GetSection("NypContacts").GetValue<string>("EmailHtmlFile").ToString(); } }

        private string AllowedAudienceUrisAppliesTo
        { get { return configuration.GetSection("Saml2").GetValue<string>("AllowedAudienceUrisAppliesTo").ToString(); } }

        private Uri SamlUrlDestinationSSO
        { get { return new Uri(configuration.GetSection("Saml2").GetValue<string>("SamlUrlDestinationSSO").ToString()); } }

        private Uri SamlUrlDestinationSLO
        { get { return new Uri(configuration.GetSection("Saml2").GetValue<string>("SamlUrlDestinationSLO").ToString()); } }
        public LoginViewModel(IOptions<Saml2Configuration> configAccessor, ILog logsAudit, IConfiguration configBin, IEmailSettings email, IWebHostEnvironment webHostEnvironment)
        {
            config = configAccessor.Value;
            configuration = configBin;
            auditLogs = logsAudit;
            env = webHostEnvironment;
            emailSettings = email;
        }
        private async Task GetLogInformaiton()
        {

            auditLogs = InitAuditLogs.LogAsync(auditLogs, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private async Task SetCookie(string cookieKey, string cookieValue)
        {
            DelCookie(cookieKey).ConfigureAwait(false).GetAwaiter().GetResult();
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(5);
            Response.Cookies.Append(cookieKey, cookieValue, options);
        }
        private async Task SetSessionReturnUrl(string cookieValue, ISession session)
        {
            GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(session, GetSessionVariables.SessionKeyReturnUrl, cookieValue).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        private async Task<string> GetSessionReturnUrl(ISession session)
        {
            return (GetSessionVariables.SessionVarInstance.GetSessionVariable(session, GetSessionVariables.SessionKeyReturnUrl).ConfigureAwait(false).GetAwaiter().GetResult());
        }
        private async Task DelCookie(string cookieKey)
        {
            if (!(string.IsNullOrWhiteSpace(GetCookie(cookieKey).ConfigureAwait(false).GetAwaiter().GetResult())))
                Response.Cookies.Delete(cookieKey);
        }
        private async Task<string> GetCookie(string cookieKey)
        {
            return Request.Cookies[cookieKey];
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {


                InitAuditLogs.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start LoginViewModel OnPostAsync");
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
                var binding = new Saml2PostBinding();
                var saml2AuthnResponse = new Saml2AuthnResponse(config);

                Saml2Request saml2Request = binding.ReadSamlResponse(Request.ToGenericHttpRequest(), saml2AuthnResponse);

                if (saml2AuthnResponse.Status != Saml2StatusCodes.Success)
                {
                    auditLogs.LogError($"Colud not validate user SAML Response status: {saml2AuthnResponse.Status}");
                    throw new AuthenticationException($"SAML Response status: {saml2AuthnResponse.Status}");
                }



                auditLogs.LogInformation($"Checking cwid: {saml2Request.NameId.Value} has access to binmoniotr");
                if (!(GetUserInformation(saml2Request.NameId.Value, saml2Request.XmlDocument).ConfigureAwait(false).GetAwaiter().GetResult()))
                    return Redirect(Url.Content("/BinUsers/ErrorLoggingInView"));
                binding.Unbind(Request.ToGenericHttpRequest(), saml2AuthnResponse);
                saml2AuthnResponse.Saml2SecurityTokenHandler.TokenLifetimeInMinutes = 60;
                await saml2AuthnResponse.CreateSession(HttpContext, claimsTransform: (claimsPrincipal) => ClaimsTransform.Transform(claimsPrincipal));
                GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "CacheSecurityTokenValidTo", saml2AuthnResponse.SecurityTokenValidTo).ConfigureAwait(false).GetAwaiter().GetResult();
                GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "CacheSecurityTokenValidFrom", saml2AuthnResponse.SecurityTokenValidFrom).ConfigureAwait(false).GetAwaiter().GetResult();
                var relayStateQuery = binding.GetRelayStateQuery();

                var returnUrl = relayStateQuery.ContainsKey(relayStateReturnUrl) ? relayStateQuery[relayStateReturnUrl] : Url.Content("~/");
                if (returnUrl.Length < 2)
                {
                    returnUrl = GetReturnUrl(saml2Request.NameId.Value, returnUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                }

                HttpContext.Session.SetString(GetSessionVariables.SessionKeyCwid, saml2Request.NameId.Value);
                auditLogs.LogInformation($"User {User.Identity.Name} logged in");
                auditLogs.LogInformation($"End LoginView OnPostAsync total time: {InitAuditLogs.StopStopWatch()} ms return url: {returnUrl}");
                return Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"OnPostAsync Login total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model OnPostAsync Login {ex.Message}");
            }

        }
        public async Task<string> GetReturnUrl(string cwid, string urlRet)
        {

            auditLogs.LogInformation($"GetReturnUrl for cwid {cwid}");
            try
            {


                if (System.IO.File.Exists($"{env.WebRootPath}//returnurl//{cwid}.txt"))
                {
                    auditLogs.LogInformation($"Writing return url to file {env.WebRootPath}//returnurl//{cwid}.txt");
                    urlRet = System.IO.File.ReadAllText($"{env.WebRootPath}//returnurl//{cwid}.txt");
                    DelReturnUrlFile(cwid).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                auditLogs.LogInformation($"Found ReturnUrl {urlRet} for cwid {cwid}");
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"For cwid {cwid} {ex.Message}");
            }
            return urlRet;
        }
        public async Task SetReturnUrl(string cwid, string url)
        {
            try
            {
                auditLogs.LogInformation($"Method SetReturnUrl for cwid {cwid} return url {url}");
                DelReturnUrlFile(cwid).ConfigureAwait(false).GetAwaiter().GetResult();
                if (!(System.IO.Directory.Exists($"{env.WebRootPath}//returnurl")))
                    System.IO.Directory.CreateDirectory($"{env.WebRootPath}//returnurl");
                System.IO.File.WriteAllText($"{env.WebRootPath}//returnurl//{cwid}.txt", url);
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"Method SetReturnUrl for cwid {cwid} return url {url} {ex.Message}");
            }
        }
        public async Task DelReturnUrlFile(string cwid)
        {
            try
            {
                auditLogs.LogInformation($"Method DelReturnUrlFile for cwid {cwid}");
                if (System.IO.File.Exists($"{env.WebRootPath}//returnurl//{cwid}.txt"))
                {
                    System.IO.File.Delete($"{env.WebRootPath}//returnurl//{cwid}.txt");
                    auditLogs.LogInformation($"Del file {env.WebRootPath}//returnurl//{cwid}.txt  for cwid {cwid}");
                }
                else
                {
                    auditLogs.LogWarning($"File not found {env.WebRootPath}//returnurl//{cwid}.txt  for cwid {cwid}");
                }
            }
            catch (Exception ex)
            {
                auditLogs.LogWarning($"File {env.WebRootPath}//returnurl//{cwid}.txt  for cwid {cwid} {ex.Message}");
            }
        }
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            try
            {


                InitAuditLogs.StartStopWatch();
                if (!(System.IO.Directory.Exists($"{env.WebRootPath}//returnurl")))
                    System.IO.Directory.CreateDirectory($"{env.WebRootPath}//returnurl");
                var userID = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid);
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                auditLogs.LogInformation("Start LoginViewModel OngetAsync");
                //   DelCookie(SqlConstants.CookieReturnUrl).ConfigureAwait(true).GetAwaiter().GetResult();
                if (!(string.IsNullOrWhiteSpace(returnUrl)))
                {
                    var qStr = Request.QueryString;
                    if (qStr.HasValue)
                    {
                        if (qStr.Value.Contains("?"))
                        {
                            int indexEq = qStr.Value.IndexOf("=");
                            returnUrl = qStr.Value.Substring(++indexEq);
                        }
                        //        SetSessionReturnUrl(returnUrl, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
                        // SetCookie(SqlConstants.CookieReturnUrl, returnUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                        auditLogs.LogInformation($"LoginViewModel OngetAsync return url: {returnUrl}");
                        if (!(string.IsNullOrWhiteSpace(userID)))
                            SetReturnUrl(userID, returnUrl).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

                var serviceProviderRealm = config.SingleSignOnDestination;
                var binding = new Saml2PostBinding();
                binding.RelayState = $"RPID={Uri.EscapeDataString(serviceProviderRealm.ToString())}";

                var appliesToAddress = AllowedAudienceUrisAppliesTo;

                var response = new Saml2AuthnResponse(config);
                response.Config.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.Online;
                response.Config.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.PeerOrChainTrust;
                response.Status = Saml2StatusCodes.Success;

                var claimsIdentity = new ClaimsIdentity(CreateClaims());
                response.Destination = SamlUrlDestinationSSO;
                response.NameId = new Saml2NameIdentifier(claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).Single(), NameIdentifierFormats.Persistent);
                response.ClaimsIdentity = claimsIdentity;
                var token = response.CreateSecurityToken(appliesToAddress);
                auditLogs.LogInformation($"End LoginView OnGetAsync total time: {InitAuditLogs.StopStopWatch()} ms");
                return binding.Bind(response).ToActionResult();
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"OnPostAsync Login total time: {InitAuditLogs.StopStopWatch()} ms {ex.Message}");
                return Redirect($"/BinUsers/DisplayErrorMessagesView?ErrMEss=Model OnGet Login {ex.Message}");
            }
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            InitAuditLogs.StartStopWatch();
            GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
            auditLogs.LogInformation($"Method OnPostLogoutAsync logging out user");

            if (!User.Identity.IsAuthenticated)
            {
                auditLogs.LogError($"User :{User.Identity.Name} was not authenticated");
                auditLogs.LogInformation($"End LoginView OnPostLogoutAsync total time: {InitAuditLogs.StopStopWatch()} ms");
                return Redirect(Url.Content("~/"));
            }
            auditLogs.LogInformation($"Logging out User :{User.Identity.Name}");
            var binding = new Saml2PostBinding();

            config.SingleLogoutDestination = SamlUrlDestinationSLO;
            var saml2LogoutRequest = new Saml2LogoutRequest(config, User).DeleteSession(HttpContext).ConfigureAwait(true).GetAwaiter().GetResult();


            HttpContext.Session.Clear();
            auditLogs.LogInformation($"End LoginView OnPostLogoutAsync total time: {InitAuditLogs.StopStopWatch()} ms");
            return binding.Bind(saml2LogoutRequest).ToActionResult();
        }
        private IEnumerable<Claim> CreateClaims()
        {
            yield return new Claim(ClaimTypes.NameIdentifier, "Cwid");
            yield return new Claim(ClaimTypes.Email, "some-user@nyp.org");
            yield return new Claim(ClaimTypes.UserData, "lastName");
            yield return new Claim(ClaimTypes.UserData, "firstName");
        }
        private async Task<bool> GetUserInformation(string cwid, XmlDocument samlXmlDoc)
        {
            auditLogs.LogInformation($"LoginView Getting user information for cwid: {cwid}");
            string email = GetSamlAtt(samlXmlDoc, "mail").ConfigureAwait(false).GetAwaiter().GetResult();
            auditLogs.LogInformation($"LoginView email address: {email} for  cwid: {cwid}");
            string lastName = GetSamlAtt(samlXmlDoc, "lastName").ConfigureAwait(false).GetAwaiter().GetResult();
            auditLogs.LogInformation($"LoginView last name: {lastName} for  cwid: {cwid}");
            string firstName = GetSamlAtt(samlXmlDoc, "firstName").ConfigureAwait(false).GetAwaiter().GetResult();
            auditLogs.LogInformation($"LoginView first name: {firstName} for  cwid: {cwid}");
            string controlParams = string.Format(SqlConstants.ApiSpecMonitorUserUpdateInfo, cwid, "LoginBMUser");
            UsersModel user = new UsersModel();
            user.Cwid = cwid;
            user.EmailAddress = email;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.UserPW = "NoPassword";
            user.UserProfile = "LogInBM";
            string retMessage = await BinsInformation.BinsApisInctance.UpDateUserInfo(user, WebApiUrl, controlParams, auditLogs);
            if (!string.IsNullOrWhiteSpace(retMessage))
            {
                SendEMail(cwid, email).ConfigureAwait(false).GetAwaiter().GetResult();
                return false;
            }
            GetUserProfile(cwid, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
            return true;
        }
        private async Task<string> GetSamlAtt(XmlDocument samlXmlDoc, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(samlXmlDoc.InnerXml);
                var nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
                XmlNode xmlNode = doc.SelectSingleNode($"//saml:AttributeStatement/saml:Attribute[@Name='{value}']/saml:AttributeValue", nsmgr);
                if (xmlNode != null)
                {
                    return xmlNode.InnerText;

                }
            }
            catch (Exception ex)
            {
                auditLogs.LogError($"Could not get saml attribute {value} {ex.Message}");
            }
            return "Not Supplied";
        }
        public async Task GetUserProfile(string cwid, ISession session)
        {
            ApiSetUserProfile.UserProfileInstance.GetUserProfile(cwid, WebApiUrl, auditLogs, session).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task GetUserProfile(string cwid)
        {
            HttpContext.Session.SetString(GetSessionVariables.SessionKeyCwid, cwid);
            //  HttpContext.Session.SetString(GetSessionVariables.SessionKeyUserProfile, "1");

            SpecMonitorFormUserPre MonitorFormUserPre = new SpecMonitorFormUserPre();
            UsersModel UsersModelEdit = null;
            IDictionary<string, SpecMonitorUserProfileRights> SpecMonUserRigths = null;

            MonitorFormUserPre.Cwid = cwid;
            string spinfo = string.Format(SqlConstants.ApiUserInformation, cwid, "NA");
            UsersModelEdit = await GetApis.GetApisInctance.ApiUserProfileInfo(spinfo, $"{WebApiUrl}{SqlConstants.ApiSpecMonitorUserController}", auditLogs);
            SpecMonUserRigths = await BinsInformation.BinsApisInctance.ApiGetUserRightsModel($"{WebApiUrl}{SqlConstants.WebApiBinMonitor}", SqlConstants.SpGetSpUserRightsModel, auditLogs);
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

                    if (keyValuePair.Value.TransFerBins.ToLower().Trim().StartsWith("true"))
                    {
                        MonitorFormUserPre.TransFerBins = true;
                    }
                    if (keyValuePair.Value.TransFerCategories.ToLower().Trim().StartsWith("true"))
                    {
                        MonitorFormUserPre.TransFerCategories = true;
                    }

                    if (keyValuePair.Value.ManageUserProfiles.ToLower().Trim().StartsWith("true"))
                    {
                        MonitorFormUserPre.ManageUserProfiles = true;
                    }
                    if (keyValuePair.Value.CreateUsers.ToLower().Trim().StartsWith("true"))
                    {
                        MonitorFormUserPre.CreateUsers = true;
                    }

                    await GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "ProfileUser", MonitorFormUserPre);
                    break;
                }

            }
        }
        private async Task SendEMail(string cwid, string emailAddress)
        {

            EmailService emailService = new EmailService();
            emailSettings.EmailTo = NypEmailTo;


            if (!(string.IsNullOrWhiteSpace(emailSettings.EmailCC)))
            {
                emailSettings.EmailCC = $"{emailSettings.EmailCC};{NypEmailCC};{emailAddress}";

            }
            else
            {
                if (!(string.IsNullOrWhiteSpace(NypEmailCC)))
                {
                    emailSettings.EmailCC = $"{NypEmailCC};{emailAddress}";
                }
                else
                    emailSettings.EmailCC = $"{emailAddress};";
            }
            emailService.SendHtmlEmail($"{env.WebRootPath}//{EmailHtmlFile}", $"New Request BinMonitor System CWID {cwid}", emailSettings, cwid, emailAddress);
        }
    }
}