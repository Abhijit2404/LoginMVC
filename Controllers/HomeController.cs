using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using LoginMVC.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LoginMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var accessToken = HttpContext.Session.GetString("Token");
            IEnumerable<Hospital> hospitals = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.BaseAddress = new Uri("https://localhost:5001");
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
    }
}
