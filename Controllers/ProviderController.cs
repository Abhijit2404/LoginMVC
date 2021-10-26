using Microsoft.AspNetCore.Mvc;

namespace LoginMVC.Controllers
{
    public class ProviderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}