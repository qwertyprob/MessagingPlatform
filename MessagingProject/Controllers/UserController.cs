﻿using MessagingProject.Abstractions;
using MessagingProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using System.Globalization;

namespace MessagingProject.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly HttpClient _httpClient;
        public UserController(IUserService userService, HttpClient httpClient)
        {
            _userService = userService;
            _httpClient = httpClient;


        }
        [HttpGet]
        [Route("/GetUserByClaims")]
        public  Task<IActionResult> GetClaimsResult()
        {
            try
            {
                var token = _userService.GetToken();
                var user = _userService.GetProfileInfo(token).Result;
                
                //В высоком регистре
                var userDictionary = new Dictionary<string, string>
                {
                    { "Company", user.Company },
                    { "Email", user.Email },
                    { "FullName", user.FullName },
                    { "UiLanguage", user.UiLanguage.ToString() },
                    { "Password", user.Password },
                    { "Token", user.Token }
                };

                

                return Task.FromResult<IActionResult>(Ok(userDictionary));

            }
            catch (Exception ex)
            {
                return Task.FromResult<IActionResult>(StatusCode(500, $"Internal server error: {ex.Message}"));
            }
        }


        [AllowAnonymous]
        public IActionResult ChangeLanguage(string language)
        {
            try
            {
                _userService.ChangeUILanguage(language);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            //redirect
            var referer = Request.GetTypedHeaders().Referer?.ToString() ?? "/";
            return Redirect(referer);
        }



    }
}
