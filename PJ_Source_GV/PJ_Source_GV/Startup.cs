using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using PJ_Source_GV.Caption;
using PJ_Source_GV.FunctionSupport;
using SSOLibCore;

namespace PJ_Source_GV
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
            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
            services.AddControllersWithViews(options =>
                {
                    options.EnableEndpointRouting = false; // ✅ Tạm tắt endpoint routing
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, opts => {
                    opts.ResourcesPath = "Resources";
                })
                .AddDataAnnotationsLocalization();
            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Home/ChooseLogin");
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(120); // ✅ sửa ở đây
                });

            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("vi") {
                            NumberFormat = new CultureInfo("en-US").NumberFormat
                        },
                        new CultureInfo("en")
                    };

                    opts.DefaultRequestCulture = new RequestCulture("vi");
                    // Formatting numbers, dates, etc.
                    opts.SupportedCultures = supportedCultures;
                    // UI strings that we have localized.
                    opts.SupportedUICultures = supportedCultures;
                    opts.RequestCultureProviders = new List<IRequestCultureProvider>
                    {
                        new QueryStringRequestCultureProvider(),
                        new CookieRequestCultureProvider()
                    };
                });


            services.AddTransient<ITicketStore, InMemoryTicketStore>();
            services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>,
              ConfigureCookieAuthenticationOptions>();

            services.AddSingleton<IFileProvider>(
            new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            ConstValue.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            ConstValue.PDFViewer = Configuration.GetConnectionString("PDFViewer");
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTI2ODU2QDMxMzkyZTMzMmUzMGVxcVlybE80VGZITkhubXJGRGJrZk96R2JUM2labS9RQUxGaS9IRjlSNzg9");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseSession();
            if (env.IsDevelopment())
            {
                // app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRequestLocalization(app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });



            SSOLibSVCore.HttpHelper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            SSOLibSVCore.ConfigurationHelper.Configure(Configuration);
            //SSO code
            HttpHelper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            ConfigurationHelper.Configure(Configuration);
        }
    }
}
