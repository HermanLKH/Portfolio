using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Areas.Admin.Controllers
{
    // just divide the pages into different segments logically to organize the application
    [Area("Admin")]
    // actually restrict access of users by role
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        [Route("admin/[controller]/[action]")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
