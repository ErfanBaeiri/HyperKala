using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ZayraMarket.DataLayer.Context;
using ZayraMarket.DataLayer.Entities.Common;

namespace ZayraMarket.DataLayer.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ZayraMarketDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ZayraMarketDbContext dbContext)
        {
            _context = dbContext;
            this._dbSet = _context.Set<TEntity>();
        }

        public async Task AddEntity(TEntity entity)
        {
            entity.CreateDate = DateTime.Now;
            await _dbSet.AddAsync(entity);
        }

        public void DeleteEntity(TEntity entity)
        {
            entity.IsDelete = true;
            EditEntity(entity);
        }

        public async Task DeleteEntity(long id)
        {
            TEntity? entity = await GetEntityId(id);
            if (entity != null) DeleteEntity(entity);
        }

        public void Deletepermanet(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task Deletepermanet(long id)
        {
            TEntity? entity = await GetEntityId(id);
            if (entity != null)
                Deletepermanet(entity);
        }

        public async ValueTask DisposeAsync()
        {
            if (_context != null)
                await _context.DisposeAsync();
        }

        public void EditEntity(TEntity entity)
        {
            entity.LastUpdateDate = DateTime.Now;
            _dbSet.Update(entity);
        }

        public async Task<TEntity?> GetEntityId(long entityId)
        {
            return await _dbSet.SingleOrDefaultAsync(s => s.Id == entityId);
        }

        public IQueryable<TEntity> GetQuery()
        {
            return _dbSet.AsQueryable();
        }
    }
}
