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

namespace Edocs.ITS.AppService.Pages.Partial_Pages
{
    public class UploadImageViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(EdocsITSConstants.JsonWebApi); } }

           
    public string ImgFolder
    { get; set; }
    public string ImgBase64
    { get; set; }
    public UploadImageViewModel(IConfiguration config, IHttpClientFactory factoryClient, IWebHostEnvironment webHostEnvironment)
        {
            configuration = config;
            clientFactory = factoryClient;
            _webHostEnvironment = webHostEnvironment;


        }
         
        public async Task<IActionResult> OnGet(IFormFile file)
        {
            var qString = Request.QueryString;
            //var path = Path.Combine(
            //            Directory.GetCurrentDirectory(), "wwwroot/uploads",
            //            image.FileName);
            //var memory = new MemoryStream();
            //using (var stream = new FileStream(path, FileMode.Create))
            //{
            //    await image.CopyToAsync(stream);
            //}
            string webRootPath = Path.Combine(_webHostEnvironment.WebRootPath, @"img\excel.png");
            ImgBase64 = ConvertImageBase64(webRootPath).ConfigureAwait(false).GetAwaiter().GetResult();
            ImgBase64 = $"image/png;base64,{ImgBase64}";
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(IFormFile MyUploader)
        {
            var qString = Request.QueryString;
            //var path = Path.Combine(
            //            Directory.GetCurrentDirectory(), "wwwroot/uploads",
            //            image.FileName);
            //var memory = new MemoryStream();
            //using (var stream = new FileStream(path, FileMode.Create))
            //{
            //    await image.CopyToAsync(stream);
            //}
            string webRootPath = Path.Combine(_webHostEnvironment.WebRootPath, @"img\excel.png");
            ImgBase64 = ConvertImageBase64(webRootPath).ConfigureAwait(false).GetAwaiter().GetResult();
            ImgBase64 = $"image/png;base64,{ImgBase64}";
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
