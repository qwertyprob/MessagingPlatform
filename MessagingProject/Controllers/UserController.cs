using MessagingProject.Abstractions;
using MessagingProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;

namespace MessagingProject.Controllers
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
        public  async Task<IActionResult> GetClaimsResult()
        {
            try
            {
                var token = _userService.GetToken().Result;
                var user = _userService.GetProfileInfo(token).Result;

                return Ok(user);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet]
        [Route("/User")]
        public async Task<IActionResult> User()
        {
            try
            {
                var token = _userService.GetToken().Result;
                var user = _userService.GetProfileInfo(token).Result;

                string list = user.FullName;


                return Json(new Dictionary<string, string> { {"FullName", list } });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
