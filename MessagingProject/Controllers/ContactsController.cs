using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        public IActionResult ContactLists()
        {
            return View();
        }
        [Route("Contacts/SingleList/{id?}")]
        public IActionResult SingleList()
        {
            
            return View();
        }
    }
}
