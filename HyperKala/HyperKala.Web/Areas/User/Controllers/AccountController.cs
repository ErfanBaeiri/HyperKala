using HyperKala.Application.Extensions;
using HyperKala.Application.Interfaces;
using HyperKala.Domain.ViewModels.Account;
using HyperKala.Domain.ViewModels.Wallet;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ZarinpalSandbox;

namespace HyperKala.Web.Areas.User.Controllers
{
    public class AccountController : UserBaseController
    {
        #region DI
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IConfiguration _configuration;
        public AccountController(IUserService userService, IWalletService walletService, IConfiguration configuration)
        {
            _userService = userService;
            _walletService = walletService;
            _configuration = configuration;
        }
        #endregion

        #region edit user profile
        [HttpGet("edit-user-profile")]
        public async Task<IActionResult> EditUserProfile()
        {
            var user = await _userService.FillEditUserProfile(User.GetUserId());
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost("edit-user-profile"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserProfile(EditUserProfileViewModel editUserProfile, IFormFile userAvatar)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.EditUserProfile(User.GetUserId(), userAvatar, editUserProfile);
                switch (result)
                {
                    case EditUserProfileResult.NotFound:
                        TempData[WarningMessage] = "کاربری با مشخصات وارد شده یافت نشد";
                        break;
                    case EditUserProfileResult.Success:
                        TempData[SuccessMessage] = "عملیات ویرایش حساب کاربری با موفقیت انجام شد";
                        return RedirectToAction("EditUserProfile");
                }
            }
            return View(editUserProfile);
        }
        #endregion

        #region change password
        [HttpGet("change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("change-password"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePassword)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ChangePasswordInUserPanelAsync(User.GetUserId(), changePassword);
                switch (result)
                {
                    case ChangePasswordResult.NotFound:
                        TempData[WarningMessage] = "کاربری با مشخصات وارد شده یافت نشد";
                        break;
                    case ChangePasswordResult.PasswordEqual:
                        TempData[InfoMessage] = "لطفا از کلمه عبور جدیدی استفاده کنید";
                        ModelState.AddModelError("NewPassword", "لطفا از کلمه عبور جدیدی استفاده کنید");
                        break;
                    case ChangePasswordResult.Success:
                        TempData[SuccessMessage] = "کلمه ی عبور شما با موفقیت تغیر یافت";
                        await HttpContext.SignOutAsync();
                        return RedirectToAction("Login", "Account", new { area = "" });
                }
            }
            return View(changePassword);
        }
        #endregion

        #region charge wallet
        [HttpGet("charge-wallet")]
        public async Task<IActionResult> ChargeWallet()
        {
            //ToDo:Show Transaction List For User
            return View();
        }
        [HttpPost("charge-wallet"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ChargeWallet(ChargeWalletViewModel chargeWallet)
        {
            if (ModelState.IsValid)
            {
                var walletId = await _walletService.ChargeWallet(User.GetUserId(), chargeWallet, $"شارژ به مبلغ {chargeWallet.Amount}");
  
                var payment = new Payment(chargeWallet.Amount);
                var url = _configuration.GetSection("DefaultUrl")["Host"] + "/user/online-payment/" + walletId;

                try
                {
                    var result = await payment.PaymentRequest("شارژ کیف پول", url);
                    if (result.Status == 100)
                    {
                        return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + result.Authority);
                    }
                    else
                    {
                        TempData[ErrorMessage] = "مشکلی در پرداخت به وجود آمده است، لطفا مجددا امتحان کنید";
                    }
                }
                catch (Newtonsoft.Json.JsonReaderException ex)
                {
                    // احتمالاً پاسخ HTML دریافت شده
                    TempData[ErrorMessage] = "پاسخ نامعتبر از درگاه پرداخت دریافت شد. لطفا کمی بعد دوباره تلاش کنید.";

                    // برای لاگ دقیق‌تر
                    Console.WriteLine("خطای JSON: " + ex.Message);
                }
                catch (Exception ex)
                {
                    TempData[ErrorMessage] = "خطای غیرمنتظره‌ای رخ داده است.";
                    Console.WriteLine("خطای کلی: " + ex.Message);
                }

            }

            return View(chargeWallet);
        }
        #endregion

        #region online payment
        [HttpGet("online-payment/{id}")]
        public async Task<IActionResult> OnlinePayment(long id)
        {
            if (HttpContext.Request.Query["Status"] != "" && HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" && HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];
                var wallet = await _walletService.GetUserWalletById(id);
                if (wallet != null)
                {
                    var payment = new Payment(wallet.Amount);
                    var result = payment.Verification(authority).Result;

                    if (result.Status == 100)
                    {
                        ViewBag.RefId = result.RefId;
                        ViewBag.Success = true;
                        await _walletService.UpdateWalletForCharge(wallet);
                    }
                    return View();
                }
                return NotFound();
            }
            return View();
        }
        #endregion

        #region user wallet
        public async Task<IActionResult> UserWallet(FilterWalletViewModel filter)
        {
            filter.UserId = User.GetUserId();

            return View(await _walletService.FilterWallets(filter));
        }
        #endregion
    }
}

















