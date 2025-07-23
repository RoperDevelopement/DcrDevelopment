using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Edocs.ITS.AppService.ApisConst;
using System.Net.Http;
using System.Text.RegularExpressions;
using Edocs.ITS.AppService.Models;
using System.ComponentModel.DataAnnotations;
using Edocs.ITS.AppService.Interfaces;

namespace Edocs.ITS.AppService.Pages.EdocsITSUsers
{
    public class EdocsITSUsersViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private readonly IEmailSettings emailSettings;
        public EdocsITSUsersViewModel(IConfiguration config, IHttpClientFactory factoryClient, IEmailSettings email)
        {
            try
            {
                configuration = config;
                clientFactory = factoryClient;
                emailSettings = email;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }

        public IDictionary<int, EdocsITSUsersNamesModel> DicUserNameModel
        { get; set; }
        [BindProperty]
        public string SelUser
        { get; set; }
        public string CurrSelUser
        { get; set; }
        public UserLoginModel UserLogin
        { get; set; }
        [BindProperty]
        public EdocsITSUsersModel CreateUserModel
        { get; set; }

        public IDictionary<int, EdocsITSUsersModel> UserModelEdit
        { get; set; }
        [BindProperty]
        public EdocsITSUsersModel EditUserModel
        { get; set; }


        //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Verify Password:")]
        [BindProperty]
        public string VerifyPW
        { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Verify New Password:")]
        [BindProperty]
        public string VerifyNewPW
        { get; set; }



        public string ShowActiveCr
        { get; set; }
        public string ShowActiveEditUser
        { get; set; }
        [BindProperty]
        [Display(Name = "Generate Password:")]
        public bool GeneratePW
        { get; set; }
        [BindProperty]
        [Display(Name = "Generate New Password:")]
        public bool GenerateNewPW
        { get; set; }
        [BindProperty]
        [Display(Name = "Change Password:")]
        public bool ChangePW
        { get; set; }
        public async Task<IActionResult> OnGetAsync(string SelUser, string userEmail)
        {
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");

            ShowActiveCr = "show active";
            ShowActiveEditUser = "fade";



            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            if ((!(UserLogin.IsEdocsAdmin)) || (!(UserLogin.IsCustomerAdmin)))
                SelUser = UserLogin.UserName;
            //  EdocsITSUsersModel t = CreateUserModel;
            if (!(string.IsNullOrWhiteSpace(SelUser)))
            {
                GetSelectedUser(SelUser).ConfigureAwait(true).GetAwaiter().GetResult();
                //ShowActiveEditUser = "show active";
                //ShowActiveCr = "fade";
                //UserModelEdit = EdocsITSUsersApi.GetAllUserNames(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSUsersController, EdocsITSConstants.SpEdocsITSGetUserByUserName, SelUser).ConfigureAwait(true).GetAwaiter().GetResult();
                //if((UserModelEdit != null) && (UserModelEdit.Count > 0))
                //    EditUserModel = UserModelEdit.Values.ElementAt(0);
            }
            else if (!(string.IsNullOrWhiteSpace(Request.Query["exportuser"].ToString())))
            {
                FileContentResult fileContentResult = ExportUsers().ConfigureAwait(false).GetAwaiter().GetResult();
                return fileContentResult;
            }


            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            GetEdocsITSUserNames().ConfigureAwait(false).GetAwaiter().GetResult();
            UserLogin.CellPhoneNumber = "xxx-xxx-xxxx";
            //if(!(string.IsNullOrWhiteSpace(SelUser)))
            // {
            // OnPostAsync(CreateUserModel, null).ConfigureAwait(false).GetAwaiter();
            //   ModelState.AddModelError("ErrorPW", $"Invalid Delivery method");

            // }
            //CurrSelUser = SelUser;
            return Page();
        }
        private async Task GetSelectedUser(string selUser)
        {
            ShowActiveEditUser = "show active";
            ShowActiveCr = "fade";
            UserModelEdit = EdocsITSUsersApi.GetAllUserNames(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSUsersController, EdocsITSConstants.SpEdocsITSGetUserByUserName, selUser).ConfigureAwait(true).GetAwaiter().GetResult();
            if ((UserModelEdit != null) && (UserModelEdit.Count > 0))
            {
                EditUserModel = UserModelEdit.Values.ElementAt(0);
                //  EditUserModel.CellPhoneNumber = "406-490-8956";
                CurrSelUser = selUser;
            }
        }
        private async Task<FileContentResult> ExportUsers()
        {

            IDictionary<int, EdocsITSUsersModel> ListUserModel = new Dictionary<int, EdocsITSUsersModel>();
            if (UserLogin.IsEdocsAdmin)
                ListUserModel = EdocsITSUsersApi.GetAllUserNames(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSUsersController, EdocsITSConstants.SpEdocsITSGetUsersInformation, EdocsITSConstants.Edocs.ToLower()).ConfigureAwait(true).GetAwaiter().GetResult();
            else
                ListUserModel = EdocsITSUsersApi.GetAllUserNames(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSUsersController, EdocsITSConstants.SpEdocsITSGetUsersInformation, UserLogin.EdocsCustomerName).ConfigureAwait(true).GetAwaiter().GetResult();
            if (ListUserModel.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"{EdocsITSConstants.ExportUsersHeader}");
                foreach (KeyValuePair<int, EdocsITSUsersModel> keyValuePair in ListUserModel)
                {
                    sb.AppendLine($@"{keyValuePair.Key},{keyValuePair.Value.EdocsCustomerName},{keyValuePair.Value.UserName},{keyValuePair.Value.EmailAddress},{keyValuePair.Value.FirstName},{keyValuePair.Value.LastName},{keyValuePair.Value.CellPhoneNumber},{keyValuePair.Value.DateLastLogin},{keyValuePair.Value.DatePasswordLastChanged},{keyValuePair.Value.IsEdocsAdmin},{keyValuePair.Value.IsCustomerAdmin},{keyValuePair.Value.IsUserActive}");


                }

                // Response.Headers.Add("Content-Disposition", "attachment;filename=LabReqUsers.csv");
                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", $"{UserLogin.EdocsCustomerName}_Users_List_{DateTime.Now.ToString("MM_dd_yyyy")}.csv");
            }
            return null;
        }
        public async Task<IActionResult> OnPostAsync(string VerifyNewPW, bool GenerateNewPW, bool ChangePW)
        {
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");

            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            string tempPW = string.Empty;
            if ((ChangePW) && (!(GenerateNewPW)))
            {
                tempPW = EditUserModel.NewPassword;
                EditUserModel.Password = VerifyNewPW;
            }

            if (VerifyInput(EditUserModel, "ErrorMessageUserName", "ErrorMessageChangePW", "ErrorMessageVerifyChangePW", "ErrorMessageEditUserEmail", "ErrorMessageCellPhoneEditUser", GenerateNewPW, ChangePW).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                if (!(string.IsNullOrWhiteSpace(tempPW)))
                    EditUserModel.NewPassword = tempPW;
                //if (ChangePW)
                //{
                //    if (GenerateNewPW)
                //    {
                //        EditUserModel.Password = GeneratePassWord.GeneratePassWordInstance.GeneratePw(8);
                //        EditUserModel.NewPassword = EditUserModel.Password;
                //    }
                //}
                if ((!(UserLogin.IsCustomerAdmin)) || (!(UserLogin.IsCustomerAdmin)))
                    EditUserModel.IsUserActive = true;


                string retStr = EdocsITSUsersApi.UsersInstance.UpdateUserProfile(EditUserModel, EdocsITSConstants.EdocsITSUsersController, WebApiUrl, string.Empty).ConfigureAwait(true).GetAwaiter().GetResult();
                ModelState.AddModelError("ErrorMessage", $"{retStr} ");
                if (!(retStr.ToLower().StartsWith(EdocsITSConstants.SqlErrorStartsWith)))
                {

                    EditUserModel = null;
                    ShowActiveCr = "show active";
                    ShowActiveEditUser = "fade";
                    SelUser = string.Empty;
                }
                else
                {
                    ShowActiveEditUser = "show active";
                    ShowActiveCr = "fade";
                    SelUser = EditUserModel.UserName;

                }

            }
            else
            {
                if (!(string.IsNullOrWhiteSpace(tempPW)))
                    EditUserModel.NewPassword = tempPW;
                SelUser = UserLogin.UserName;
                ShowActiveEditUser = "show active";
                ShowActiveCr = "fade";
            }

            if (!(UserLogin.IsCustomerAdmin) || (!(UserLogin.IsEdocsAdmin)))
            {
                ShowActiveEditUser = "show active";
                ShowActiveCr = "fade";
                SelUser = UserLogin.UserName;
                GetSelectedUser(SelUser).ConfigureAwait(true).GetAwaiter().GetResult();

            }
            CurrSelUser = SelUser;
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();

            GetEdocsITSUserNames().ConfigureAwait(false).GetAwaiter().GetResult();
            EditUserModel.CellPhoneNumber = "406-490-8965";
            return Page();
        }
        public async Task<IActionResult> OnPostCreateNewUser(bool GeneratePW, string VerifyPW)
        {
            if (!CheckAuth())
                return Redirect("/LogInOut/LoginView");
            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            //  EdocsITSUsersModel t = CreateUserModel;
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            CreateUserModel.NewPassword = VerifyPW;
            ShowActiveCr = "show active";
            ShowActiveEditUser = "fade";
            if (VerifyInput(CreateUserModel, "ErrorMessageUserName", "ErrorMessagePW", "ErrorVerifyPW", "ErrorCrUserEmail", "ErrorMessageCellPhone", GeneratePW, true).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                CreateUserModel.EdocsCustomerName = UserLogin.EdocsCustomerName;
                if (GeneratePW)
                {
                    EditUserModel.Password = GeneratePassWord.GeneratePassWordInstance.GeneratePw(8);
                    EditUserModel.NewPassword = EditUserModel.Password;
                }
                //  if(ModelState.IsValid)
                //{ 
                string errMess = EdocsITSUsersApi.UsersInstance.AddNewUser(CreateUserModel, EdocsITSConstants.EdocsITSUsersController, WebApiUrl, EdocsITSConstants.SpEdocsITSAddUserLogin).ConfigureAwait(true).GetAwaiter().GetResult();
                if (errMess.ToLower().StartsWith(EdocsITSConstants.SqlSuccess))
                {
                    GetEdocsITSUserNames().ConfigureAwait(false).GetAwaiter().GetResult();
                    DicUserNameModel.Clear();
                    GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, EdocsITSConstants.SessionKeyEdocsITSUserNames, DicUserNameModel).ConfigureAwait(true).GetAwaiter().GetResult();
                    return Redirect("/EdocsITSUsers/EdocsITSUsersView");
                }
            }
            // }
            // }
            //// else
            //{
            //  foreach (var e in ModelState)
            //{

            // }
            //   }
            GetEdocsITSUserNames().ConfigureAwait(false).GetAwaiter().GetResult();
            return Page();
        }
        //public async Task<IActionResult> OnPostAsync(EdocsITSUsersModel CreateUserModel, string SelUser)
        //{
        //    ModelState.AddModelError("ErrorPW", $"Invalid Delivery method");
        //    return Page();
        //}
        private async Task GetEdocsITSUserNames()
        {

            DicUserNameModel = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IDictionary<int, EdocsITSUsersNamesModel>>(HttpContext.Session, EdocsITSConstants.SessionKeyEdocsITSUserNames).ConfigureAwait(true).GetAwaiter().GetResult();
            if ((DicUserNameModel == null) || (DicUserNameModel.Count == 0))
            {


                if (UserLogin.IsEdocsAdmin)
                    DicUserNameModel = EdocsITSUsersApi.GetUserNames(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSUsersController, EdocsITSConstants.SpEdocsITSGetUsersIDs, "edocs").ConfigureAwait(true).GetAwaiter().GetResult();
                else
                    DicUserNameModel = EdocsITSUsersApi.GetUserNames(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSUsersController, EdocsITSConstants.SpEdocsITSGetUsersIDs, UserLogin.EdocsCustomerName).ConfigureAwait(true).GetAwaiter().GetResult();
                if ((!(UserLogin.IsCustomerAdmin)) || (!(UserLogin.IsEdocsAdmin)))
                {
                    IDictionary<int, EdocsITSUsersNamesModel> keyValuePairs = new Dictionary<int, EdocsITSUsersNamesModel>();
                    foreach (var un in DicUserNameModel)
                    {
                        if (string.Compare(un.Value.UserName, UserLogin.UserName, true) == 0)
                        {
                            EdocsITSUsersNamesModel m = new EdocsITSUsersNamesModel();
                            m.EdocsCustomerID = un.Value.EdocsCustomerID;
                            m.EdocsCustomerName = un.Value.EdocsCustomerName;
                            m.UserID = un.Value.UserID;
                            m.UserName = un.Value.UserName;
                            keyValuePairs.Add(un.Key, m);
                        }
                    }
                    DicUserNameModel.Clear();
                    DicUserNameModel = keyValuePairs;

                }
                GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, EdocsITSConstants.SessionKeyEdocsITSUserNames, DicUserNameModel).ConfigureAwait(true).GetAwaiter().GetResult();
            }
        }
        private async Task<bool> VerifyInput(EdocsITSUsersModel edocsITSUsers, string errMessUserName, string errMessPassWord, string errMessVerifyPassWord, string errMessEmail,
            string errMessCellPhone, bool genPw, bool changePW)
        {
            genPw = true;
            bool retBool = true;
            if (edocsITSUsers.UserName.Length < 8)
            {
                ModelState.AddModelError(errMessUserName, $"Invalid User Name {edocsITSUsers.UserName} min 8 char");
                retBool = false;
            }
            if (edocsITSUsers.CellPhoneNumber.Length != 10)
            {
                ModelState.AddModelError(errMessCellPhone, $"Invalid Cell Phone Number {edocsITSUsers.CellPhoneNumber}");
                retBool = false;
            }
            var emMatch = Regex.Match(edocsITSUsers.EmailAddress, EdocsITSConstants.ValidEmailAdd, RegexOptions.None);
            if (!(emMatch.Success))
            {
                ModelState.AddModelError(errMessEmail, $"Invalid Email Address {edocsITSUsers.EmailAddress}");
                retBool = false;
            }
            if (!(genPw))
            {
                if (changePW)
                {
                    if (string.IsNullOrWhiteSpace(edocsITSUsers.Password) || (edocsITSUsers.Password.Length < 8))
                    {
                        ModelState.AddModelError(errMessPassWord, $"Invalid Password length must be at least 8");
                        retBool = false;
                        return retBool;
                    }
                    if (string.IsNullOrWhiteSpace(edocsITSUsers.NewPassword) || (edocsITSUsers.NewPassword.Length < 8))
                    {
                        ModelState.AddModelError(errMessVerifyPassWord, $"Invalid Password length must be at least 8");
                        retBool = false;
                        return retBool;
                    }
                    if (string.Compare(edocsITSUsers.Password, edocsITSUsers.NewPassword, false) != 0)
                    {
                        ModelState.AddModelError(errMessPassWord, $"Passwords don't match");
                        retBool = false;
                    }
                    string pwErr = GeneratePassWord.GeneratePassWordInstance.ChekcPasword(edocsITSUsers.NewPassword);
                    if (!(string.IsNullOrWhiteSpace(pwErr)))
                    {
                        ModelState.AddModelError(errMessPassWord, $"Invalid Password {pwErr}");
                    }
                }
            }
            return retBool;
        }
        private bool CheckAuth()
        {
            if (!(EdocsITSHelpers.UserAuth(HttpContext.Session)))
                return false;
            return true;
        }
    }
}
