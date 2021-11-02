using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using LoginMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LoginMVC.Controllers
{
    [Authorize(Roles = "1,3")]
    public class UserController : Controller
    {
        string BaseUrl = Urls.AuthUrl;
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(UserCreate user)
        {
            var accessToken = HttpContext.Session.GetString("Token");

            using(var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(BaseUrl);
                var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8,"application/json");
                var postjob = client.PostAsync("users",stringContent);

                var result = postjob.Result;
                if(result.IsSuccessStatusCode){
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        public ActionResult Delete(int Id)
        {
            var accessToken = HttpContext.Session.GetString("Token");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                client.BaseAddress = new Uri(BaseUrl);

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
            var accessToken = HttpContext.Session.GetString("Token");
            IEnumerable<User> obj = null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(BaseUrl);

            var apicall = client.GetAsync("users/func?Search=" + search);

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