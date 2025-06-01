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
        #endregion
    }
}
