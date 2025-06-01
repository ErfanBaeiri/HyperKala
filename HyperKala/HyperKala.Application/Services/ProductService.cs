using HyperKala.Application.Extensions;
using HyperKala.Application.Interfaces;
using HyperKala.Application.Statics;
using HyperKala.Domain.Entities.ProductEntity;
using HyperKala.Domain.Interfaces;
using HyperKala.Domain.ViewModels.Admin.Products;
using Microsoft.AspNetCore.Http;
using Shop.Application.Extentions;
using SixLabors.ImageSharp.Processing;

namespace HyperKala.Application.Services
{
    public class ProductService : IProductService
    {
        #region DI
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        #endregion

        #region product - admin

        #region product categories
        public async Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryViewModel createCategory, IFormFile image)
        {
            if (await _productRepository.CheckUrlNameCategories(createCategory.UrlName)) return CreateProductCategoryResult.IsExistUrlName;

            var newCategory = new ProductCategory
            {
                UrlName = createCategory.UrlName,
                Title = createCategory.Title,
                ParentId = null,
                IsDelete = false
            };
            if (image != null && image.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                image.AddImageToServer(imageName, PathTools.CategoryOrginServer, 150, 150, PathTools.CategoryThumbServer);

                newCategory.ImageName = imageName;
            }

            await _productRepository.AddProductCtaegory(newCategory);
            await _productRepository.SaveChangesAsync();

            return CreateProductCategoryResult.Success;
        }

        public async Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryViewModel editProductCategory, IFormFile image)
        {
            var productcategory = await _productRepository.GetProductCategoryById(editProductCategory.ProductCategoryId);

            if (productcategory == null) return EditProductCategoryResult.NotFound;

            if (await _productRepository.CheckUrlNameCategories(editProductCategory.UrlName, editProductCategory.ProductCategoryId)) return EditProductCategoryResult.IsExistUrlName;

            productcategory.UrlName = editProductCategory.UrlName;
            productcategory.Title = editProductCategory.Title;

            if (image != null && image.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                image.AddImageToServer(imageName, PathTools.CategoryOrginServer, 150, 150, PathTools.CategoryThumbServer, productcategory.ImageName);

                productcategory.ImageName = imageName;
            }

            _productRepository.UpdateProductCtaegory(productcategory);

            await _productRepository.SaveChangesAsync();

            return EditProductCategoryResult.Success;
        }

        public async Task<FilterProductCategoriesViewModel?> FilterProductCategories(FilterProductCategoriesViewModel filter)
        {
            return await _productRepository.FilterProductCategories(filter);
        }

        public async Task<EditProductCategoryViewModel?> GetEditProductCategory(long categoryId)
        {
            var productcategory = await _productRepository.GetProductCategoryById(categoryId);

            if (productcategory != null)
            {
                return new EditProductCategoryViewModel
                {
                    ImageName = productcategory.ImageName,
                    ProductCategoryId = productcategory.Id,
                    Title = productcategory.Title,
                    UrlName = productcategory.UrlName
                };
            }

            return null;

        }
        #endregion

        #endregion
    }

}

