using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rock_Market.Areas.Identity.Data;
using Rock_Market.Data;

namespace Rock_Market
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
            /* 
             * This portion of the code already exists in the Areas/Identity/IdentityHostingStartup.cs for some reason:
             * services.AddDbContext<AuthDBContext>(options =>
             *      options.UseSqlServer(
             *           context.Configuration.GetConnectionString("AuthDBContextConnection")));
             *   services.AddDefaultIdentity<Rock_MarketUser>(options => options.SignIn.RequireConfirmedAccount = true)
             *       .AddEntityFrameworkStores<AuthDBContext>();
             *       
             * This means we have to go into that file and comment out that portion or it will cause errors with our websites which won't let it run.
             * Also in this portion we had to copy that code and pasted it in here instead of just leaving it in there. For some reason it would be able
             * to do the IdentityRole function when left in there so we added that here. (.AddRoles<IdentityRole>())
             */
            services.AddDbContext<AuthDBContext>(options => options.UseSqlServer(
            Configuration.GetConnectionString("AuthDBContextConnection")));

            // Made the signIn = false so that a user without a confirmed email account can still login. 
            services.AddDefaultIdentity<Rock_MarketUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AuthDBContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
