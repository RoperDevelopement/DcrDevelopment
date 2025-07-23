using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data;
using System.Data.SqlClient;


namespace EDocs.Nyp.LabReqs.AppService.Logging
{
    public class WriteErrorMessage: PageModel,IError
    {
       public async Task ErrorMessage(string eMessage,HttpContext context)
        {
             context.Response.WriteAsync("<h1 class=\"text-danger\"> Error.</h1>").ConfigureAwait(false).GetAwaiter().GetResult();
              context.Response.WriteAsync("<h2 class=\"text-danger\">An error occurred while processing your request.</h2>").GetAwaiter().GetResult();

              context.Response.WriteAsync(eMessage).GetAwaiter().GetResult();
        }
    }
}
