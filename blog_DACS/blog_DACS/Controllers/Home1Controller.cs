using blog_DACS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
namespace blog_DACS.Controllers
{
    public class Home1Controller : Controller
    {
        private readonly BlogcanhannContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public Home1Controller(BlogcanhannContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        private async Task<bool> IsUser()
        {

            var currentUserId = GetLoggedInUserId();
             if (currentUserId != null)
           {
        var currentUser = await _context.Users.FindAsync(currentUserId);
        if (currentUser != null && currentUser.IdRole == 2)
        {
            return true;
        }
    }
    return false;
}
        

        public async Task<IActionResult> Index()
        {   // Kiểm tra xem người dùng đã đăng nhập và có IdRole là 2 hay không
            if (await IsUser())
            {
             
                return RedirectToAction("AccessDenied", "Default");
            }

      
            return View();
        


    }
        private long GetLoggedInUserId()
        {
            // Lấy Id của người dùng từ session
            var userIdString = HttpContext.Session.GetString("userId");

            // Kiểm tra xem userIdString có giá trị không
            if (userIdString != null && long.TryParse(userIdString, out long userId))
            {
                return userId;
            }
            else
            {
                // Trả về một giá trị mặc định hoặc xử lý theo nhu cầu của bạn
                return 0;
            }
        }
    }
}
