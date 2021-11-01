using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LoginMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LoginMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private IHttpClientFactory httpClientFactory;
        private IHttpContextAccessor _httpContext;
        public LoginController(IHttpClientFactory clientFactory, IHttpContextAccessor contextAccessor, ILogger<LoginController> logger)
        {
            _logger = logger;
            httpClientFactory = clientFactory;
            _httpContext = contextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoginUser(UserInfo model)
        {
            using (var client = new HttpClient())
            {
                StringContent sc = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync(Urls.LoginUrl, sc))
                {
                    string token = await response.Content.ReadAsStringAsync();
                    
                    if (token == "Invalid Credentials" || token == "Invalid")
                    {
                        TempData["Message"] = "Incorrect Email Or Password";
                        return RedirectToAction("Index","Login");
                    }

                    HttpContext.Session.SetString("Token", token);

                    var stream = token;
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(stream);                  
                    var tokenS = handler.ReadToken(token) as JwtSecurityToken;

                    string hsptlid = tokenS.Claims.First(claim => claim.Type == "HospitalId").Value;
                    string userr = tokenS.Claims.First(claim => claim.Type == "User").Value;
                    string role = tokenS.Claims.First(claim => claim.Type == "Role").Value;
                    HttpContext.Session.SetString("HospitalId", hsptlid);
                    HttpContext.Session.SetString("User", userr);
                    HttpContext.Session.SetString("Role", role);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,userr),
                        new Claim(ClaimTypes.Role,role)
                    };
                    var claimIdentity = new ClaimsIdentity(claims, "Login");
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                        IsPersistent = true,
                    };

                    await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));
                }
                return RedirectToAction("Index","Home");
            }
        }
        public async Task<IActionResult> Logout()
        {
            await _httpContext.HttpContext.SignOutAsync();
            HttpContext.Session.Remove("Token");
            HttpContext.Response.Cookies.Delete("Phynd.Session");
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Login");
            
        }
    }
}