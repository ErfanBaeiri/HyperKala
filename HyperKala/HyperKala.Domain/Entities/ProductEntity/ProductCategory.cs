﻿using HyperKala.Domain.Entities.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace HyperKala.Domain.Entities.ProductEntity
{
    public class ProductCategory : BaseEntity
    {
        #region properties
        public long? ParentId { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "عنوان url")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string UrlName { get; set; }

        [Display(Name = "تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }
        #endregion

        #region relations
        public ProductCategory Parent { get; set; }
        public ICollection<ProductSelectedCategories> ProductSelectedCategories { get; set; }
        #endregion
    }
}
