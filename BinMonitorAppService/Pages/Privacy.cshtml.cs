using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BinMonitorAppService.Models;
using BinMonitorAppService.Constants;
using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.Net.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.ApiClasses;
using Microsoft.AspNetCore.Http;
using BinMonitorAppService.Logging;

namespace BinMonitorAppService.Pages
{
    public class PrivacyModel : PageModel
    {
        public async Task OnGet()
        {
            if (!(User.Identity.IsAuthenticated))
            {
             Redirect("/BinUsers/LoginView");
            }
            ViewData["CWID"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyCwid).ConfigureAwait(false);
            if (ViewData["CWID"] == null)
                Redirect("/BinUsers/LoginView");
        //    ViewData["UserProfile"] = await GetSessionVariables.SessionVarInstance.GetSessionVariable(HttpContext.Session, GetSessionVariables.SessionKeyUserProfile);
        }
    }
}