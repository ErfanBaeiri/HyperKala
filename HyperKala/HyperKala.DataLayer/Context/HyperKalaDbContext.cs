using HyperKala.Domain.Entities.Account;
using HyperKala.Domain.Entities.Wallet;
using Microsoft.EntityFrameworkCore;

namespace HyperKala.DataLayer.Context
{
    public class HyperKalaDbContext : DbContext
    {
        public HyperKalaDbContext(DbContextOptions<HyperKalaDbContext> options) : base(options)
        {
                
        }


        public DbSet<User> Users { get; set; }

        public DbSet<UserWallet> UserWallets { get; set; }
    }
}
