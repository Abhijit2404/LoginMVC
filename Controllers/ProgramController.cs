using Microsoft.AspNetCore.Mvc;

namespace LoginMVC.Controllers
{
    public class ProgramController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}