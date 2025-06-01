using HyperKala.Domain.Entities.BaseEntities;
using HyperKala.Domain.Entities.Wallet;
using System.ComponentModel.DataAnnotations;

namespace HyperKala.Domain.Entities.Account
{
    public class User : BaseEntity
    {
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public required string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public required string LastName { get; set; }

        [Display(Name = "شماره تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public required string PhoneNumber { get; set; }

        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public required string Password { get; set; }

        [Display(Name = "آواتار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public required string Avatar { get; set; }

        [Display(Name = "مسدود شده / نشده")]
        public bool IsBlocked { get; set; }

        [Display(Name = "کد احرازهویت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public required string MobileActiveCode { get; set; }

        [Display(Name = "تایید شده / نشده")]
        public bool IsMobileActive { get; set; }

        [Display(Name = "جنسیت")]
        public UserGender UserGender { get; set; }

        #region relations
        public  ICollection<UserWallet> UserWallets { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }

        #endregion
    }
    public enum UserGender
    {
        [Display(Name = "مرد")]
        Male,
        [Display(Name = "زن")]
        Female,
        [Display(Name = "نامشخص")]
        Unknown
    }
}

