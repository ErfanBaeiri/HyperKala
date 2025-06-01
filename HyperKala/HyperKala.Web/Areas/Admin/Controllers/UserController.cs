using HyperKala.Application.Interfaces;
using HyperKala.Domain.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace HyperKala.Web.Areas.Admin.Controllers
{
    public class UserController : AdminBaseController
    {
        #region constractor
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region filter user
        public async Task<IActionResult> FilterUser(FilterUserViewModel filter)
        {
            var data = await _userService.FilterUsers(filter);

            return View(data);
        }
        #endregion

        #region edit user
        [HttpGet]
        public async Task<IActionResult> EditUser(long userId) //id
        {
            var data = await _userService.GetEditUserFromAdmin(userId);

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserFromAdmin editUser)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.EditUserFromAdmin(editUser);

                switch (result)
                {
                    case EditUserFromAdminResult.NotFound:
                        TempData[WarningMessage] = "کاربری با مشخصات وارد شده یافت نشد";
                        break;
                    case EditUserFromAdminResult.Success:
                        TempData[SuccessMessage] = "ویراش کاربر با موفقیت انجام شد";
                        return RedirectToAction("FilterUser");
                }
            }

            return View(editUser);
        }

        #endregion

    }
}
