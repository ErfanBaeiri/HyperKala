using Microsoft.AspNetCore.Mvc;

namespace HyperKala.Web.wwwroot.admin.js
{
    public class custom : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
