using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Edocs.ITS.AppService.ApisConst;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Edocs.ITS.AppService.Pages.EdocsITSCustomers
{
    public class ImageViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string TempImgFolder = @"tempimgfolder";
        private readonly string CustImgName = "custimg.png";
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }
        public ImageViewModel(IConfiguration config, IHttpClientFactory factoryClient, IWebHostEnvironment webHostEnvironment)
        {
            configuration = config;
            clientFactory = factoryClient;
            _webHostEnvironment = webHostEnvironment;


        }



        public string ImgBase64
        { get; set; }
        [BindProperty]
        public IFormFile MyUploader
        { get; set; }
        //   public void OnGet(IFormFile file)
        //HttpContext context
        //      public async Task<IActionResult> OnGet(HttpContext context)
        public async Task<IActionResult> OnGet([FromForm] IFormFile MyUploader)
        {
            var qStringd = Request.QueryString;
            //   var qString = Request.HttpContext;
            // // var f= Request.Form.Count;
            //foreach(KeyValuePair<object,object> k in qString.Items)
            //   {
            //       Console.WriteLine();
            //   }
            string webRootPath = Path.Combine(_webHostEnvironment.WebRootPath, @"img\excel.png");
            ImgBase64 = ConvertImageBase64(webRootPath).ConfigureAwait(false).GetAwaiter().GetResult();
            ImgBase64 = $"image/png;base64,{ImgBase64}";
            //   ImgBase64 = ConvertImageBase64(file).ConfigureAwait(false).GetAwaiter().GetResult();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(IFormFile MyUploader)
        //   public async Task<IActionResult> OnGet(IFormFile file)
        {
           
            if (MyUploader != null)
            {


                string webRootPath = Path.Combine(_webHostEnvironment.WebRootPath, TempImgFolder);
                if (!(Directory.Exists(webRootPath)))
                    Directory.CreateDirectory(webRootPath);
                if (System.IO.File.Exists(Path.Combine(webRootPath, CustImgName)))
                    System.IO.File.Delete(Path.Combine(webRootPath, CustImgName));
                string fileName = Path.GetFileName(MyUploader.FileName);
               // byte[] img = System.IO.File.ReadAllBytesAsync(Path.Combine(webRootPath,MyUploader.FileName)).ConfigureAwait(false).GetAwaiter().GetResult();
               // System.IO.File.WriteAllBytesAsync(webRootPath, img).ConfigureAwait(false).GetAwaiter().GetResult();
                using (FileStream stream = new FileStream(Path.Combine(webRootPath, CustImgName), FileMode.Create))
                {
                    //FileName = Path.Combine(path, fileName);
                    MyUploader.CopyTo(stream);
                    //uploadedFiles.Add(fileName);
                    //this.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);

                   
                    // ImgBase64 = $"image/png;base64,{ImgBase64}";
                    //   ImgBase64 = ConvertImageBase64(file).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                ImgBase64 = ConvertImageBase64(Path.Combine(webRootPath, CustImgName)).ConfigureAwait(false).GetAwaiter().GetResult();
                
            }
            return Page();
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return string.Empty;
            }
        }
    }
