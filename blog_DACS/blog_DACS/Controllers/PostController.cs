using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using blog_DACS.Models;
using blog_DACS.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;

namespace blog_DACS.Controllers
{
    public class PostController : Controller
    {
        private readonly BlogcanhannContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public PostController(BlogcanhannContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        private async Task<bool> IsUser()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var username = session.GetInt32("user");
            return await _context.Users.AnyAsync(h => h.IdUser == username);
        }
        public async Task<IActionResult> Index()
        {
            if (await IsUser())
            {
                return RedirectToAction("AccessDenied", "Admin");
            }
            var posts = await _context.Posts
     .OrderByDescending(p => p.CreatedAt)
     .Select(p => new PostViewModel
     {
         Title = p.Title,
         ImagePost = p.ImagePost,
         IdUser = p.IdUser,
         CreatedAt = p.CreatedAt,
         // Lấy tên đầy đủ của người đăng từ bảng User
         FullName = _context.Users
             .Where(u => u.IdUser == p.IdUser)
             .Select(u => u.FullName)
             .FirstOrDefault()
     })
     .ToListAsync();

            return View(posts);
        }
        public async Task<IActionResult> Details(long id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            // Lấy tên đầy đủ của người đăng từ bảng User
            var userFullName = await _context.Users
                                            .Where(u => u.IdUser == post.IdUser)
                                            .Select(u => u.FullName)
                                            .FirstOrDefaultAsync();

            // Truyền tên đầy đủ của người đăng vào ViewBag hoặc ViewModel để sử dụng trong View
            ViewBag.UserFullName = userFullName;

            return View(post);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post, IFormFile image)
        {
            
                // Thiết lập các giá trị mặc định cho bài viết
                post.CreatedAt = DateTime.Now;
                post.LastAccessedAt = DateTime.Now;
                post.ExistenceStatus = "Active"; // Hoặc bất kỳ giá trị mặc định nào khác bạn muốn

                // Lưu hình ảnh nếu có
                if (image != null && image.Length > 0)
                {
                    post.ImagePost = await SaveImage(image);
                }
            post.IdPost = _context.Posts.Max(p => p.IdPost) + 1;
               
                _context.Add(post);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            

            // Nếu ModelState không hợp lệ, hiển thị lại form với dữ liệu đã nhập
           
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var imageName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(image.FileName)}";
            var imagePath = Path.Combine("wwwroot/images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return imageName;
        }
    }

}