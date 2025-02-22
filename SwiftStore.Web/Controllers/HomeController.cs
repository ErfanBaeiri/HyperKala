using Microsoft.AspNetCore.Mvc;

namespace SwiftStore.Web.Controllers
{
    public class HomeController : Controller
    {
      
        public IActionResult Index()
        {
            return View();
        }

     
    }
}