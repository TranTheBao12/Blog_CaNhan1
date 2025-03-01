using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_DACS.Areas.Admin.Controllers
{
    public class Home1Controller : Controller
    {
        [Area("Admin")]
        [Authorize()]
        public IActionResult Index()
        {
            return View();
        }
    }
}
