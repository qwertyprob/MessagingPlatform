using MessagingProject.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MessagingProject.Controllers
{
    
    public class BaseController : Controller
    {
        private readonly IAuthService _authService;

        public BaseController(IAuthService authService)
        {
            _authService = authService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                context.Result = new RedirectResult("/Login");
                return;
            }

            var tokenClaim = user.Claims.FirstOrDefault(x => x.Type == "Token");

            if (tokenClaim == null || string.IsNullOrEmpty(tokenClaim.Value))
            {
                await HttpContext.SignOutAsync();
                context.Result = new RedirectResult("/Login");
                return;
            }

            //kind a token check with timer
            //var expiryClaim = user.Claims.FirstOrDefault(c => c.Type == "Token");
            //if (expiryClaim != null && DateTime.TryParse(expiryClaim.Value, out var expiry))
            //{
            //    if (expiry > DateTime.UtcNow.AddMinutes(2))
            //    {
            //        await next(); 
            //        return;
            //    }
            //}
            //try to refresh
            try
            {
                var newToken = await _authService.RefreshAccessTokenAsync();
                if (!string.IsNullOrEmpty(newToken))
                {
                    var claims = user.Claims.Where(c => c.Type != "Token").ToList();
                    claims.Add(new Claim("Token", newToken));

                    var identity = new ClaimsIdentity(claims, "Cookies");
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync("Cookies", principal);

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\tRefreshed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token refresh failed: {ex.Message}");
                await HttpContext.SignOutAsync();
                context.Result = new RedirectResult("/Login");
                return;
            }

            await next();
        }

    }
}
