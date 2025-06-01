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
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<UserWallet> UserWallets { get; set; }
    }
}
