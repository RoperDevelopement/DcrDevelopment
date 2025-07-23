using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Edocs.ITS.AppService.ApisConst;
using System.Net.Http;
using Edocs.ITS.AppService.Models;
using System.ComponentModel.DataAnnotations;
using Edocs.ITS.AppService.Interfaces;
using Newtonsoft.Json.Linq;
using System.Security.Policy;
using System.Xml.Linq;

namespace Edocs.ITS.AppService.Pages.ITSLookUpDocuments
{
    public class ITSPSUSDLookUpDocViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public UserLoginModel UserLogin
        { get; set; }
        [Display(Name = "Date of Records")]
        [DataType(DataType.Date)]
        public DateTime RepStartDate
        { get; set; }
        [BindProperty]
        [DataType(DataType.Date)]
        //  [Display(Name = "Document End Date")]
        public DateTime RepEndDate
        { get; set; }
        //   List<object> RecordsModel
        //   { get; set; }
        public List<PSUSDDepartmentModel> DepModel
        { get; set; }
        public List<PSUSDOrginationDepartmentModel> OrgDep
        { get; set; }
        public ITSPSUSDLookUpDocViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }
        public async Task<IActionResult> OnGetAsync(string sDate, string endDate)
        {

            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");
            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(sDate))
            {
                List<PSUSDDateOfRecordsModel> RecordsModel = EdocsITSApis.GetPSUSDLookRecordsDateRange(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSPSUSDRecordsController, EdocsITSConstants.SpGetPSUSDRecordSearch, "NA", "NA", DateTime.Now, DateTime.Now).ConfigureAwait(false).GetAwaiter().GetResult();
                if ((RecordsModel != null) && (RecordsModel.Count() > 0))
                {

                    RepStartDate = RecordsModel[0].RecordStartDate;
                    RepEndDate = RecordsModel[0].RecordEndDate;

                    // List<PSUSDDateOfRecordsModel> myAnythingList = (RecordsModel as IEnumerable<PSUSDDateOfRecordsModel>).Cast<PSUSDDateOfRecordsModel>().ToList();

                }
                else
                {
                    RepStartDate = DateTime.Now.AddDays(-30);
                    RepEndDate = DateTime.Now.AddDays(1);
                }
            }
            else
            {
                RepStartDate = DateTime.Parse(sDate);
                RepEndDate = DateTime.Parse(endDate);
            //    List<PSUSDDateOfRecordsModel> RecordsModel = EdocsITSApis.GetPSUSDLookRecordsDateRange(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSPSUSDRecordsController, EdocsITSConstants.SpGetPSUSDRecordSearch, "NA", "NA", DateTime.Parse(sDate), DateTime.Parse(endDate)).ConfigureAwait(false).GetAwaiter().GetResult();
            //    if ((RecordsModel != null) && (RecordsModel.Count() > 0))
            //    {

            //        RepStartDate = RecordsModel[0].RecordStartDate;
            //        RepEndDate = RecordsModel[0].RecordEndDate;

            //        // List<PSUSDDateOfRecordsModel> myAnythingList = (RecordsModel as IEnumerable<PSUSDDateOfRecordsModel>).Cast<PSUSDDateOfRecordsModel>().ToList();

            //    }
            //    else
            //    {
            //        RepStartDate = DateTime.Now.AddDays(-30);
            //        RepEndDate = DateTime.Now.AddDays(1);
            //    }
             }
            DepModel = EdocsITSApis.GetPSUSDLookRecordsDept(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSPSUSDRecordsController, EdocsITSConstants.SpGetPSUSDRecordSearch, "Department", "Department", RepStartDate, RepEndDate).ConfigureAwait(false).GetAwaiter().GetResult();
            OrgDep = EdocsITSApis.GetPSUSDLookRecordsOrginationDepartment(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSPSUSDRecordsController, EdocsITSConstants.SpGetPSUSDRecordSearch, "OrginationDepartment", "OrginationDepartment", RepStartDate, RepEndDate).ConfigureAwait(false).GetAwaiter().GetResult();

            return Page();
        }
    }
}
