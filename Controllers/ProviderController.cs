using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginMVC.Controllers
{
    [Authorize(Roles = "1,2")]
    public class ProviderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}