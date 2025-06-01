using HyperKala.Domain.Entities.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace HyperKala.Domain.Entities.ProductEntity
{
    public class ProductFeature : BaseEntity
    {
        #region properties
        public long ProductId { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FeatuerTitle { get; set; }

        [Display(Name = "مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FeatureValue { get; set; }
        #endregion

        #region relations
        public Product Product { get; set; }
        #endregion
    }
}
