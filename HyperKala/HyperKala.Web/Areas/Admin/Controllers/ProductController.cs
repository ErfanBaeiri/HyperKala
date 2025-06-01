using HyperKala.Application.Interfaces;
using HyperKala.Domain.ViewModels.Admin.Products;
using Microsoft.AspNetCore.Mvc;

namespace HyperKala.Web.Areas.Admin.Controllers
{
    public class ProductController : AdminBaseController
    {
        #region DI
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion


        #region Filter - Products 
        public IActionResult Index()
        {
            return View();
        }
        #endregion


        #region Filter - Product - Categories 
        public async Task<IActionResult> FilterProductCategories(FilterProductCategoriesViewModel filter)
        {
            return View(await _productService.FilterProductCategories(filter));
        }
        #endregion

        #region Create product category
        [HttpGet]
        public IActionResult CreateProductCategory()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductCategory(CreateProductCategoryViewModel createProductCategory, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProductCategory(createProductCategory, image);
                switch (result)
                {
                    case CreateProductCategoryResult.IsExistUrlName:
                        TempData[WarningMessage] = "اسم Url تکراری است";
                        break;
                    case CreateProductCategoryResult.Success:
                        TempData[SuccessMessage] = "دسته بندی با موفقیت ثبت شد";
                        return RedirectToAction("FilterProductCategories");
                }
            }

            return View(createProductCategory);
        }
        #endregion

        #region edit product category
        [HttpGet]
        public async Task<IActionResult> EditProductCategory(long categoryId)
        {
            var data = await _productService.GetEditProductCategory(categoryId);

            if (data == null) return NotFound();

            return View(data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductCategory(EditProductCategoryViewModel editProductCategory, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.EditProductCategory(editProductCategory, image);

                switch (result)
                {
                    case EditProductCategoryResult.IsExistUrlName:
                        TempData[WarningMessage] = "اسم Url تکراری است";
                        break;
                    case EditProductCategoryResult.NotFound:
                        TempData[ErrorMessage] = "دسته بندی با مشخصات وارد شده یافت نشد";
                        break;
                    case EditProductCategoryResult.Success:
                        TempData[SuccessMessage] = "دسته بندی با موفقیت ویرایش شد";
                        return RedirectToAction("FilterProductCategories");
                }
            }

            return View(editProductCategory);
        }
        #endregion

    }
}
