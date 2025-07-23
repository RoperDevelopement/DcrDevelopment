using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDocs.Nyp.LabReqs.AppService.Logging;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Microsoft.VisualBasic;

namespace EDocs.Nyp.LabReqs.AppServices.Pages.LabReqs
{
    public class EditLabReqsViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog log;
        private string WebApiUrl
        { get { return configuration.GetValue<string>("LabResApi").ToString(); } }


        public int LID
        { get; set; }

        public LabReqsEditModel LabReqs
        { get; set; }
        public EditLabReqsViewModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            log = logConfig;
        }
        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<IActionResult> OnGetAsync(int labReqID)
        {
            try
            {


                var qString = Request.QueryString;
                if (!(User.Identity.IsAuthenticated))
                {
                    if (qString.HasValue)
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/EditLabReqsView{qString.Value}");
                    else
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpLabReqsView");
                }
                ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
                if (ViewData["CWID"] == null)
                    return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpLabReqsView");
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                LabReqHelpers.StartStopWatch();
                log.LogInformation("Start EditLabReqsViewModel");
                log.LogInformation($"User {User.Identity.Name} editing labreq id {labReqID}");
                GetViewAudLogsAdmin();

                Init(labReqID).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                log.LogError($"EditLabReqsViewModel  total time: {LabReqHelpers.StopStopWatch()} {ex.Message}");
                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model EditLabReqsViewModel OngettAsync {ex.Message}");
            }


            log.LogInformation($"End EditLabReqsViewModel total time: {LabReqHelpers.StopStopWatch()} ms");
            return Page(); ;
        }
        public async Task<IActionResult> OnPostAsync(LabReqsEditModel LabReqs, int lrid)
        {
            if (!(User.Identity.IsAuthenticated))
            {

                return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/EditLabReqsView?labReqID={lrid}");

            }
            ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
            if (ViewData["CWID"] == null)
                return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpLabReqsView");
            try
            {

                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Start PostAsunc EditLabReqsViewModel");
                log.LogInformation($"User {User.Identity.Name} updating labreq id {lrid}");
                LabReqs.LabReqID = lrid;
                LabReqs.PatientID = LabReqs.MRN;

                if (CheckForChanges(LabReqs).ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    LabReqs.ModifyBy = User.Identity.Name; ;
                    GetNypLabReqs.NypLabReqsApisInctance.UpDateLabReqEdit(WebApiUrl, ConstNypLabReqs.ApiEditLabReqsController, LabReqs, log).ConfigureAwait(false).GetAwaiter().GetResult();
                    ModelState.AddModelError("NoChangesFound", "LabReq Changed Information");
                    GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, ConstNypLabReqs.SessionLabReqEdit, null).ConfigureAwait(false).GetAwaiter().GetResult();
                    //  return Redirect($"/LabReqs/LookUpLabReqsView");
                }
             
                Init(lrid).ConfigureAwait(false).GetAwaiter().GetResult();
               
                GetViewAudLogsAdmin();
            }
            catch (Exception ex)
            {
                log.LogError($"EditLabReqsViewModel  total time: {LabReqHelpers.StopStopWatch()} {ex.Message}");
                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model EditLabReqsViewModel PostAsync {ex.Message}");
            }
            // 
            log.LogError($" End EditLabReqsViewModel PostAsync total time: {LabReqHelpers.StopStopWatch()}");
            return Page();
        }
        private async Task Init(int labReqID)
        {
            log.LogInformation($"EditLabReqsViewModel  getting labreq id {labReqID} for user {User.Identity.Name}");
            if (!(User.Identity.IsAuthenticated))
            {

                Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/EditLabReqsView?labReqID={labReqID}");

            }
            LabReqs = LabReqsConstants.GetSessionVariables.SessionVarInstance.GetJsonSessionObject<LabReqsEditModel>(HttpContext.Session, ConstNypLabReqs.SessionLabReqEdit).ConfigureAwait(false).GetAwaiter().GetResult();
            if (LabReqs == null)
            {
                LabReqs = GetNypLabReqs.NypLabReqsApisInctance.GetLabReqToEdit(WebApiUrl, ConstNypLabReqs.ApiEditLabReqsController, labReqID, log).ConfigureAwait(false).GetAwaiter().GetResult();
                if (LabReqs != null)
                {
                    LID = LabReqs.LabReqID;
                    GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, ConstNypLabReqs.SessionLabReqEdit, LabReqs).ConfigureAwait(false).GetAwaiter().GetResult();

                }
                else
                    log.LogError($"EditLabReqsViewModel  labreq id {labReqID} for user {User.Identity.Name} not found");
            }
        }
        private async Task<bool> CheckForChanges(LabReqsEditModel LabReqs)
        {
            LabReqsEditModel editModel = LabReqsConstants.GetSessionVariables.SessionVarInstance.GetJsonSessionObject<LabReqsEditModel>(HttpContext.Session, ConstNypLabReqs.SessionLabReqEdit).ConfigureAwait(false).GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(LabReqs.IndexNumber))
            {
                ModelState.AddModelError("ErrIndexNumber", "Index Number cannot be blank");
                return false;
            }
            if (LabReqs.IndexNumber.Length < 9)
            {
                ModelState.AddModelError("ErrIndexNumber", $"Invalid Index Number {LabReqs.IndexNumber} not enought chars");
                return false;
            }
            if (string.Compare(LabReqs.IndexNumber, editModel.IndexNumber, false) != 0)
                return true;

            if (string.IsNullOrWhiteSpace(LabReqs.FinancialNumber))
            {
                ModelState.AddModelError("ErrFinancialNumber", "Financial Number cannot be blank");
                return false;
            }
            if (LabReqs.IndexNumber.Length < 8)
            {
                ModelState.AddModelError("ErrFinancialNumber", $"Financial Number  {LabReqs.FinancialNumber} not enought chars");
                return false;
            }
            if (string.Compare(LabReqs.FinancialNumber, editModel.FinancialNumber, false) != 0)
                return true;


            if (string.IsNullOrWhiteSpace(LabReqs.MRN))
            {
                ModelState.AddModelError("ErrMRN", "MRN/Patient ID cannot be blank");
                return false;
            }
            if (LabReqs.MRN.Length < 6)
            {
                ModelState.AddModelError("ErrMRN", $"MRN/Patient ID  {LabReqs.MRN} not enought chars");
                return false;
            }
            if (string.Compare(LabReqs.MRN, editModel.MRN, false) != 0)
                return true;

            if (!(string.IsNullOrWhiteSpace(LabReqs.RequisitionNumber)))
            {
                if (LabReqs.RequisitionNumber.Length < 6)
                {
                    ModelState.AddModelError("ErrRequisitionNumber", $"Requisition Number  {LabReqs.RequisitionNumber} not enought chars");
                    return false;
                }
                if (string.Compare(LabReqs.RequisitionNumber, editModel.RequisitionNumber, false) != 0)
                    return true;
            }
            if (!(string.IsNullOrWhiteSpace(LabReqs.PatientFName)))
            {
                if (LabReqs.PatientFName.Length < 5)
                {
                    ModelState.AddModelError("ErrPatientFName", $"Patient First Name  {LabReqs.PatientFName} not enought chars");
                    return false;
                }
                if (string.Compare(LabReqs.PatientFName, editModel.PatientFName, false) != 0)
                    return true;
            }
            if (!(string.IsNullOrWhiteSpace(LabReqs.PatientLName)))
            {
                if (LabReqs.PatientLName.Length < 5)
                {
                    ModelState.AddModelError("ErrPatientLName", $"Patient Last Name  {LabReqs.PatientLName} not enought chars");
                    return false;
                }
                if (string.Compare(LabReqs.PatientLName, editModel.PatientLName, false) != 0)
                    return true;
            }
            if (!(string.IsNullOrWhiteSpace(LabReqs.ClientCode)))
            {
                if (LabReqs.ClientCode.Length < 4)
                {
                    ModelState.AddModelError("ErrClientCode", $"Client Code  {LabReqs.ClientCode} not enought chars make four 0000 e.g. 0000");
                    return false;
                }
                if (string.Compare(LabReqs.ClientCode, editModel.ClientCode, false) != 0)
                    return true;
            }
            if (!(string.IsNullOrWhiteSpace(LabReqs.DrFName)))
            {
                if (LabReqs.ClientCode.Length < 4)
                {
                    ModelState.AddModelError("ErrDrFName", $"Dr First Name  {LabReqs.DrFName} not enought chars make four 0000 e.g. 0000");
                    return false;
                }
                if (string.Compare(LabReqs.DrFName, editModel.DrFName, false) != 0)
                    return true;
            }

            if (!(string.IsNullOrWhiteSpace(LabReqs.DrLName)))
            {
                if (LabReqs.DrLName.Length < 4)
                {
                    ModelState.AddModelError("ErrDrLName", $"Dr Last Name  {LabReqs.DrLName} not enought chars make four 0000 e.g. 0000");
                    return false;
                }
                if (string.Compare(LabReqs.DrLName, editModel.DrLName, false) != 0)
                    return true;
            }

            if (!(string.IsNullOrWhiteSpace(LabReqs.DrCode)))
            {
                if (LabReqs.DrCode.Length < 4)
                {
                    ModelState.AddModelError("ErrDrCode", $"Dr Code  {LabReqs.DrCode} not enought chars make four 0000 e.g. 0000");
                    return false;
                }
                if (string.Compare(LabReqs.DrCode, editModel.DrCode, false) != 0)
                    return true;
            }
            if ((LabReqs.DateOfService.Day != editModel.DateOfService.Day))
                return true;
            if ((LabReqs.DateOfService.Month != editModel.DateOfService.Month))
                return true;
            if ((LabReqs.DateOfService.Year != editModel.DateOfService.Year))
                return true;
            ModelState.AddModelError("NoChangesFound", "No LabReq Changes Found");
            return false;
        }
        private void GetViewAudLogsAdmin()
        {

            LabReqsIsAdminVeiwAuditLogs IsAdminVeiwAuditLogs = LabReqsConstants.GetSessionVariables.SessionVarInstance.GetJsonSessionObject<LabReqsIsAdminVeiwAuditLogs>(HttpContext.Session, ConstNypLabReqs.SessionIsAdminVeiwAuditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData["IsAdmin"] = IsAdminVeiwAuditLogs.IsAdmin;
            ViewData["ViewAuditLogs"] = IsAdminVeiwAuditLogs.ViewAuditLogs;
            ViewData["EditLRDocs"] = IsAdminVeiwAuditLogs.EditLRDocs;
        }
    }
}
