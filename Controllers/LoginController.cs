using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
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
        private IHttpClientFactory _httpClient;
        private IHttpContextAccessor _httpContext;

        public LoginController(IHttpContextAccessor httpContext,
                               IHttpClientFactory httpClient,
                               ILogger<LoginController> logger)
        {
            _httpContext = httpContext;
            _httpClient = httpClient;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> LoginUser(UserInfo user)
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var message = new HttpRequestMessage();
            message.Content = httpContent;
            message.Method = HttpMethod.Post;
            message.RequestUri = new Uri("https://localhost:5001/authenticate");

            HttpClient client = _httpClient.CreateClient();
            HttpResponseMessage responseMessage = await client.SendAsync(message);
            var result = await responseMessage.Content.ReadAsStringAsync();

            Token resultfinal = JsonConvert.DeserializeObject<Token>(result);
            // HttpContext.Session.SetString("token", resultfinal.ToString());

            //Cookies
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Authentication,resultfinal.tokenString),
            };
            var claimIdentity = new ClaimsIdentity(claims, "LoginUser");

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true, 
                ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                IsPersistent = true,
            };
            await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));
            return RedirectToAction("Index", "Home");
            
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _httpContext.HttpContext.SignOutAsync();
            return RedirectToAction("Index","Login");
        }
    }
}