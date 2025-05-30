using HyperKala.Domain.Entities.Account;
using HyperKala.Domain.Entities.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace HyperKala.Domain.Entities.Wallet
{
    public class UserWallet : BaseEntity
    {
        #region properties

        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long UserId { get; set; }

        [Display(Name = "نوع تراکنش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public WalletType WalletType { get; set; }

        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Amount { get; set; }

        [Display(Name = "شرح")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public required string Description { get; set; }

        [Display(Name = "پرداخت شده / نشده")]
        public bool IsPay { get; set; }
        #endregion

        #region relations
        public  User User { get; set; }
        #endregion

    }

    public enum WalletType
    {

        [Display(Name = "واریز")]
        Variz = 1,
        [Display(Name = "برداشت")]
        Bardasht = 2
    }
}
