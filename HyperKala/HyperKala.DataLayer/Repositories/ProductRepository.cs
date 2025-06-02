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

        #region Admin Panel

        #region Product-Category
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

        public async Task<List<ProductCategory>> GetAllProductCategories()
        {
            return await _context.ProductCategories.AsQueryable().Where(c => !c.IsDelete).ToListAsync();
        }

        #endregion

        #region product

        public async Task<FilterProductsViewModel> FilterProducts(FilterProductsViewModel filter)
        {
            var query = _context.Products
                .Include(c => c.ProductSelectedCategories)
                .ThenInclude(c => c.ProductCategory)
                .AsQueryable();

            #region filter
            if (!string.IsNullOrEmpty(filter.ProductName))
            {
                query = query.Where(c => EF.Functions.Like(c.Name, $"%{filter.ProductName}%"));
            }

            if (!string.IsNullOrEmpty(filter.FilterByCategory))
            {
                query = query.Where(c => c.ProductSelectedCategories.Any(s => s.ProductCategory.UrlName == filter.FilterByCategory));
            }

            #endregion

            #region state
            switch (filter.ProductState)
            {
                case ProductState.All:
                    break;
                case ProductState.IsActice:
                    query = query.Where(c => c.IsActive);
                    break;
                case ProductState.Delete:
                    query = query.Where(c => c.IsDelete);
                    break;
            }

            switch (filter.ProductOrder)
            {
                case ProductOrder.All:
                    break;
                case ProductOrder.ProductNewss:
                    query = query.Where(c => c.IsActive).OrderByDescending(c => c.CreateDate);
                    break;
                case ProductOrder.ProductExp:
                    query = query.Where(c => c.IsActive).OrderByDescending(c => c.Price);
                    break;
                case ProductOrder.ProductInExpensive:
                    query = query.Where(c => c.IsActive).OrderBy(c => c.Price);
                    break;
            }
            #endregion

            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowAfterAndBefore);
            var allData = await query.Paging(pager).ToListAsync();
            #endregion

            return filter.SetPaging(pager).SetProducts(allData);
        }

        public async Task AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task RemoveProductSelectedCategories(long productId)
        {
            var allProductSelectedCategories = await _context.ProductSelectedCategories.AsQueryable()
                .Where(c => c.ProductId == productId).ToListAsync();

            if (allProductSelectedCategories.Any())
            {
                _context.ProductSelectedCategories.RemoveRange(allProductSelectedCategories);
            }

        }

        public async Task AddProductSelectedCategories(List<long> productSelectedCategories, long productId)
        {
            if (productSelectedCategories != null && productSelectedCategories.Any())
            {
                var newProductSelectedCategories = new List<ProductSelectedCategories>();

                foreach (var categoryId in productSelectedCategories)
                {
                    newProductSelectedCategories.Add(new ProductSelectedCategories
                    {
                        ProductId = productId,
                        ProductCategoryId = categoryId
                    });
                }

                await _context.ProductSelectedCategories.AddRangeAsync(newProductSelectedCategories);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Product?> GetProductById(long productId)
        {
            return await _context.Products.AsQueryable()
                .SingleOrDefaultAsync(c => c.Id == productId);
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }

        public async Task<List<long>> GetAllProductCategoriesId(long productId)
        {
            return await _context.ProductSelectedCategories.AsQueryable()
                .Where(c => c.ProductId == productId)
                .Select(c => c.ProductCategoryId)
                .ToListAsync();
        }

        public async Task<bool> DeleteProduct(long productId)
        {
            var currentProduct = await _context.Products.AsQueryable()
                .Where(c => c.Id == productId).FirstOrDefaultAsync();

            if (currentProduct != null)
            {
                currentProduct.IsDelete = true;
                _context.Products.Update(currentProduct);
                await _context.SaveChangesAsync();


                return true;
            }

            return false;
        }

        public async Task<bool> RecoverProduct(long productId)
        {
            var currentProduct = await _context.Products.AsQueryable()
               .Where(c => c.Id == productId).FirstOrDefaultAsync();

            if (currentProduct != null)
            {
                currentProduct.IsDelete = false;
                _context.Products.Update(currentProduct);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task AddProductGalleries(List<ProductGalleries> productGalleries)
        {
            await _context.ProductGalleries.AddRangeAsync(productGalleries);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckProduct(long productId)
        {
            return await _context.Products.AsQueryable()
                .AnyAsync(c => c.Id == productId);
        }

        public async Task<List<ProductGalleries>> GetAllProductGalleries(long productId)
        {
            return await _context.ProductGalleries.AsQueryable()
                .Where(c => c.ProductId == productId && !c.IsDelete)
                .ToListAsync();
        }

        public async Task<ProductGalleries> GetProductGalleriesById(long id)
        {
            return await _context.ProductGalleries.AsQueryable()
                //.Where(c => c.Id == id)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task DeleteProductGallery(long id)
        {
            var currentGallery = await _context.ProductGalleries.AsQueryable()
                //.Where(c => c.Id == id)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (currentGallery != null)
            {
                currentGallery.IsDelete = true;

                _context.ProductGalleries.Update(currentGallery);
                await _context.SaveChangesAsync();
            }

        }

        #endregion

        #endregion
    }
}
