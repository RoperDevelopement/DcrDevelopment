using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Saml2.Authentication.Core.Authentication;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using EDocs.Nyp.LabReqs.AppServices.Models;

namespace EDocs.Nyp.LabReqs.AppServices.Services
{
    public class NypWebAppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUserModel>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        
        public NypWebAppClaimsPrincipalFactory(
         UserManager<ApplicationUserModel> userManager,
         IOptions<IdentityOptions> optionsAccessor,
         IHttpContextAccessor arghttpContextAccessor)
         : base(userManager, optionsAccessor)
        {
            httpContextAccessor = arghttpContextAccessor;
        }

        private HttpContext Context => httpContextAccessor.HttpContext;
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUserModel user)
        {
            var signInManager =
                (SignInManager<ApplicationUserModel>)Context.RequestServices.GetService(
                    typeof(SignInManager<ApplicationUserModel>));

            var claims = new List<Claim>();
            var authenticationSchemes = await signInManager.GetExternalAuthenticationSchemesAsync();
            foreach (var scheme in authenticationSchemes)
            {
                var authenticateResult =   Context.AuthenticateAsync(scheme.Name).ConfigureAwait(true).GetAwaiter().GetResult();
                if (!authenticateResult.Succeeded)
                {
                    continue;
                }

                var sessionIndex = authenticateResult.Principal.Claims.First(c => c.Type == Saml2ClaimTypes.SessionIndex);
                var saml2Subject = authenticateResult.Principal.Claims.First(c => c.Type == Saml2ClaimTypes.Subject);
                claims.Add(sessionIndex);
                claims.Add(saml2Subject);
            }

            var claimsIdentity = base.GenerateClaimsAsync(user).GetAwaiter().GetResult();
            claimsIdentity.AddClaims(claims); //Add external claims to cookie. The SessionIndex and Subject are required for SLO
            return claimsIdentity;
        }
    }
}
