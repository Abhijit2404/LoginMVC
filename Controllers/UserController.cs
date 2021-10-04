using System.Threading.Tasks;
using LoginMVC.Models;
using LoginMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoginMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> List()
        {
            var listusers = await _userService.GetUserList();
            return View(listusers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreate user)
        {
            bool success = await _userService.SaveUser(user);
            if(success)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Index","Home");
        }
    }
}