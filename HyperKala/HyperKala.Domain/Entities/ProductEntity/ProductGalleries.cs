using HyperKala.Domain.Entities.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace HyperKala.Domain.Entities.ProductEntity
{
    public class ProductGalleries : BaseEntity
    {
        #region properties
        public long ProductId { get; set; }

        [Display(Name = "تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }
        #endregion

        #region relations
        public Product Product { get; set; }
        #endregion
    }
}
