using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RadPdf;
namespace EditPDF
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddSession();
        }
        //https://dev.to/eliotjones/reading-a-pdf-in-c-on-net-core-43ef
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
            //// Create middleware settings (add UseService = false to run RAD PDF without the System Service)
            //RadPdfCoreMiddlewareSettings settings = new RadPdfCoreMiddlewareSettings() { ConnectionString = "Server=.;Database=RadPdf;Trusted_Connection=Yes;", LicenseKey = "DEMO" };

            //// Add RAD PDF's middleware to app
            //app.UseRadPdf(settings);
        }
    }
}
    