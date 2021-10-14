using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LoginMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginMVC.Controllers
{
    public class UserController : Controller
    {

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserCreate user)
        {
                using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                var postjob = client.PostAsJsonAsync<UserCreate>("users",user);
                postjob.Wait();

                var result = postjob.Result;
                if(result.IsSuccessStatusCode){
                    return RedirectToAction("Index");
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

                var deleteTask = client.DeleteAsync("users?Id=" + Id.ToString());

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index","Home");
        }

       public IActionResult Index(string search)
        {
            IEnumerable<User> obj = null;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:5001");

            var apicall = client.GetAsync("users/func?Search=" + search);
            //apicall.Wait();

            var read = apicall.Result;
            if(read.IsSuccessStatusCode)
            {
                var result = read.Content.ReadAsAsync<IList<User>>();
                result.Wait();
                obj = result.Result;

            }
            return View(obj);
        }
    }
}