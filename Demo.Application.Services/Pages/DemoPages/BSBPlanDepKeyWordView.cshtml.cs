using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Edocs.Demo.Application.Services.ApiConsts;
using Edocs.Demo.Application.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Edocs.Demo.Application.Services.Pages.DemoPages
{
    [BindProperties]
    public class BSBPlanDepKeyWordViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        public BSBPlanDepKeyWordViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(DemoConstants.JsonWebApi); } }
        public IDictionary<int, BspPlanDepPermitsModel> DicBSPPlanModel
        { get; set; }
        [BindProperty]
        [Display(Name = "KeyWords:")]
        public string SearchText
        { get; set; }
        public async Task<IActionResult> OnGetAsync(string SearchText)
        {
            if (!(string.IsNullOrWhiteSpace(SearchText)))
            {
                //if (string.IsNullOrWhiteSpace(BSPPlanModel.ConstCompany))
                //    BSPPlanModel.ConstCompany = "NA";
                //if (string.IsNullOrWhiteSpace(BSPPlanModel.GoCode))
                //    BSPPlanModel.GoCode = "NA";
                //if (string.IsNullOrWhiteSpace(BSPPlanModel.OwnerLot))
                //    BSPPlanModel.OwnerLot = "NA";
                 DicBSPPlanModel = DemoApis.DemoInstance.GetBSBPlanDepPermitsBySearchTxt(WebApiUrl,DemoConstants.BSBPlanDepSearchTextController,SearchText).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            return Page();
        }
 
}
}
