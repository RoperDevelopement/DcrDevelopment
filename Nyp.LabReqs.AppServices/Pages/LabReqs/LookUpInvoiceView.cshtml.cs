using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using EDocs.Nyp.LabReqs.AppService.Logging;

namespace EDocs.Nyp.LabReqs.AppServices
{
    public class LookUpInvoiceViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog log;

        private string WebApiUrl
        { get { return configuration.GetValue<string>("LabResApi").ToString(); } }
        public string StartDate
        { get; set; }
        public string EndDate
        { get; set; }

        public IDictionary<string, InvoicesModel> IDicInvoiceModel
        { get; private set; }

        public IList<string> InvoiceDep
        { get; private set; }
        public IList<string> InvoiceCat
        { get; private set; }
        public IList<string> InvoiceAccount
        { get; private set; }

        public InvoicesModel ModelInvoice
        { get; set; }
        public string TotalQueryTime
        { get; set; }
        public LookUpInvoiceViewModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            log = logConfig;
        }

        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }


        public async Task<IActionResult> OnGetAsync(InvoicesModel ModelInvoice)
        {
            try
            {
                var qString = Request.QueryString;
                if (!(User.Identity.IsAuthenticated))
                {
                    if (qString.HasValue)
                        return Redirect($"/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpInvoiceView{qString.Value}");
                    else
                        return Redirect("/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpInvoiceView");
                }
                ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
                if (ViewData["CWID"] == null)
                    return Redirect("/NypUsersInfo/LoginView?returnUrl=/LabReqs/LookUpInvoiceView");

                LabReqHelpers.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Start LookUpInvoiceViewModel");
                GetViewAudLogsAdmin();


                if (qString.HasValue)
                {
                    log.LogInformation($"Class LookUpInvoiceViewModel query string: {qString.Value}");
                    await GetInvoices(ModelInvoice, Request.Query[ConstNypLabReqs.FormValueSearchBy]).ConfigureAwait(true);
                }
                await InvoiceList().ConfigureAwait(true);
                TotalQueryTime = LabReqHelpers.StopStopWatch();
                log.LogInformation($"End LookUpInvoiceViewModel total time: {TotalQueryTime} ms");
                return Page();
            }
            catch (Exception ex)
            {
                log.LogError($"LookUpInvoiceViewModel {LabReqHelpers.StopStopWatch()} ms {ex.Message}");


                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model LookUpInvoiceViewModel OngettAsync {ex.Message}");
            }
          //  return Page();
        }
        private void GetViewAudLogsAdmin()
        {
            LabReqsIsAdminVeiwAuditLogs IsAdminVeiwAuditLogs = LabReqsConstants.GetSessionVariables.SessionVarInstance.GetJsonSessionObject<LabReqsIsAdminVeiwAuditLogs>(HttpContext.Session, ConstNypLabReqs.SessionIsAdminVeiwAuditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData["IsAdmin"] = IsAdminVeiwAuditLogs.IsAdmin;
            ViewData["ViewAuditLogs"] = IsAdminVeiwAuditLogs.ViewAuditLogs;
            ViewData["EditLRDocs"] = IsAdminVeiwAuditLogs.EditLRDocs;
        }
        private async Task GetInvoices(InvoicesModel invoicesModel, string srchLogScanDate)
        {
            DateTime dtEndTime = DateTime.Now;
            DateTime dtStTime = DateTime.Now;

            if (LabReqHelpers.GetStartEndDate(Request.Query[ConstNypLabReqs.FormValueStartSDate].ToString(), Request.Query[ConstNypLabReqs.FormValueEndSDate].ToString(), ref dtStTime, ref dtEndTime))
            {
                invoicesModel.InvoiceDate = dtStTime;
                invoicesModel.ScanDate = dtEndTime;
                StartDate = dtStTime.ToString("yyyy-MM-dd");
                EndDate = dtEndTime.ToString("yyyy-MM-dd");
                invoicesModel.ScanDate = LabReqHelpers.AddDayToSearchEndDate(invoicesModel.ScanDate);
            }
            if (string.IsNullOrWhiteSpace(srchLogScanDate))
                invoicesModel.ScanMachine = ConstNypLabReqs.NA;
            else
                invoicesModel.ScanMachine = srchLogScanDate;
            invoicesModel.Account = Request.Query[ConstNypLabReqs.FormValueInvAcct];
            invoicesModel.Department = Request.Query[ConstNypLabReqs.FormValueInvDep];
            invoicesModel.Category = Request.Query[ConstNypLabReqs.FormValueInvCat];
            log.LogInformation($"Search Invoice search by:{srchLogScanDate} {invoicesModel.Account} {invoicesModel.Category} {invoicesModel.Department} {invoicesModel.Reference} {invoicesModel.ScanOperator} start date: {invoicesModel.InvoiceDate.ToString()} end date: {invoicesModel.ScanDate.ToString()}");
            string spname = $"{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypInvoicesByInvoiceDate)}";
            IDicInvoiceModel = await NypInvoiceApi.InvoiceIntance.NypInvoices(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypInvoiceController}", UriKind.RelativeOrAbsolute), spname, invoicesModel, log).ConfigureAwait(true);
        }
        private async Task InvoiceList()
        {
            InvoiceDep = await GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IList<string>>(HttpContext.Session, "InvoiceDepCache").ConfigureAwait(true);
            if (InvoiceDep == null)
            {
                InvoiceDep = await NypInvoiceApi.InvoiceIntance.NypInvoiceDepartment(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypInvoiceController}", UriKind.RelativeOrAbsolute), $"{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypInvoiceDepartment)}", log).ConfigureAwait(true);
                await GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "InvoiceDepCache", InvoiceDep).ConfigureAwait(true);
            }

            InvoiceCat = await GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IList<string>>(HttpContext.Session, "InvoiceCatCache").ConfigureAwait(true);
            if (InvoiceCat == null)
            {
                InvoiceCat = await NypInvoiceApi.InvoiceIntance.NypInvoiceCategory(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypInvoiceController}", UriKind.RelativeOrAbsolute), $"{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypInvoiceCategory)}", log).ConfigureAwait(true);
                await GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "InvoiceCatCache", InvoiceCat).ConfigureAwait(true);
            }
            InvoiceAccount = await GetSessionVariables.SessionVarInstance.GetJsonSessionObject<IList<string>>(HttpContext.Session, "InvoiceAccountCache").ConfigureAwait(true);
            if (InvoiceAccount == null)
            {
                InvoiceAccount = await NypInvoiceApi.InvoiceIntance.NypInvoiceAccount(new Uri($"{WebApiUrl}{ConstNypLabReqs.ApiNypInvoiceController}", UriKind.RelativeOrAbsolute), $"{string.Format(ConstNypLabReqs.ApiSpecimenRejectionControllerParam, ConstNypLabReqs.SpNypInvoiceAccount)}", log).ConfigureAwait(true);
                await GetSessionVariables.SessionVarInstance.SetSessionOjbjectAsJson(HttpContext.Session, "InvoiceAccountCache", InvoiceAccount).ConfigureAwait(true);
            }


        }

    }
}