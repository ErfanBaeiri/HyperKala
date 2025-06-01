using HyperKala.Domain.Entities.ProductEntity;
using HyperKala.Domain.ViewModels.Admin.Products;

namespace HyperKala.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task SaveChangesAsync();

        #region product categories
        Task<bool> CheckUrlNameCategories(string urlName);
        Task<bool> CheckUrlNameCategories(string urlName, long CategoryId);
        Task AddProductCtaegory(ProductCategory productCategory);
        Task<ProductCategory?> GetProductCategoryById(long id);
        void UpdateProductCtaegory(ProductCategory category);
        Task<FilterProductCategoriesViewModel?> FilterProductCategories(FilterProductCategoriesViewModel filter);
        #endregion
    }
}
