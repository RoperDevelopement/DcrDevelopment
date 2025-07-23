using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Edocs.ITS.AppService.ApisConst;
using Edocs.ITS.AppService.Interfaces;
using Edocs.ITS.AppService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Edocs.ITS.AppService.Pages.TrackingSystem
{
    public class PrintsSgnatureViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PrintsSgnatureViewModel(IConfiguration config, IHttpClientFactory factoryClient, IWebHostEnvironment webHostEnvironment)
        {
            configuration = config;
            clientFactory = factoryClient;
            _webHostEnvironment = webHostEnvironment;
        }
        public string SigatureData
        { get; set; }
        public async Task<IActionResult> OnGetAsync(string sigatureFile)
        {
            if(!(string.IsNullOrWhiteSpace(sigatureFile)))
                SigatureData = System.IO.File.ReadAllTextAsync(sigatureFile).ConfigureAwait(false).GetAwaiter().GetResult();

            return Page();
        }
    }
}
