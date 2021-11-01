using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LoginMVC.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;

namespace LoginMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var accessToken = HttpContext.Session.GetString("Token");
            IEnumerable<Hospital> hospitals = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri(Urls.AuthUrl);
                var response = client.GetAsync("hospital");
                response.Wait();
                
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var read = result.Content.ReadAsAsync<IList<Hospital>>();
                    read.Wait();
                    hospitals = read.Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty,"Server Error Occured");
                }

                return View(hospitals);
            }
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
