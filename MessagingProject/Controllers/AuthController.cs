using FluentValidation;
using FluentValidation.AspNetCore;
using MessagingProject.Abstractions;
using MessagingProject.Models;
using MessagingProject.Validators;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Resources;

namespace MessagingProject.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IValidator<LoginViewModel> _validator;
        public AuthController(IAuthService auth, IUserService userService,IValidator<LoginViewModel> validator)
        {
            _validator = validator;
            _authService = auth;
            _userService = userService;
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

        [Route("/Login")]
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> LoginUser([FromForm] LoginViewModel model)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(model);
                if (validationResult.IsValid)
                {
                    var authResponse = await _authService.AuthenticateUserAsync(model);
                    await _authService.SignInUserAsync(authResponse, model);
                    return Redirect("/");

                }

               validationResult.AddToModelState(this.ModelState);

                return View("Index", model);


            }
            catch (UnauthorizedAccessException)
            {
                ViewData.ModelState.AddModelError("Email", MessagingProject.Resources.ValidateResource.Invalid);
                return View("Index", model);
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
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email)
        {
            try
            {
              
                var authResponse = await _authService.ResetPassword(email);

                return Redirect("/");

            }
            catch (UnauthorizedAccessException)
            {
                ViewBag.ErrorMessage = "Invalid credentials or token.";
                return Redirect("/");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"$\"Internal server error: {ex.Message}\"";
                return Redirect("/");
            }

            
         }

        
    }
}
