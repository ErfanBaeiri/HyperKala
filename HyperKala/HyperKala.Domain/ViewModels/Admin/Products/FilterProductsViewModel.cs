using HyperKala.Domain.Entities.ProductEntity;
using HyperKala.Domain.ViewModels.Paging;
using System.ComponentModel.DataAnnotations;

namespace HyperKala.Domain.ViewModels.Admin.Products
{
    public class FilterProductsViewModel : BasePaging
    {
        public string ProductName { get; set; }
        public string FilterByCategory { get; set; }
        public List<Product> Products { get; set; }
        public ProductState ProductState { get; set; }
        public ProductOrder ProductOrder { get; set; }

        #region methods
        public FilterProductsViewModel SetProducts(List<Product> products)
        {
            this.Products = products;
            return this;
        }

        public FilterProductsViewModel SetPaging(BasePaging paging)
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
    public enum ProductState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "فعال")]
        IsActice,
        [Display(Name = "حذف شده")]
        Delete
    }
    public enum ProductOrder
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "جدیدترین ها")]

        ProductNewss,
        [Display(Name = "گران ترین ها")]

        ProductExp,
        [Display(Name = "ارزان ترین ها")]

        ProductInExpensive
    }
}
