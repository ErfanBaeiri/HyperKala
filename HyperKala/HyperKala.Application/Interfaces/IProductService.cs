using HyperKala.Domain.Entities.ProductEntity;
using HyperKala.Domain.ViewModels.Admin.Products;
using Microsoft.AspNetCore.Http;

namespace HyperKala.Application.Interfaces
{
    public interface IProductService
    {
        #region product-admin
        Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryViewModel createCategory, IFormFile image);
        Task<EditProductCategoryViewModel?> GetEditProductCategory(long categoryId);
        Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryViewModel editProductCategory, IFormFile image);
        Task<FilterProductCategoriesViewModel?> FilterProductCategories(FilterProductCategoriesViewModel filter);
        Task<FilterProductsViewModel> FilterProducts(FilterProductsViewModel filter);
        Task<CreateProductResult> CreateProduct(CreateProductViewModel createProduct, IFormFile imageProduct);
        Task<List<ProductCategory>> GetAllProductCategories();

        Task<EditProductViewModel> GetEditProduct(long productId);
        Task<EditProductResult> EditProduct(EditProductViewModel editProduct);
        Task<bool> DeleteProduct(long productId);
        Task<bool> RecoverProduct(long productId);
        Task<bool> AddProductGallery(long productId, List<IFormFile> images);
        Task<List<ProductGalleries>> GetAllProductGalleries(long productId);
        Task DeleteImage(long galleryId);
        #endregion
    }
}
