using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LoginMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LoginMVC.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoginUser(UserInfo model)
        {
            using (var client = new HttpClient())
            {
                StringContent sc = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("https://localhost:5001/authenticate", sc))
                {
                    string token = await response.Content.ReadAsStringAsync();
                    if (token == "Invalid Credentials")
                    {
                        ViewBag.Message = "Incorrect Email or Password";
                        return RedirectToAction("Index","Login");
                    }
                    HttpContext.Session.SetString("Token", token);
                }

                return RedirectToAction("Index","Home");

            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Token");
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Login");
        }
    }
}