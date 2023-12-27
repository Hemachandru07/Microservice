using Microsoft.EntityFrameworkCore;
using MS.Services.RewardAPI.Models;

namespace MS.Services.RewardAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Rewards> Rewards { get; set; }
       
    }
}
