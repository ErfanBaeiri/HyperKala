using Microsoft.AspNetCore.Mvc;

namespace HyperKala.Web.Controllers;

public class HomeController : BaseController
{

    public IActionResult Index()
    {
     
        return View();
    }

}
