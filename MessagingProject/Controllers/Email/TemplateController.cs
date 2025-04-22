using MessagingProject.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers.Email
{
    [Authorize]

    public class TemplateController:BaseController
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;


        public TemplateController(IAuthService auth, IUserService userService, IEmailService emailService) : base(auth)
        {
            _userService = userService;
            _emailService = emailService;
        }

        
        
    }
}
