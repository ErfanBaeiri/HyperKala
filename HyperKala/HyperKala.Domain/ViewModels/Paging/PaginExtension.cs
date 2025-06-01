namespace HyperKala.Domain.ViewModels.Paging
{
    public static class PaginExtension
    {
        public static IQueryable<T> Paging<T>(this IQueryable<T> query, BasePaging basePaging)
        {
            return query.Skip(basePaging.SkipEntity).Take(basePaging.TakeEntity);
        }
    }
}
