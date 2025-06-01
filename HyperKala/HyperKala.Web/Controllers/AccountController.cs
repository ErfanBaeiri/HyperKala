using GoogleReCaptcha.V3.Interface;
using HyperKala.Application.Extensions;
using HyperKala.Application.Interfaces;
using HyperKala.Domain.ViewModels.Account;
using HyperKala.Domain.ViewModels.Wallet;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HyperKala.Web.Controllers
{
    public class AccountController : BaseController
    {
        #region DI
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly ICaptchaValidator _captchaValidator;
        public AccountController(IUserService userService, ICaptchaValidator captchaValidator, IWalletService walletService)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
            _walletService = walletService;
        }
        #endregion

        #region Register
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel register)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(register.Token))
            {
                TempData[WarningMessage] = "اعتبارسنجی شما با شکست مواجه شد";
                return View(register);
            }


            if (!ModelState.IsValid)
                return View(register);

            var result = await _userService.RegisterUserAsync(register);

            switch (result)
            {
                case RegisterUserResult.MobileExists:
                    TempData[WarningMessage] = "شماره وارد شده تکراری میباشد";
                    break;
                case RegisterUserResult.Success:
                    TempData[SuccessMessage] = "ثبت نام  با موفقیت انجام شد";
                    return RedirectToAction("ActiveAccount", "Account", new { mobile = register.PhoneNumber });

            }

            return View(register);
        }
        #endregion

        #region Login
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("Login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel login)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(login.Token))
            {
                TempData[WarningMessage] = "اعتبارسنجی شما با شکست مواجه شد";
                return View(login);
            }

            if (!ModelState.IsValid)
                return View(login);

            var result = await _userService.LoginUserAsync(login);

            switch (result)
            {
                case LoginUserResult.NotFound:
                    TempData[WarningMessage] = "کاربر مورد نظر یافت نشد";
                    break;
                case LoginUserResult.NotActive:
                    TempData[InfoMessage] = "حساب کاربری فعال نمیباشد";
                    break;
                case LoginUserResult.IsBlocked:
                    TempData[ErrorMessage] = "حساب کاربری مسدود میباشد";
                    break;
                case LoginUserResult.Success:
                    var user = await _userService.GetUserByPhoneNumberAsync(login.PhoneNumber);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,user.PhoneNumber),
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principle = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe
                    };
                    await HttpContext.SignInAsync(principle, properties);
                    TempData[SuccessMessage] = "خوش آمدید";
                    return Redirect("/");

            }
            return View(login);
        }
        #endregion

        #region LogOut
        [HttpGet("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            TempData[InfoMessage] = "شما با موفقیت از حساب کاربری خارج شدید";
            return Redirect("/");
        }

        #endregion

        #region activate account


        [HttpGet("activate-account/{mobile}")]
        public async Task<IActionResult> ActiveAccount(string mobile)
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            var activeAccount = new ActiveAccountViewModel { PhoneNumber = mobile };

            return View(activeAccount);
        }

        [HttpPost("activate-account/{mobile}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ActiveAccount(ActiveAccountViewModel activeAccount)
        {

            #region captcha Validator
            if (!await _captchaValidator.IsCaptchaPassedAsync(activeAccount.Token))
            {
                TempData[ErrorMessage] = "کد کپچای شما معتبر نمیباشد";
                return View(activeAccount);
            }
            #endregion
            if (ModelState.IsValid)
            {
                var result = await _userService.ActivateAccountAsync(activeAccount);
                switch (result)
                {
                    case ActiveAccountResult.Error:
                        TempData[ErrorMessage] = "فعالسازی حساب کاربری با خطا مواجه شد";
                        break;
                    case ActiveAccountResult.NotFound:
                        TempData[WarningMessage] = "کاربر مورد نظر یافت نشد";
                        break;
                    case ActiveAccountResult.WrongCode:
                        TempData[WarningMessage] = "کد وارد شده اشتباه است";
                        break;
                    case ActiveAccountResult.IsBlock:
                        TempData[ErrorMessage] = "دسترسی حساب کاربری شما مسدود میباشد";
                        await HttpContext.SignOutAsync();
                        return Redirect("/");
                    case ActiveAccountResult.Success:
                        TempData[SuccessMessage] = "حساب کاربری شما با موفقیت فعال شد";
                        return RedirectToAction("Login");
                }
            }
            return View(activeAccount);
        }

        #endregion

    }
}
