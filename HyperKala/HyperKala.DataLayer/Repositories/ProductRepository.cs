using HyperKala.DataLayer.Context;
using HyperKala.Domain.Entities.ProductEntity;
using HyperKala.Domain.Interfaces;
using HyperKala.Domain.ViewModels.Admin.Products;
using HyperKala.Domain.ViewModels.Paging;
using Microsoft.EntityFrameworkCore;

namespace HyperKala.DataLayer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        #region DI
        private readonly HyperKalaDbContext _context;
        public ProductRepository(HyperKalaDbContext context)
        {
            _context = context;
        }
        #endregion

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        #region Admin - Product 

        
        #region product-categories
        public async Task<bool> CheckUrlNameCategories(string urlName)
        {
            return await _context.ProductCategories.AsQueryable()
                .AnyAsync(c => c.UrlName == urlName);
        }

        public async Task AddProductCtaegory(ProductCategory productCategory)
        {
            await _context.ProductCategories.AddAsync(productCategory);
        }

        public async Task<ProductCategory?> GetProductCategoryById(long id)
        {
            return await _context.ProductCategories.AsQueryable()
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> CheckUrlNameCategories(string urlName, long CategoryId)
        {
            return await _context.ProductCategories.AsQueryable()
                .AnyAsync(c => c.UrlName == urlName && c.Id != CategoryId);
        }

        public void UpdateProductCtaegory(ProductCategory category)
        {
            _context.Update(category);
        }

        public async Task<FilterProductCategoriesViewModel?> FilterProductCategories(FilterProductCategoriesViewModel filter)
        {
            var query = _context.ProductCategories.AsQueryable();

            #region filter
            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(c => EF.Functions.Like(c.Title, $"%{filter.Title}%"));
            }
            #endregion

            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowAfterAndBefore);
            var allData = await query.Paging(pager).ToListAsync();
            #endregion

            return filter.SetPaging(pager).SetProductCategories(allData);
        }
        #endregion
        #endregion
    }
}
