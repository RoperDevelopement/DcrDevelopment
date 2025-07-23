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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.NetworkInformation;

namespace Edocs.ITS.AppService.Pages.EdocsITSCustomers
{
    public class EdocsITSAddNewCustomersModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private string fullPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "UploadImages";
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EdocsITSAddNewCustomersModel(IConfiguration config, IHttpClientFactory factoryClient, IWebHostEnvironment webHostEnvironment)
        {
            configuration = config;
            clientFactory = factoryClient;
            _webHostEnvironment = webHostEnvironment;
        }
      
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        
        [BindProperty]
        public EdocsITSCustomersModel EdocsITSCustomersModel
        { get; set; }
        [BindProperty]
        public string CustomerState
        { get; set; }
        public IList<string> StateAbb
        { get; set; }
        public UserLoginModel UserLogin
        { get; set; }
        [BindProperty]
        [Display(Name = "Add Customer Login:")]
        public  bool AddCustomerLogIn
        { get; set; }
        [BindProperty]
        [Display(Name = "Customer Site Admin:")]
        public bool CustomerAdmin
        { get; set; }
        [BindProperty]
        [Display(Name = "Edocs Site Admin:")]
        public bool EdocsAdmin
        { get; set; }
        [BindProperty]
        [Display(Name = "Send Customer Email:")]
        public bool SendCustomerEmail
        { get; set; }
        public string ImgFolder
        { get; set; }
        public string ImgBase64
        { get; set; }
        [BindProperty]
        public FileUploadModel FileUpload { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (!EdocsITSHelpers.CheckAuth(HttpContext.Session))
                return Redirect("/LogInOut/LoginView");

            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            string webRootPath = Path.Combine(_webHostEnvironment.WebRootPath, @"img\edocs logo.gif");
            ImgBase64 = ConvertImageBase64(webRootPath).ConfigureAwait(false).GetAwaiter().GetResult();
            ImgBase64 = $"image/png;base64,{ImgBase64}";
            StateAbb = EdocsITSUtilites.GetStatesABB().ConfigureAwait(false).GetAwaiter().GetResult();
            return Page();
        }
        public IActionResult OnGetMyUploader(IFormCollection MyUploader)
        {
            var qStringd = Request.QueryString;

            if (MyUploader != null)
            {
                 string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload");
              //  string filePath = Path.Combine(uploadsFolder, MyUploader.FileName);
              //  using (var fileStream = new FileStream(filePath, FileMode.Create))
                //{
                  //  MyUploader.CopyTo(fileStream);
              ////  }
              //  return new ObjectResult(new { status = "success" });
            }
            return new ObjectResult(new { status = "fail" });

        }
        public async Task  OnPostUpload(IFormFile MyUploader)
        {
            //if (!Directory.Exists(fullPath))
            //{
            //    Directory.CreateDirectory(fullPath);
            //}
             var formFile = MyUploader.FileName;
            //var filePath = Path.Combine(fullPath, formFile.FileName);

            //using (var stream = System.IO.File.Create(filePath))
            //{
            //    formFile.CopyToAsync(stream);
            //}
            string webRootPath = Path.Combine(_webHostEnvironment.WebRootPath, @"img\excel.png");
             ImgBase64 = ConvertImageBase64(webRootPath).ConfigureAwait(false).GetAwaiter().GetResult();
            ImgBase64 = $"image/png;base64,{ImgBase64}";
            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);

            StateAbb = EdocsITSUtilites.GetStatesABB().ConfigureAwait(false).GetAwaiter().GetResult();
            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.
          //  ViewData["SuccessMessage"] = formFile.FileName.ToString() + " files uploaded!!";
            // return Page();

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.
             
        }
        public async Task<string> ConvertImageBase64(string filePath)
        {
            try
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);

                //using (System.Drawing.Image image = System.Drawing.Image.FromFile(filePath))
                //{
                //    using (MemoryStream m = new MemoryStream())
                //    {
                //        image.Save(m, image.RawFormat);
                //        byte[] imageBytes = m.ToArray();

                //        // Convert byte[] to Base64 String
                         string base64String = Convert.ToBase64String(imageBytes);

                         return base64String;
                //    }
                //}
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }
        //public async Task<IActionResult> OnPostAsync(EdocsITSCustomersModel EdocsITSCustomersModel, string CustomerState, bool DeactiveCustomer, FileUploadModel FileUpload)
             public async Task<IActionResult> OnPostAsync(EdocsITSCustomersModel EdocsITSCustomersModel, string CustomerState, bool DeactiveCustomer)
        {
            if (!CheckAuth())
                return Redirect("/LogInOut/LoginView");
            // var sUser = Request.Query["fileElem"].ToString();
//
       //     if (!Directory.Exists(fullPath))
        //    {
           //     Directory.CreateDirectory(fullPath);
          //  }
         //   var formFile = FileUpload.FormFile;
            //if (formFile.Length > 0)
            //{
            //    var filePath = Path.Combine(fullPath, formFile.FileName);

            //    using (var stream = System.IO.File.Create(filePath))
            //    {
            //        //FileUpload.FormFile.FormFile.CopyToAsync(stream);
            //    }
            //}

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.
           // ViewData["SuccessMessage"] = formFile.FileName.ToString() + " files uploaded!!";
            //return Page();

            ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
            UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
            ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
            if(string.IsNullOrWhiteSpace(EdocsITSCustomersModel.ImgStr))
            {
                string webRootPath = Path.Combine(_webHostEnvironment.WebRootPath, @"img\edocs logo.gif");
                EdocsITSCustomersModel.ImgStr = $"data:image/png;base64,{ConvertImageBase64(webRootPath).ConfigureAwait(false).GetAwaiter().GetResult()}";
            }
            EdocsITSCustomersModel.EdosCustomerState = CustomerState;
            EdocsITSCustomersModel.Active = true;
            EdocsITSCustomersModel.EdocsCustomerModifyBy = UserLogin.UserName;
            EdocsITSApis.AddNewCustomer(clientFactory, WebApiUrl, EdocsITSConstants.EdocsITSCustomersController, EdocsITSCustomersModel).ConfigureAwait(true).GetAwaiter().GetResult();
            StateAbb = EdocsITSUtilites.GetStatesABB().ConfigureAwait(false).GetAwaiter().GetResult();

            return Redirect("/EdocsITSCustomers/EdocsITSAddNewCustomers");
        }
        //public IActionResult OnPostUploadFile(IFormFile postedFile)
        //{
        //    string fileName = Path.GetFileName(postedFile.FileName);
        //    string contentType = postedFile.ContentType;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        postedFile.CopyTo(ms);
                 
                 
                
        //    }
        //    return Page();
             
        //}
        private bool CheckAuth()
        {
            if (!(EdocsITSHelpers.UserAuth(HttpContext.Session)))
                return false;
            return true;
        }
    }
    
}

//ViewData[GetSessionVariables.SessionKeyUserName] = GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserName).ConfigureAwait(true).GetAwaiter().GetResult();
//UserLogin = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<UserLoginModel>(HttpContext.Session, EdocsITSConstants.SessionUserInfo).ConfigureAwait(false).GetAwaiter().GetResult();
//ViewData[GetSessionVariables.SessionKeyUserName] = EdocsITSHelpers.RepStr(ViewData[GetSessionVariables.SessionKeyUserName].ToString(), EdocsITSHelpers.QUOTE, string.Empty);
//public UserLoginModel UserLogin
//{ get; set; }