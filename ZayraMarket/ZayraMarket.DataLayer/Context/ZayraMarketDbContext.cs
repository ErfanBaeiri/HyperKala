using Microsoft.EntityFrameworkCore;
using ZayraMarket.DataLayer.Entities.Account;

namespace ZayraMarket.DataLayer.Context;

public class ZayraMarketDbContext : DbContext
{
    #region dbsets
    public DbSet<User> Users { get; set; }
    #endregion
}
