using ZayraMarket.DataLayer.Entities.Common;

namespace ZayraMarket.DataLayer.Repository
{
    public interface IGenericRepository<TEntity> : IAsyncDisposable where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetQuery();
        Task AddEntity(TEntity entity);
        Task<TEntity?> GetEntityId(long entityId);
        void EditEntity(TEntity entity);
        void DeleteEntity(TEntity entity);
        Task DeleteEntity(long id);
        void Deletepermanet(TEntity entity);
        Task Deletepermanet(long id);


    }
}
