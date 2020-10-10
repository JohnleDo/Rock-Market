//using System;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Rock_Market.Areas.Identity.Data;
//using Rock_Market.Data;

//[assembly: HostingStartup(typeof(Rock_Market.Areas.Identity.IdentityHostingStartup))]
//namespace Rock_Market.Areas.Identity
//{
//    public class IdentityHostingStartup : IHostingStartup
//    {
//        public void Configure(IWebHostBuilder builder)
//        {
//            /*
//             * If you're here from the comment left in Startup.cs file, then this is the place where you make changes regarding the ConfigureServices but
//             * in terms of Identity (I think). So far in here the only thing added is:
//             * 1. .AddRoles<IdentityRole>()
//             */
//            builder.ConfigureServices((context, services) => {
//                services.AddDbContext<AuthDBContext>(options =>
//                    options.UseSqlServer(
//                        context.Configuration.GetConnectionString("AuthDBContextConnection")));

//                services.AddDefaultIdentity<Rock_MarketUser>(options => options.SignIn.RequireConfirmedAccount = true)
//                    .AddEntityFrameworkStores<AuthDBContext>();
//            });
//        }
//    }
//}