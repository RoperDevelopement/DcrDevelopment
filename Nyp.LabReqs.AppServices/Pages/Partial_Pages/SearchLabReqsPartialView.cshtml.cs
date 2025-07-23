using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using EDocs.Nyp.LabReqs.AppServices.Models;
using EDocs.Nyp.LabReqs.AppServices.ApiClasses;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using EDocs.Nyp.LabReqs.AppService.Logging;
namespace EDocs.Nyp.LabReqs.AppServices
{
    public class SearchLabReqsPartialViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private ILog log;
        private string WebApiUrl => configuration.GetValue<string>("LabResApi").ToString();


        public SearchLabReqsPartialViewModel(IConfiguration config, ILog logConfig)
        {
            configuration = config;
            LabReqs = new Dictionary<int, LabReqsModel>();
            SearchLabReqs = new LabReqsModel();
            log = logConfig;


        }

        public IDictionary<int, LabReqsModel> LabReqs
        { get; set; }

        [BindProperty]
        public LabReqsModel SearchLabReqs
        { get; set; }

        public string TotalQueryTime
        { get; set; }
        public LabReqsIsAdminVeiwAuditLogs IsAdminVeiwAuditLogs
        { get; set; }
        public string Message
        { get; set; }
        private async Task GetLogInformaiton()
        {

            log = InitAuditLogs.LogAsync(log, HttpContext.Session).ConfigureAwait(false).GetAwaiter().GetResult();
        }


        public async Task<IActionResult> OnGetAsync()
        {

            try
            {
                ViewData["CWID"] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(true).GetAwaiter().GetResult();
                if (ViewData["CWID"] == null)
                    return new RedirectToPageResult("/Index");
                var qString = Request.QueryString;

                LabReqHelpers.StartStopWatch();
                GetLogInformaiton().ConfigureAwait(false).GetAwaiter().GetResult();
                log.LogInformation("Start SearchLabReqsPartialViewModel");
                GetViewAudLogsAdmin();



                if (qString.HasValue)
                {
                    log.LogInformation($"Class SearchLabReqsPartialViewModel query string: {qString.Value}");
                    await LookUpLabReqs(Request.Query[ConstNypLabReqs.FormValueSearchlr].ToString()).ConfigureAwait(true);
                    //await LookUpLabReqs().ConfigureAwait(true);

                }



                if ((LabReqs == null) || (LabReqs.Count == 0))
                {
                    log.LogInformation($"SearchLabReqsPartialViewModel no results returned for query string {qString.Value}");
                }
                TotalQueryTime = LabReqHelpers.StopStopWatch();
                log.LogInformation($"End SearchLabReqsPartialViewModel total time: {TotalQueryTime} ms");


            }
            catch (Exception ex)
            {
                log.LogError($"Class SearchLabReqsPartialViewModel total time: {LabReqHelpers.StopStopWatch()} ms {ex.Message}");
                //  RedirectToPage($"/Error?ErrMEss=Model SearchLabReqsPartialViewModel  OngettAsync {ex.Message}");
                return Redirect($"/NypUsersInfo/DisplayErrorMessages?ErrMEss=Model SearchLabReqsPartialViewModel  OngettAsync {ex.Message}");
            }
            return Page();
        }

        private void GetViewAudLogsAdmin()
        {
              IsAdminVeiwAuditLogs = LabReqsConstants.GetSessionVariables.SessionVarInstance.GetJsonSessionObject<LabReqsIsAdminVeiwAuditLogs>(HttpContext.Session, ConstNypLabReqs.SessionIsAdminVeiwAuditLogs).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData["IsAdmin"] = IsAdminVeiwAuditLogs.IsAdmin;
            ViewData["ViewAuditLogs"] = IsAdminVeiwAuditLogs.ViewAuditLogs;
            ViewData["EditLRDocs"] = IsAdminVeiwAuditLogs.EditLRDocs;
        }

        private bool SearchPartial()
        {
            string searchPart = Request.Query[ConstNypLabReqs.FormValueCbSearchPartial].ToString().Trim();
            if ((string.Compare(searchPart, "searchpart", true) == 0) || (string.Compare(searchPart, "true", true) == 0))
                return true;


            return false;
        }
        private async Task LookUpLabReqs(string searchStr)
        {
            IDictionary<int, LabReqsModel> LabReqs = new Dictionary<int, LabReqsModel>();

            SearchLabReqs.ScanMachine = Request.Query[ConstNypLabReqs.FormValueSearchByLogScanDate].ToString();
            if (string.IsNullOrWhiteSpace(SearchLabReqs.ScanMachine))
            {
                SearchLabReqs.ScanMachine = "receiptDate";
                log.LogInformation("Search LabReqs by Receipt Date");
                Message = "LabReq's Returned 0";
            }
            else
            {
                log.LogInformation($"Search LabReqs by Scan Date ");
                Message = LabReqHelpers.CheckDate(Request.Query[ConstNypLabReqs.FormValueStartSDate].ToString().Trim(), Request.Query[ConstNypLabReqs.FormValueEndSDate].ToString().Trim());
                if ((string.IsNullOrWhiteSpace(Message)))
                    Message = "LabReq's Returned 0";
                else
                {
                    log.LogError($"Error with date: {Message}");
                    return;
                }
                    
                log.LogInformation($"Search LabReqs by Scan Date ");
            }
            switch (searchStr)
            {
                case ConstNypLabReqs.FormValueSearchLabReqs:
                    await LookUpLabReqsByLabIndexFinNum().ConfigureAwait(true);
                    break;
                case ConstNypLabReqs.FormValueSearchByCSNNum:
                    await LookUpLabReqsByCSNNum().ConfigureAwait(true);
                    break;
                case ConstNypLabReqs.FormValueSearchByIndexNumberCsnNumber:
                    await LookUpLabReqsByIndexNumberCSNNum().ConfigureAwait(true);
                    break;

                    
                case ConstNypLabReqs.FormValueSearchByPatId:
                    await LookUpLabReqsByPatientId().ConfigureAwait(true);
                    break;
                case ConstNypLabReqs.FormValueSearchByMRN:
                    await LookUpLabReqsByMRN().ConfigureAwait(true);
                    break;
                case ConstNypLabReqs.FormValueSearchByKeyWord:
                    await LookUpLabReqsByKeyWords().ConfigureAwait(true);
                    break;

                case ConstNypLabReqs.FormValueSearchByClientCode:
                    await LookUpLabReqsByClinetCode().ConfigureAwait(true);
                    break;
                case ConstNypLabReqs.FormValueSearchByDrCode:
                    await LookUpLabReqsByDrCode().ConfigureAwait(true);
                    break;
                case ConstNypLabReqs.FormValueSearchByPatFName:
                    await LookUpLabReqsByPFName().ConfigureAwait(true);
                    break;
                case ConstNypLabReqs.FormValueSearchByPatLName:
                    await LookUpLabReqsByLName().ConfigureAwait(true);
                    break;
                case ConstNypLabReqs.FormValueSearchByDrName:
                    await LookUpLabReqsByDrName().ConfigureAwait(true);
                    break;
                case ConstNypLabReqs.FormValueSearchByScanOperator:
                    await LookUpLabReqsByScanOperator().ConfigureAwait(true);
                    break;
                case ConstNypLabReqs.FormValueSearchByBatchId:
                    await LookUpLabReqsByBatchId().ConfigureAwait(true);
                    break;
                case ConstNypLabReqs.FormValueSearchByRecDate:
                    if (string.Compare(SearchLabReqs.ScanMachine, ConstNypLabReqs.FormValueSearchByLogDate, true) == 0)
                        await LookUpLabReqsByDate("RedDate").ConfigureAwait(true);
                    else
                        await LookUpLabReqsByDate("ScanDate").ConfigureAwait(true);
                    break;
                case ConstNypLabReqs.FormValueSearchByReqNum:
                    await LookUpLabReqsByReqNum().ConfigureAwait(true);
                    break;
                case ConstNypLabReqs.FormValueSearchByDosDate:
                    await LookUpLabReqsByDate("DoS").ConfigureAwait(true);
                    break;
                case ConstNypLabReqs.FormValueSearchByScanDate:
                    await LookUpLabReqsByDate("ScanDate").ConfigureAwait(true);
                    break;




                default: throw new Exception($"Search bu {searchStr} not found");
            }
        }
        private void GetStartEndDate()
        {
            string seDate = Request.Query[ConstNypLabReqs.FormValueStartSDate].ToString().Trim();
            log.LogInformation($"Method GetStartEndDate Start date {seDate}");
            if (!(string.IsNullOrWhiteSpace(seDate)))
            {
                SearchLabReqs.ReceiptDate = GetNypLabReqs.NypLabReqsApisInctance.GetLabRecDate(seDate);
                log.LogInformation($"SearchLabReqs.ReceiptDate {SearchLabReqs.ReceiptDate}");
                seDate = Request.Query[ConstNypLabReqs.FormValueEndSDate].ToString();
                SearchLabReqs.ScanDate = GetNypLabReqs.NypLabReqsApisInctance.GetLabRecDate(seDate);
                log.LogInformation($"SearchLabReqs.ScanDate {SearchLabReqs.ScanDate}");
                SearchLabReqs.ScanDate = LabReqHelpers.AddDayToSearchEndDate(SearchLabReqs.ScanDate);
                log.LogInformation($"SearchLabReqs.ScanDate after add days {SearchLabReqs.ScanDate}");

            }




        }

        private async Task LookUpLabReqsByMRN()
        {

            string storedProduces = string.Format(ConstNypLabReqs.ApiNypLabReqsControllerParam, ConstNypLabReqs.SpNypLabMRNRecDate);
            SearchLabReqs.MRN = Request.Query[ConstNypLabReqs.FormValueMRN].ToString();
            SearchLabReqs.SearchPartial = SearchPartial();
            GetStartEndDate();
            log.LogInformation($"Search LabReqs by MRN {SearchLabReqs.MRN} Start Date: {SearchLabReqs.ReceiptDate} End Date: {SearchLabReqs.ScanDate}");
            if (!(string.IsNullOrWhiteSpace(SearchLabReqs.MRN)))
            {
                LabReqs = await GetNypLabReqs.NypLabReqsApisInctance.NypLabReqs($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsController}", storedProduces, SearchLabReqs, log).ConfigureAwait(true);
            }


        }
        private async Task LookUpLabReqsByLabIndexFinNum()
        {

            string storedProduces = string.Format(ConstNypLabReqs.ApiNypLabReqsControllerParam, ConstNypLabReqs.SpNypLabIndexFinancialNumber);
            //  SearchLabReqs.IndexNumber = SearchLabReqs.FinancialNumber = Request.Query[ConstNypLabReqs.FormValueFinIndNum].ToString().Trim();
            SearchLabReqs.IndexNumber = Request.Query[ConstNypLabReqs.FormValueFinIndNum].ToString().Trim();
            SearchLabReqs.SearchPartial = SearchPartial();

            GetStartEndDate();

            if (!(string.IsNullOrWhiteSpace(SearchLabReqs.IndexNumber)))
            {
                //if (SearchLabReqs.FinancialNumber.Length > 9)
                //{
                //    log.LogInformation($"Removing digits from FinancialNumber {SearchLabReqs.FinancialNumber}");

                //    SearchLabReqs.FinancialNumber = SearchLabReqs.FinancialNumber.Substring(6);
                //    log.LogInformation($"New FinancialNumber {SearchLabReqs.FinancialNumber}");
                //}
                //SearchLabReqs.SearchPartial = true;
                log.LogInformation($"Search LabReqs by Index Number {SearchLabReqs.IndexNumber} Start Date: {SearchLabReqs.ReceiptDate} End Date: {SearchLabReqs.ScanDate}");
                LabReqs = await GetNypLabReqs.NypLabReqsApisInctance.NypLabReqs($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsController}", storedProduces, SearchLabReqs, log).ConfigureAwait(true);
            }

        }

        private async Task LookUpLabReqsByKeyWords()
        {

           // string storedProduces = string.Format(ConstNypLabReqs.ApiNypLabReqsControllerParam, ConstNypLabReqs.SpNypLabIndexFinancialNumber);
           string keyWord =  Request.Query[ConstNypLabReqs.FormValueKeyWords].ToString().Trim();
          //  SearchLabReqs.SearchPartial = SearchPartial();

            GetStartEndDate();

            if (!(string.IsNullOrWhiteSpace(keyWord)))
            {
                //if (SearchLabReqs.FinancialNumber.Length > 9)
                //{
                //    log.LogInformation($"Removing digits from FinancialNumber {SearchLabReqs.FinancialNumber}");

                //    SearchLabReqs.FinancialNumber = SearchLabReqs.FinancialNumber.Substring(6);
                //    log.LogInformation($"New FinancialNumber {SearchLabReqs.FinancialNumber}");
                //}
                //SearchLabReqs.SearchPartial = true;
                log.LogInformation($"Search LabReqs by KeyWord {keyWord} Start Date: {SearchLabReqs.ReceiptDate} End Date: {SearchLabReqs.ScanDate}");
                 LabReqs = await GetNypLabReqs.NypLabReqsApisInctance.GetLabReqsByKeyWords($"{WebApiUrl}", ConstNypLabReqs.ApiNypLabReqsKeyWordsController, SearchLabReqs.ReceiptDate, SearchLabReqs.ScanDate, SearchLabReqs.ScanMachine,keyWord, log).ConfigureAwait(true);
            }

        }
        private async Task LookUpLabReqsByPatientId()
        {

            string storedProduces = string.Format(ConstNypLabReqs.ApiNypLabReqsControllerParam, ConstNypLabReqs.SpNypLabReqsPatientId);
            SearchLabReqs.PatientID = Request.Query[ConstNypLabReqs.FormValuePatID].ToString();
            SearchLabReqs.SearchPartial = SearchPartial();
            GetStartEndDate();
            log.LogInformation($"Search LabReqs by PatientID {SearchLabReqs.PatientID} Start Date: {SearchLabReqs.ReceiptDate} End Date: {SearchLabReqs.ScanDate}");
            if (!(string.IsNullOrWhiteSpace(SearchLabReqs.PatientID)))
            {
                LabReqs = await GetNypLabReqs.NypLabReqsApisInctance.NypLabReqs($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsController}", storedProduces, SearchLabReqs, log).ConfigureAwait(true);
            }


        }
        private async Task LookUpLabReqsByReqNum()
        {

            string storedProduces = string.Format(ConstNypLabReqs.ApiNypLabReqsControllerParam, ConstNypLabReqs.SpNypLabRequestionNumber);
            SearchLabReqs.RequisitionNumber = Request.Query[ConstNypLabReqs.FormValueReqsNum].ToString().Trim();
            SearchLabReqs.SearchPartial = SearchPartial();
            string seDate = Request.Query[ConstNypLabReqs.FormValueStartSDate].ToString();
            GetStartEndDate();
            log.LogInformation($"Search LabReqs by RequisitionNumber {SearchLabReqs.RequisitionNumber} Start Date: {SearchLabReqs.ReceiptDate} End Date: {SearchLabReqs.ScanDate}");
            if (!(string.IsNullOrWhiteSpace(SearchLabReqs.RequisitionNumber)))
            {

                LabReqs = await GetNypLabReqs.NypLabReqsApisInctance.NypLabReqs($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsController}", storedProduces, SearchLabReqs, log).ConfigureAwait(true);
            }

        }

        private async Task LookUpLabReqsByDate(string dosRecScanDate)
        {

            string storedProduces = string.Format(ConstNypLabReqs.ApiNypLabReqsControllerParam, ConstNypLabReqs.SpNypLabReqsDosRecScanDate);
            SearchLabReqs.ScanMachine = dosRecScanDate;
            GetStartEndDate();
            log.LogInformation($"Search LabReqs by Receipt or Scan Date Start Date: {SearchLabReqs.ReceiptDate} End Date: {SearchLabReqs.ScanDate}");
            LabReqs = await GetNypLabReqs.NypLabReqsApisInctance.NypLabReqs($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsController}", storedProduces, SearchLabReqs, log).ConfigureAwait(true);

        }


        private async Task LookUpLabReqsByBatchId()
        {

            string storedProduces = string.Format(ConstNypLabReqs.ApiNypLabReqsControllerParam, ConstNypLabReqs.SpNypLabReqsScanBatch);
            SearchLabReqs.ScanBatch = Request.Query[ConstNypLabReqs.FormValueScanBatchId].ToString().Trim();
            SearchLabReqs.SearchPartial = SearchPartial();
            GetStartEndDate();
            log.LogInformation($"Search LabReqs by ScanBatch {SearchLabReqs.ScanBatch} Start Date: {SearchLabReqs.ReceiptDate} End Date: {SearchLabReqs.ScanDate}");
            if (!(string.IsNullOrWhiteSpace(SearchLabReqs.ScanBatch)))
                LabReqs = await GetNypLabReqs.NypLabReqsApisInctance.NypLabReqs($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsController}", storedProduces, SearchLabReqs, log).ConfigureAwait(true);

        }
        private async Task LookUpLabReqsByScanOperator()
        {

            string storedProduces = string.Format(ConstNypLabReqs.ApiNypLabReqsControllerParam, ConstNypLabReqs.SpNypLabReqsScanOperator);
            SearchLabReqs.ScanOperator = Request.Query[ConstNypLabReqs.FormValueScanOperator].ToString().Trim();
            SearchLabReqs.SearchPartial = SearchPartial();
            GetStartEndDate();
            log.LogInformation($"Search LabReqs by ScanOperator {SearchLabReqs.ScanOperator} Start Date: {SearchLabReqs.ReceiptDate} End Date: {SearchLabReqs.ScanDate}");
            if (!(string.IsNullOrWhiteSpace(SearchLabReqs.ScanOperator)))
                LabReqs = await GetNypLabReqs.NypLabReqsApisInctance.NypLabReqs($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsController}", storedProduces, SearchLabReqs, log).ConfigureAwait(true);

        }

        private async Task LookUpLabReqsByClinetCode()
        {

            string storedProduces = string.Format(ConstNypLabReqs.ApiNypLabReqsControllerParam, ConstNypLabReqs.SpNypLabReqsClientCode);
            SearchLabReqs.ClientCode = Request.Query[ConstNypLabReqs.FormValueClientCode].ToString().Trim();
            SearchLabReqs.SearchPartial = SearchPartial();
            GetStartEndDate();
            log.LogInformation($"Search LabReqs by ClientCode {SearchLabReqs.ClientCode} Start Date: {SearchLabReqs.ReceiptDate} End Date: {SearchLabReqs.ScanDate}");
            if (!(string.IsNullOrWhiteSpace(SearchLabReqs.ClientCode)))
                LabReqs = await GetNypLabReqs.NypLabReqsApisInctance.NypLabReqs($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsController}", storedProduces, SearchLabReqs, log).ConfigureAwait(true);

        }
        private async Task LookUpLabReqsByPFName()
        {


            string storedProduces = string.Format(ConstNypLabReqs.ApiNypLabReqsControllerParam, ConstNypLabReqs.SpNypLabReqsPatientFirstName);
            SearchLabReqs.SearchPartial = SearchPartial();
            string pFName = Request.Query[ConstNypLabReqs.FormValuePFName].ToString().Trim();
            if (string.IsNullOrWhiteSpace(pFName))
                pFName = "N/A";

            string pLname = Request.Query[ConstNypLabReqs.FormValuePLName].ToString().Trim();
            if (string.IsNullOrWhiteSpace(pLname))
                pLname = "N/A";


            SearchLabReqs.PatientName = $"{pFName}@{pLname}";


            GetStartEndDate();
            log.LogInformation($"Search LabReqs by Patient Name {SearchLabReqs.PatientName} Start Date: {SearchLabReqs.ReceiptDate} End Date: {SearchLabReqs.ScanDate}");
            if (!(string.IsNullOrWhiteSpace(SearchLabReqs.PatientName)))
                LabReqs = await GetNypLabReqs.NypLabReqsApisInctance.NypLabReqs($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsController}", storedProduces, SearchLabReqs, log).ConfigureAwait(true);

        }

        private async Task LookUpLabReqsByLName()
        {


            string storedProduces = string.Format(ConstNypLabReqs.ApiNypLabReqsControllerParam, ConstNypLabReqs.SpNypLabReqsPatientLastName);
            SearchLabReqs.SearchPartial = SearchPartial();
            SearchLabReqs.PatientName = Request.Query[ConstNypLabReqs.FormValuePLName].ToString().Trim();
            GetStartEndDate();
            log.LogInformation($"Search LabReqs by Patient Last Name {SearchLabReqs.PatientName} Start Date: {SearchLabReqs.ReceiptDate} End Date: {SearchLabReqs.ScanDate}");
            if (!(string.IsNullOrWhiteSpace(SearchLabReqs.PatientName)))
                LabReqs = await GetNypLabReqs.NypLabReqsApisInctance.NypLabReqs($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsController}", storedProduces, SearchLabReqs, log).ConfigureAwait(true);

        }
        private async Task LookUpLabReqsByDrCode()
        {

            string storedProduces = string.Format(ConstNypLabReqs.ApiNypLabReqsControllerParam, ConstNypLabReqs.SpNypLabReqsDrCode);
            SearchLabReqs.DrCode = Request.Query[ConstNypLabReqs.FormValueDeCode].ToString().Trim();
            SearchLabReqs.SearchPartial = SearchPartial();
            GetStartEndDate();
            log.LogInformation($"Search LabReqs by DrCode {SearchLabReqs.DrCode} Start Date: {SearchLabReqs.ReceiptDate} End Date: {SearchLabReqs.ScanDate}");
            if (!(string.IsNullOrWhiteSpace(SearchLabReqs.DrCode)))
                LabReqs = await GetNypLabReqs.NypLabReqsApisInctance.NypLabReqs($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsController}", storedProduces, SearchLabReqs, log).ConfigureAwait(true);

        }

        private async Task LookUpLabReqsByDrName()
        {

            string storedProduces = string.Format(ConstNypLabReqs.ApiNypLabReqsControllerParam, ConstNypLabReqs.SpNypLabReqsDrName);
            SearchLabReqs.SearchPartial = SearchPartial();
            SearchLabReqs.DrName = Request.Query[ConstNypLabReqs.FormValueDrName].ToString().Trim();
            GetStartEndDate();
            log.LogInformation($"Search LabReqs by DrName {SearchLabReqs.DrName} Start Date: {SearchLabReqs.ReceiptDate} End Date: {SearchLabReqs.ScanDate}");
            if (!(string.IsNullOrWhiteSpace(SearchLabReqs.DrName)))
                LabReqs = await GetNypLabReqs.NypLabReqsApisInctance.NypLabReqs($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsController}", storedProduces, SearchLabReqs, log).ConfigureAwait(true);

        }

        private async Task LookUpLabReqsByCSNNum()
        {

            string storedProduces = string.Format(ConstNypLabReqs.ApiNypLabReqsControllerParam, ConstNypLabReqs.SpNypLabReqsCSNNumber);
            SearchLabReqs.FinancialNumber = Request.Query[ConstNypLabReqs.FormValueCSNNum].ToString().Trim();
            SearchLabReqs.SearchPartial = SearchPartial();
            string seDate = Request.Query[ConstNypLabReqs.FormValueStartSDate].ToString();
            GetStartEndDate();
            log.LogInformation($"Search LabReqs by CSN Number {SearchLabReqs.FinancialNumber} Start Date: {SearchLabReqs.ReceiptDate} End Date: {SearchLabReqs.ScanDate}");
            if (!(string.IsNullOrWhiteSpace(SearchLabReqs.FinancialNumber)))
            {

                LabReqs = await GetNypLabReqs.NypLabReqsApisInctance.NypLabReqs($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsController}", storedProduces, SearchLabReqs, log).ConfigureAwait(true);
            }

        }
        private async Task LookUpLabReqsByIndexNumberCSNNum()
        {

            string storedProduces = string.Format(ConstNypLabReqs.ApiNypLabReqsControllerParam, ConstNypLabReqs.SpNypLabIndexFinancialandCSNNumber);
            SearchLabReqs.FinancialNumber = Request.Query[ConstNypLabReqs.FormValueCSNNum].ToString().Trim();
            SearchLabReqs.IndexNumber = Request.Query[ConstNypLabReqs.FormValueFinIndNum].ToString().Trim();
           SearchLabReqs.SearchPartial = SearchPartial();
            string seDate = Request.Query[ConstNypLabReqs.FormValueStartSDate].ToString();
            GetStartEndDate();
            log.LogInformation($"Search LabReqs by CSN Number and Index Number Csn Number= {SearchLabReqs.FinancialNumber} Index Number = {SearchLabReqs.IndexNumber} Start Date: {SearchLabReqs.ReceiptDate} End Date: {SearchLabReqs.ScanDate}");
            if (!(string.IsNullOrWhiteSpace(SearchLabReqs.FinancialNumber)))
            {

                LabReqs = await GetNypLabReqs.NypLabReqsApisInctance.NypLabReqs($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsController}", storedProduces, SearchLabReqs, log).ConfigureAwait(true);
            }

        }

        private async Task LookUpLabReqs()
        {
            SearchLabReqs = new LabReqsModel();
            string storedProduces = string.Empty;
            foreach (var qs in Request.Query)
            {
                if (string.Compare(qs.Key, ConstNypLabReqs.FormValueSearchlr, System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (string.Compare(qs.Value, ConstNypLabReqs.FormValueSearchLabReqs, System.StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        storedProduces = ConstNypLabReqs.SpNypLabIndexFinancialNumber;
                        SearchLabReqs.SearchPartial = true;
                    }
                    else
                        storedProduces = ConstNypLabReqs.SpNypLabRequestionNumber;
                    continue;
                }

                if (string.Compare(qs.Key, ConstNypLabReqs.FormValueStartSDate, System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (!(string.IsNullOrEmpty(qs.Value)))
                    {
                        SearchLabReqs.ReceiptDate = DateTime.Parse(qs.Value);
                    }
                    continue;
                }
                if (string.Compare(qs.Key, ConstNypLabReqs.FormValueEndSDate, System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (!(string.IsNullOrEmpty(qs.Value)))
                    {
                        SearchLabReqs.ScanDate = DateTime.Parse(qs.Value);
                    }
                    continue;
                }
                if (string.Compare(qs.Key, ConstNypLabReqs.FormValueFinIndNum, System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (!(string.IsNullOrEmpty(qs.Value)))
                    {
                        SearchLabReqs.IndexNumber = SearchLabReqs.FinancialNumber = qs.Value;
                    }
                    continue;
                }
                if (string.Compare(qs.Key, ConstNypLabReqs.FormValueReqsNum, System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (!(string.IsNullOrEmpty(qs.Value)))
                    {
                        SearchLabReqs.RequisitionNumber = qs.Value;
                    }
                    continue;
                }

            }

            LabReqs = await GetNypLabReqs.NypLabReqsApisInctance.NypLabReqs($"{WebApiUrl}{ConstNypLabReqs.ApiNypLabReqsController}", storedProduces, SearchLabReqs, log).ConfigureAwait(true);
        }
    }
}