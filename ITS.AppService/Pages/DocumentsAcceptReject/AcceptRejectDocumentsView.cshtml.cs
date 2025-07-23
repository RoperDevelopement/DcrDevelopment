using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Edocs.ITS.AppService.ApisConst;
using Edocs.ITS.AppService.Models;
using Edocs.ITS.AppService.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.IO;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Edocs.ITS.AppService.Pages.DocumentsAcceptReject
{
    public class AcceptRejectDocumentsViewModel : PageModel
    {

        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private string fullPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "UploadImages";
        public AcceptRejectDocumentsViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }
        public UserLoginModel UserLogin
        { get; set; }
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public IList<AcceptRejectDocumentsModel> RejectDocumentsModels
        { get; set; }
        [Display(Name = "Document Start Date")]
        [DataType(DataType.Date)]
        public DateTime RepStartDate
        { get; set; }
        [BindProperty]
        [DataType(DataType.Date)]
        [Display(Name = "Document End Date")]
        public DateTime RepEndDate
        { get; set; }
        public string DocNameTrackID
        { get; set; }
        public string TrackID
        { get; set; }
       public string ViewingDocs
        { get; set; }
        public async Task<IActionResult> OnGetAsync(string repSDate, string repSEndDate, string trackID, string docName)
        {
            //    if (!(string.IsNullOrWhiteSpace(repSDate)))
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");

            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            if (string.IsNullOrWhiteSpace(repSDate))
            {
                RepStartDate = DateTime.Now.AddDays(-15);
                RepEndDate = DateTime.Now.AddDays(1);
            }
                   
           
            if (string.IsNullOrWhiteSpace(trackID) && string.IsNullOrWhiteSpace(docName))
            {
                var qString = Request.QueryString;

                if (qString.HasValue)
                {
                    //    AcceptRejectDocumentsModel acceptReject = new AcceptRejectDocumentsModel()
                    //        {
                    //         ID = int.Parse(Request.Query["fileID"].ToString()),
                    //          AcceptRejectDoc =bool.Parse(Request.Query["accrej"]),
                    //           Comments = Request.Query["comm"],

                    //};
                    AcceptRejectDocumentsModel acceptReject = new AcceptRejectDocumentsModel();
                    acceptReject.ID = int.Parse(Request.Query["fileID"].ToString());
                    repSDate = Request.Query["repStartDate"].ToString();
                    repSEndDate = Request.Query["repEndDate"].ToString();
                    docName = Request.Query["tidDocName"].ToString();
                    trackID = Request.Query["tids"].ToString();
                    DocNameTrackID = docName;
                    TrackID = trackID;
                    if (string.Compare(Request.Query["accrej"].ToString(), "1", true) == 0)
                    {
                        acceptReject.AcceptRejectDoc = true;
                        acceptReject.Comments = $"Document Accepted on [{DateTime.Now.ToString("MM-dd-yyyy")}] by [{UserLogin.UserName}]";
                    }
                    else
                        if (Request.Query["comm"].ToString().ToLower().Contains("rejected"))
                        acceptReject.Comments = $"{Request.Query["comm"].ToString()} Comments Updated on [{DateTime.Now.ToString("MM-dd-yyyy")}] by [{UserLogin.UserName}]";
                   
                        acceptReject.Comments = $"Document Rejected on [{DateTime.Now.ToString("MM-dd-yyyy")}] by [{UserLogin.UserName}] {Request.Query["comm"].ToString()}";

                    EdocsITSApis.UpDateAcceptRejectDocs(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSAcceptRejectDocumentsController, acceptReject, EdocsITSConstants.SPUpdateAccRejectDocs).ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }
            if (!(string.IsNullOrWhiteSpace(repSDate)))
            {
                var qString1 = Request.QueryString;
                RepEndDate = DateTime.Parse(repSEndDate);
                RepStartDate = DateTime.Parse(repSDate);
                DocNameTrackID = docName;
                TrackID = trackID;
                ViewingDocs = $" TrackingID: {trackID}";
                string repType = EdocsITSConstants.ReportTypeTrackID;
                if(string.IsNullOrWhiteSpace(trackID))
                {
                    repType = EdocsITSConstants.ReportTypeDocName;
                    trackID = docName;
                    ViewingDocs = $"Documents: {trackID}";

                }
                RejectDocumentsModels = EdocsITSApis.GetPdfDocuments(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSAcceptRejectDocumentsController, EdocsITSConstants.CurrentCustIDInt, trackID,repType,repSDate,repSEndDate).ConfigureAwait(false).GetAwaiter().GetResult();
                AddQuates().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {

            if (!CheckAuth())
                return Redirect("/LogInOut/LoginView");
            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            RejectDocumentsModels = EdocsITSApis.GetPdfDocuments(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSAcceptRejectDocumentsController, EdocsITSConstants.CurrentCustIDInt).ConfigureAwait(false).GetAwaiter().GetResult();
            AddQuates().ConfigureAwait(false).GetAwaiter().GetResult();
            var qString = Request.QueryString;
            if (qString.HasValue)
            {
                string id = Request.Query["fileID"];
                string accrej = Request.Query["accrej"];
                string comm = Request.Query["comm"];
            }
            return Page();
        }
        private async Task AddQuates()
        {
            foreach (AcceptRejectDocumentsModel acceptReject in RejectDocumentsModels)
            {
                string[] pdfPath = acceptReject.FileName.Split(Path.DirectorySeparatorChar);
                string newPath = string.Empty;
                for (int path = 0; path < pdfPath.Length; path++)
                {
                    newPath += $"{pdfPath[path]}@";
                    if (path == pdfPath.Length - 1)
                    {
                        if (newPath.EndsWith("@"))
                            newPath = newPath.Remove(newPath.Length - 1, 1);
                        acceptReject.FileName = newPath;
                    }
                }

            }
        }
        public JsonResult OnPostGetPDF(int fileId)
        {

            string docName, contentType;
            contentType = "application/pdf";
            docName = @"D:\ArchiverBackup\02-04-2023\dsdsds\dsdsds.pdf";
            byte[] bytes = System.IO.File.ReadAllBytes(docName);
            //  return new JsonResult(new { FileName = docName, ContentType = contentType, Data = bytes });

            return new JsonResult(new { FileName = docName, ContentType = contentType, Data = bytes });
        }
        //private async Task GetPdfFile()
        //{


        //    foreach (AcceptRejectDocumentsModel acceptReject in RejectDocumentsModels)
        //    {
        //        byte[] bytes = System.IO.File.ReadAllBytes(acceptReject.FileName);
        //        // System.IO.Stream stream = new System.IO.MemoryStream(bytes,0,bytes.Length);

        //        string base64String = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        //        acceptReject.PdfFileBytes= Convert.FromBase64String(base64String);
        //        //  System.IO.BinaryReader br = new System.IO.BinaryReader(stream);
        //        //acceptReject.PdfFileBytes = Convert.ToBase64String(bytes, 0, bytes.Length);

        //       // for (var i = 0; i < br.length; i++)
        //        //{
        //        //    bytes[i] = br.charCodeAt(i);
        //       // }
        //    }
        //}
        private bool CheckAuth()
        {
            if (!(EdocsITSHelpers.UserAuth(HttpContext.Session)))
                return false;
            return true;
        }
    }
}
