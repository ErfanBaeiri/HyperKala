using HyperKala.Domain.Entities.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace HyperKala.Domain.Entities.ProductEntity
{
    public class Product : BaseEntity
    {
        #region properties

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Name { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(800, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ShortDescription { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }

        [Display(Name = "تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ProductImageName { get; set; }

        [Display(Name = "فعال / غیر فعال")]
        public bool IsActive { get; set; }
        #endregion

        #region relations
        public ICollection<ProductGalleries> ProductGalleries { get; set; }
        public ICollection<ProductSelectedCategories> ProductSelectedCategories { get; set; }
        public ICollection<ProductFeature> ProductFeatures { get; set; }
        #endregion
    }
}
