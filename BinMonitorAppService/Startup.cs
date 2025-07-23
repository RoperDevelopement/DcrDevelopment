using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ITfoxtec.Identity.Saml2.MvcCore.Configuration;
using ITfoxtec.Identity.Saml2.Util;
using ITfoxtec.Identity.Saml2.MvcCore;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;
using BinMonitorAppService.Logging;
using BinMonitor.BinInterfaces;
using BinMonitorAppService.ApiClasses;
using Microsoft.AspNetCore.Authentication;
namespace BinMonitorAppService
{
    public class Startup
    {
        public IWebHostEnvironment WebEnv { get; private set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebEnv = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Saml2Configuration>(Configuration.GetSection("Saml2"));
            string metadataFile = $"{WebEnv.WebRootPath}//{Configuration["Saml2:MetadatCertFolder"]}{Configuration["Saml2:IdPMetadata"]}";
            services.Configure<Saml2Configuration>(saml2Configuration =>
            {
              
                saml2Configuration.AllowedAudienceUris.Add(saml2Configuration.Issuer);
                saml2Configuration.AllowedAudienceUris.Add(Configuration["Saml2:AllowedAudienceUrisAppliesTo"]);

                var entityDescriptor = new EntityDescriptor();
                entityDescriptor.ReadIdPSsoDescriptorFromFile(metadataFile);
                if (entityDescriptor.IdPSsoDescriptor != null)
                {
                    saml2Configuration.AllowedIssuer = entityDescriptor.EntityId;

                    saml2Configuration.SingleSignOnDestination = entityDescriptor.IdPSsoDescriptor.SingleSignOnServices.First().Location;

                    saml2Configuration.SingleLogoutDestination = entityDescriptor.IdPSsoDescriptor.SingleLogoutServices.First().Location;
                    saml2Configuration.SignatureValidationCertificates.AddRange(entityDescriptor.IdPSsoDescriptor.SigningCertificates);
                }
                else
                {
                    throw new Exception("IdPSsoDescriptor not loaded from metadata.");
                }

            });

            services.AddSaml2();

            services.AddRazorPages();
           
            services.Add(new ServiceDescriptor(typeof(ILog), new AuditLogs()));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });
            services.AddHttpContextAccessor();
            services.AddSingleton<IEmailSettings>(Configuration.GetSection("EmailSettings").Get<EmailConfiguration>());
            services.AddTransient<IEmailService, EmailService>();
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession(options => {
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30);

                options.Cookie.Name = "Sessions";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseInMemorySession(configure: s => s.IdleTimeout = TimeSpan.FromMinutes(30));

            if (env.IsDevelopment())
            {
                  app.UseDeveloperExceptionPage();
             //   app.UseExceptionHandler("/Error");
                 app.UseHsts();

            }
            else
            {
             // app.UseDeveloperExceptionPage();
                      app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            //    app.UseDeveloperExceptionPage();
            }
             
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSaml2();
            
            app.UseAuthorization();
            app.UseCookiePolicy();
             app.UseEndpoints(endpoints =>
             {
                 endpoints.MapRazorPages();
            });

        }
    }
}
