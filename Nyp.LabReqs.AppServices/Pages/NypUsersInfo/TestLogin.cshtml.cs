using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;
using Microsoft.AspNetCore.Http;
using EDocs.Nyp.LabReqs.AppService.Logging;
using EDocs.Nyp.LabReqs.AppServices.Identity;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.Schemas;
using ITfoxtec.Identity.Saml2.MvcCore;
using Microsoft.IdentityModel.Tokens.Saml2;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;
using EDocs.Nyp.LabReqs.AppServices.Models;
using System.Diagnostics;
namespace EDocs.Nyp.LabReqs.AppServices
{
    public class TestLoginModel : PageModel
    {
        const string relayStateReturnUrl = "ReturnUrl";
        private readonly Saml2Configuration config;
        private readonly IConfiguration configuration;
        private readonly Settings settings;

        public TestLoginModel(IOptions<Saml2Configuration> configAccessor, IConfiguration configdd, IOptions<Settings> settingsAccessor)
        {
            config = configAccessor.Value;

            configuration = configdd;
            settings = settingsAccessor.Value;
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            var serviceProviderRealm = "https://fedtest.nyp.org/idp/startSSO.ping?PartnerSpId=azurewebsites.net";

            var binding = new Saml2PostBinding();
            binding.RelayState = $"RPID={Uri.EscapeDataString(serviceProviderRealm)}";

            var config = new Saml2Configuration();

            config.Issuer = "https://edocsnyplabreqsdev.azurewebsites.net/LoginView";
            config.SingleSignOnDestination = new Uri("https://fedtest.nyp.org/idp/startSSO.ping?PartnerSpId=azurewebsites.net");
            //    config.SigningCertificate = config1.SigningCertificate;
            config.SignatureAlgorithm = Saml2SecurityAlgorithms.RsaSha256Signature;

            var appliesToAddress = "https://edocsnyplabreqsdev.azurewebsites.net/LoginView";

            var response = new Saml2AuthnResponse(config);
            response.Status = Saml2StatusCodes.Success;

            var claimsIdentity = new ClaimsIdentity(CreateClaims());

            response.NameId = new Saml2NameIdentifier(claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).Single(), NameIdentifierFormats.Persistent);
            response.ClaimsIdentity = claimsIdentity;
             

            return binding.Bind(response).ToActionResult();
        }
        public void OnPost()
        {

        }
        private IEnumerable<Claim> CreateClaims()
        {
            yield return new Claim(ClaimTypes.NameIdentifier, "Cwid");
            yield return new Claim(ClaimTypes.Email, "some-user@nyp.org");
            yield return new Claim(ClaimTypes.Name, "firstName");
            yield return new Claim(ClaimTypes.Name, "lastName");
        }
    }
}