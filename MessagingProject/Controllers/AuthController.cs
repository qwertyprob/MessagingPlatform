using MessagingProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using MessagingProject.Abstractions;

namespace MessagingProject.Controllers
{

    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _client;
        private readonly IAuthService _authService;

        public AuthController(IHttpClientFactory client, IAuthService auth)
        {
            _client = client;
            _authService = auth;

        }
        [Route("/Login")]
        public IActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return RedirectToAction("Index", "Auth"); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("/Get")]
        public IActionResult GetClaimsResult()
        {
            using (var client = new HttpClient())
            {
                var claims = User.Claims;
                var token = claims.SingleOrDefault(x => x.Type == "Token").Value;
                client.BaseAddress= new Uri("https://dev.edi.md/ISAuthService/json/GetProfileInfo");
                var response = client.GetAsync(client.BaseAddress + $"?Token={token}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var user = JsonConvert.DeserializeObject<AuthResponseModel>(content);
                    return Ok(user);
                }
                return BadRequest("Invalid token.");
            }
        }
        [HttpPost]
        [Route("/LoginUser")]
        public async Task<IActionResult> LoginUser([FromForm] LoginViewModel model)
        {
            try
            {
                var authResponse = await _authService.AuthenticateUserAsync(model);

                await _authService.SignInUserAsync(authResponse);

                return Redirect("/");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid credentials or token.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [Route("/AuthRecoverPassword")]
        public IActionResult AuthRecoverPassword()
        {
            return View();
        }
    }
}
