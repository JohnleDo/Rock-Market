using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockMarketPlace.Models
{
    public class RockMarketPlaceDbContext : DbContext
    {
        public RockMarketPlaceDbContext(DbContextOptions<RockMarketPlaceDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Account { get; set; }
    }
}
