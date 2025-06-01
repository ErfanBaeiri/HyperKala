using HyperKala.Domain.Entities.ProductEntity;
using HyperKala.Domain.ViewModels.Paging;

namespace HyperKala.Domain.ViewModels.Admin.Products
{
    public class FilterProductCategoriesViewModel : BasePaging
    {
        public string Title { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }


        #region methods
        public FilterProductCategoriesViewModel SetProductCategories(List<ProductCategory> productCategories)
        {
            this.ProductCategories = productCategories;
            return this;
        }

        public FilterProductCategoriesViewModel SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntityCount = paging.AllEntityCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.TakeEntity = paging.TakeEntity;
            this.CountForShowAfterAndBefore = paging.CountForShowAfterAndBefore;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;

            return this;
        }

        #endregion
    }
}
