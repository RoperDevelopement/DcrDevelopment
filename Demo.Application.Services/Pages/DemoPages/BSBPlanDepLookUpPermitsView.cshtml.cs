using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Edocs.Demo.Application.Services.ApiConsts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Edocs.Demo.Application.Services.ApiConsts;
using Edocs.Demo.Application.Services.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Edocs.Demo.Application.Services.Pages.DemoPages
{
    [BindProperties]
    public class BSBPlanDepLookUpPermitsViewModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        public BSBPlanDepLookUpPermitsViewModel(IConfiguration config, IHttpClientFactory factoryClient)
        {
            configuration = config;
            clientFactory = factoryClient;
        }
        private Uri WebApiUrl
        { get { return configuration.GetValue<Uri>(DemoConstants.JsonWebApi); } }
        public IDictionary<int,BspPlanDepPermitsModel> DicBSPPlanModel
        { get; set; }
 [BindProperty]
public BspPlanDepPermitsModel BSPPlanModel
        { get; set; }

        [BindProperty]
        [Display(Name = "Address Starts with:")]
        public bool AddressStartsWith
        { get; set; }
         
        public async Task<IActionResult> OnGetAsync(BspPlanDepPermitsModel BSPPlanModel, bool AddressStartsWith)
        {
            var qString = Request.QueryString;
            if ((BSPPlanModel.PermitNum !=0) || !(string.IsNullOrWhiteSpace(BSPPlanModel.Address)))
            {
                if (string.IsNullOrWhiteSpace(BSPPlanModel.ConstCompany))
                    BSPPlanModel.ConstCompany = "NA";
                if (string.IsNullOrWhiteSpace(BSPPlanModel.GoCode))
                    BSPPlanModel.GoCode = "NA";
                if (string.IsNullOrWhiteSpace(BSPPlanModel.OwnerLot))
                    BSPPlanModel.OwnerLot = "NA";
                if (string.IsNullOrWhiteSpace(BSPPlanModel.Address))
                    BSPPlanModel.Address = "NA";
                
                DicBSPPlanModel = DemoApis.DemoInstance.GetBSBPlanDepPermits(WebApiUrl, "BSBPlanningDepPermits", BSPPlanModel, AddressStartsWith).ConfigureAwait(false).GetAwaiter().GetResult();
            }
           
            return Page();
        }
    }
}
