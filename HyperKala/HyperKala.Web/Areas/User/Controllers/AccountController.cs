using HyperKala.Application.Extensions;
using HyperKala.Application.Interfaces;
using HyperKala.Domain.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace HyperKala.Web.Areas.User.Controllers
{
    public class AccountController : UserBaseController
    {
        #region DI
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
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
    }
}
