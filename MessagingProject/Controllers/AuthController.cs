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
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService auth, IUserService userService)
        {
            _authService = auth;
            _userService = userService;
        }
        [Route("/Login")]
        public IActionResult Index()
        {
            
            return View();
        }
        
        [HttpGet]
        [Route("/Logout")]
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
            try
            {
                var claims = User.Claims;
                var token = claims.FirstOrDefault(c => c.Type == "Token")?.Value;
                var userInfo = _userService.GetProfileInfo(token);

                if (userInfo.IsCompletedSuccessfully)
                {
                    return Ok(userInfo.Result);
                }

                return BadRequest("User info not found.");
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
