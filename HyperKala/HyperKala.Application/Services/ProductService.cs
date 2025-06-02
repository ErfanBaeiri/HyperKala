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

        #region Admin

        #region ProductCategory
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

        #region Product
        public async Task<FilterProductsViewModel> FilterProducts(FilterProductsViewModel filter)
        {
            return await _productRepository.FilterProducts(filter);
        }

        public async Task<CreateProductResult> CreateProduct(CreateProductViewModel createProduct, IFormFile imageProduct)
        {
            #region product
            var newProduct = new Product
            {
                Name = createProduct.Name,
                Price = createProduct.Price,
                Description = createProduct.Description,
                ShortDescription = createProduct.ShortDescription,
                IsActive = createProduct.IsActive
            };


            if (imageProduct != null && imageProduct.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(imageProduct.FileName);

                imageProduct.AddImageToServer(imageName, PathTools.ProductOrginServer, 255, 273, PathTools.ProductThumbServer);

                newProduct.ProductImageName = imageName;
            }
            else
            {
                return CreateProductResult.NotImage;
            }


            await _productRepository.AddProduct(newProduct);
            await _productRepository.SaveChangesAsync();
            await _productRepository.AddProductSelectedCategories(createProduct.ProductSelectedCategory, newProduct.Id);

            return CreateProductResult.Success;

            #endregion
        }
        public async Task<List<ProductCategory>> GetAllProductCategories()
        {
            return await _productRepository.GetAllProductCategories();
        }

        public async Task<EditProductViewModel> GetEditProduct(long productId)
        {
            var currentProduct = await _productRepository.GetProductById(productId);

            if (currentProduct != null)
            {
                return new EditProductViewModel
                {
                    Description = currentProduct.Description,
                    IsActive = currentProduct.IsActive,
                    Name = currentProduct.Name,
                    Price = currentProduct.Price,
                    ShortDescription = currentProduct.ShortDescription,
                    ProductImageName = currentProduct.ProductImageName,
                    ProductSelectedCategory = await _productRepository.GetAllProductCategoriesId(productId)
                };
            }

            return null;
        }

        public async Task<EditProductResult> EditProduct(EditProductViewModel editProduct)
        {
            var product = await _productRepository.GetProductById(editProduct.ProductId);
            if (product == null) return EditProductResult.NotFound;
            if (editProduct.ProductSelectedCategory == null) return EditProductResult.NotProductSelectedCategoryHasNull;

            #region edit product
            product.ShortDescription = editProduct.ShortDescription;
            product.Description = editProduct.Description;
            product.IsActive = editProduct.IsActive;
            product.Price = editProduct.Price;
            product.Name = editProduct.Name;


            if (editProduct.ProductImage != null && editProduct.ProductImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(editProduct.ProductImage.FileName);
                editProduct.ProductImage.AddImageToServer(imageName, PathTools.ProductOrginServer, 255, 273, PathTools.ProductThumbServer, product.ProductImageName);

                product.ProductImageName = imageName;
            }
            #endregion
            _productRepository.UpdateProduct(product);

            await _productRepository.RemoveProductSelectedCategories(editProduct.ProductId);
            await _productRepository.AddProductSelectedCategories(editProduct.ProductSelectedCategory, editProduct.ProductId);


            await _productRepository.SaveChangesAsync();

            return EditProductResult.Success;
        }

        public async Task<bool> DeleteProduct(long productId)
        {
            return await _productRepository.DeleteProduct(productId);
        }

        public async Task<bool> RecoverProduct(long productId)
        {
            return await _productRepository.RecoverProduct(productId);
        }
        public async Task<bool> AddProductGallery(long productId, List<IFormFile> images)
        {

            if (!await _productRepository.CheckProduct(productId))
            {
                return false;
            }

            if (images != null && images.Any())
            {
                var productGallery = new List<ProductGalleries>();
                foreach (var image in images)
                {
                    if (image.IsImage())
                    {
                        var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                        image.AddImageToServer(imageName, PathTools.ProductOrginServer, 255, 273, PathTools.ProductThumbServer);


                        productGallery.Add(new ProductGalleries
                        {
                            ImageName = imageName,
                            ProductId = productId
                        });
                    }
                }

                await _productRepository.AddProductGalleries(productGallery);

            }
            return true;
        }
        public async Task<List<ProductGalleries>> GetAllProductGalleries(long productId)
        {
            return await _productRepository.GetAllProductGalleries(productId);
        }

        public async Task DeleteImage(long galleryId)
        {
            var productGallery = await _productRepository.GetProductGalleriesById(galleryId);
            if (productGallery != null)
            {
                UploadImageExtension.DeleteImage(productGallery.ImageName, PathTools.ProductOrginServer, PathTools.ProductThumbServer);
                await _productRepository.DeleteProductGallery(galleryId);
            }
        }
        #endregion

        #endregion
    }

}

