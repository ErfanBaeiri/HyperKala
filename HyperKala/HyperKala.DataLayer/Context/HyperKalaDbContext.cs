using HyperKala.Domain.Entities.Account;
using HyperKala.Domain.Entities.ProductEntity;
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


        #region products
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<ProductSelectedCategories> ProductSelectedCategories { get; set; }
        public DbSet<ProductGalleries> ProductGalleries { get; set; }
        #endregion
    }
}
