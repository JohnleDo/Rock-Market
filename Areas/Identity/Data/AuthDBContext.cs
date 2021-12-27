using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rock_Market.Areas.Identity.Data;
using Rock_Market.Models;

namespace Rock_Market.Data
{
    // This class will reference our Rock_Market class and override our database with whatever in that class
    public class AuthDBContext : IdentityDbContext<Rock_MarketUser>
    {
        public AuthDBContext(DbContextOptions<AuthDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Rock_Market.Models.StoreViewModel> Store { get; set; }
        public DbSet<Rock_Market.Models.ImageModel> Image { get; set; }
    }
}
