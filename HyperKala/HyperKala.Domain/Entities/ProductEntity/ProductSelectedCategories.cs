using HyperKala.Domain.Entities.BaseEntities;

namespace HyperKala.Domain.Entities.ProductEntity
{
    public class ProductSelectedCategories : BaseEntity
    {
        #region properties
        public long ProductId { get; set; }
        public long ProductCategoryId { get; set; }
        #endregion

        #region relations
        public Product Product { get; set; }
        public ProductCategory ProductCategory { get; set; }
        #endregion
    }
}
