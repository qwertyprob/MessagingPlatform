using Microsoft.AspNetCore.Mvc;

namespace MessagingProject.Controllers
{
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
