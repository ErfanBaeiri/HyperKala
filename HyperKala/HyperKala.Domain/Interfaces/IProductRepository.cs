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
        Task<List<ProductCategory>> GetAllProductCategories();

        #endregion
        #region product
        Task<FilterProductsViewModel> FilterProducts(FilterProductsViewModel filter);
        Task AddProduct(Product product);
        Task RemoveProductSelectedCategories(long productId);
        Task AddProductSelectedCategories(List<long> productSelectedCategories, long productId);
        Task<Product?> GetProductById(long productId);
        void UpdateProduct(Product product);
        Task<List<long>> GetAllProductCategoriesId(long productId);
        Task<bool> DeleteProduct(long productId);
        Task<bool> RecoverProduct(long productId);
        Task AddProductGalleries(List<ProductGalleries> productGalleries);
        Task<bool> CheckProduct(long productId);
        Task<List<ProductGalleries>> GetAllProductGalleries(long productId);
        Task<ProductGalleries> GetProductGalleriesById(long id);
        Task DeleteProductGallery(long id);
        #endregion
    }
}
