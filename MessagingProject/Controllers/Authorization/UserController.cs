using MessagingProject.Abstractions;
using MessagingProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using System.Globalization;

namespace MessagingProject.Controllers.Authorization
{ 
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
        public async Task<IActionResult> GetClaimsResult()
        {
            var token = _userService.GetToken();
            var user = await _userService.GetProfileInfo(token);

            var userDictionary = new Dictionary<string, string>
        {
            { "Company", user.Company },
            { "Email", user.Email },
            { "FullName", user.FullName },
            { "Password", user.Password },
            { "Language", user.UiLanguage.ToString() },
            { "Token", user.Token }
        };

            return Ok(userDictionary);
        }
        //    [HttpGet]
        //    [Route("GetUserByClaims")]
        //    public async Task<IActionResult> GetClaimsResult()
        //    {
        //        var token = _userService.GetToken();
        //        var user = await _userService.GetProfileInfo(token);

        //        var userList = new List<object>
        //{
        //    new
        //    {
        //        user.Company,
        //        user.Email,
        //        user.FullName,
        //        UiLanguage = user.UiLanguage.ToString(),
        //        user.Password,
        //        user.Token
        //    }
        //};

        //        return Ok(userList);
        //    }




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
