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
    [Authorize]
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

            // try to refresh token
            try
            {
                var newToken = await _authService.RefreshAccessTokenAsync();
                if (!string.IsNullOrEmpty(newToken))
                {
                    // update token claims
                    var identity = new ClaimsIdentity(user.Claims.Where(c => c.Type != "Token"), "Cookies");
                    identity.AddClaim(new Claim("Token", newToken));

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync("Cookies", principal);

                    // logging for debugging
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\tRefreshed");
                }
            }
            catch
            {
                await HttpContext.SignOutAsync();
                context.Result = new RedirectResult("/Login");
                return;
            }

            await next();
        }
    }
}
