using MessagingProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using MessagingProject.Abstractions;
using System.Net;
using System.Resources;

namespace MessagingProject.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ResourceManager _resourceManager;

        public AuthController(IAuthService auth, IUserService userService)
        {
            _authService = auth;
            _userService = userService;
            _resourceManager = new ResourceManager("MessagingProject.Resources.Resource", typeof(AuthController).Assembly);
        }
        [Route("/Login")]
        public IActionResult Index()
        {
            
            ViewData["Login"] = _resourceManager.GetString("Вход");
            return View();
        }
        
        [HttpGet]
        [Route("/Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.Response.Cookies.Delete(".AspNetCore.Antiforgery");

                return RedirectToAction("Index", "Auth"); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        
        [HttpPost]
        public async Task<IActionResult> LoginUser([FromForm] LoginViewModel model)
        {
            try
            {
                var authResponse = await _authService.AuthenticateUserAsync(model);

                await _authService.SignInUserAsync(authResponse, model);

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
