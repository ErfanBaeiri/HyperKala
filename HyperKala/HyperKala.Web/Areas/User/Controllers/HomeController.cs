using Microsoft.AspNetCore.Mvc;

namespace HyperKala.Web.Areas.User.Controllers
{
    public class HomeController : UserBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
