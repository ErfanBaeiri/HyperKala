using HyperKala.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HyperKala.Web.ViewComponents
{
    #region  Layout Header
    public class SiteHeaderViewComponent : ViewComponent
    {
        #region DI
        private readonly IUserService _userService;
        public SiteHeaderViewComponent(IUserService userService)
        {
            _userService = userService;
        }
        #endregion


        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.User = await _userService.GetUserByPhoneNumberAsync(phoneNumber: User.Identity.Name);
            }
            return View("SiteHeader");
        }
    }
    #endregion


    #region Layout Footer
    public class SiteFooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SiteFooter");
        }
    }
    #endregion


}
