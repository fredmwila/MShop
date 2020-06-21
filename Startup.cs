using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using MShop.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using AspNetCoreCultureRouteParameter;

namespace MShop
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
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                //options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();

            //MySql
            services.AddTransient<MySqlDB>(_ => new MySqlDB("server=localhost; database=mshopdb; uid=root; pwd=hunter"));

            //localization
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                 {
                    new CultureInfo("en-NZ"),
                    new CultureInfo("en-GB"),
                    new CultureInfo("en-AU"),
                    new CultureInfo("en-US"),
                    new CultureInfo("de"),
                    new CultureInfo("fr-fr"),
                    new CultureInfo("es"),
                    new CultureInfo("ru"),
                    new CultureInfo("ja-jp"),
                    new CultureInfo("zh-cn"),
                    
                };
                            options.DefaultRequestCulture = new RequestCulture("en-NZ");
                            options.SupportedCultures = supportedCultures;
                            options.SupportedUICultures = supportedCultures;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            localizationOptions.RequestCultureProviders.Insert(0, new RouteValueRequestCultureProvider() { Options = localizationOptions });
            app.UseRequestLocalization(localizationOptions);


            //app.UseMvc();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapRazorPages();

                //    endpoints.MapControllerRoute(
                //       name: "LocalizedDefault",
                //       pattern: "{controller}/{action}/{id?}",
                //       defaults: new { controller = "Home", action = "Index" });
                //    //constraints: new { culture = new CultureConstraint(defaultCulture: "en", pattern: "[a-z]{2}") });

                    endpoints.MapControllerRoute(
                         name: "default",
                         pattern: "{culture}/{controller}/{action}/{id?}",
                         defaults: new { controller = "Home", action = "Index", culture="en-nz" });
            });
        

        }
    }
}
