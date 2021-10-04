using System;
using System.Net.Http;
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
        public IActionResult Create(UserCreate user)
        {
            // bool success = await _userService.SaveUser(user);
            // if(success)
            // {
            //     return RedirectToAction("Index","Home");
            // }
            // return RedirectToAction("Index","Home");
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:5001");
                    var postjob = client.PostAsJsonAsync<UserCreate>("users",user);
                    postjob.Wait();

                    var result = postjob.Result;
                    if(result.IsSuccessStatusCode){
                        return RedirectToAction("List");
                    }
                }
            ModelState.AddModelError(string.Empty,"Server Error");
            return View(user);
        }

        public ActionResult Delete(int Id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");

                var deleteTask = client.DeleteAsync("users/Id?Id=" + Id.ToString());

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("List");
                }
            }
        return RedirectToAction("Index","Home");
        }
    }
}