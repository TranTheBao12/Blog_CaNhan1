using blog_DACS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using blog_DACS.Models;
namespace blog_DACS.Controllers;
    public class SearchController : Controller
{

    private readonly BlogcanhannContext _context;

    public SearchController(BlogcanhannContext context)
    {
        _context = context;
    }

    public IActionResult Index(string keyword)
    {
        // Nếu từ khóa trống, trả về trang tìm kiếm trống
        if (string.IsNullOrEmpty(keyword))
        {
            return View();
        }

        // Tìm kiếm các bài viết có tiêu đề chứa từ khóa
        var posts = _context.Posts
            .Where(p => p.Title.Contains(keyword))
            .ToList();


        return View(posts);
    }
    public IActionResult Indexuser(string keyword)
    {
        // Nếu từ khóa trống, trả về trang tìm kiếm trống
        if (string.IsNullOrEmpty(keyword))
        {
            return View();
        }

        // Tìm kiếm các bài viết có tiêu đề chứa từ khóa
        var user = _context.Users
            .Where(p => p.FullName.Contains(keyword))
            .ToList();


        return View(user);
    }
    public IActionResult postAdmin(string keyword)
    {
        // Nếu từ khóa trống, trả về trang tìm kiếm trống
        if (string.IsNullOrEmpty(keyword))
        {
            return View();
        }

        // Tìm kiếm các bài viết có tiêu đề chứa từ khóa
        var posts = _context.Posts
            .Where(p => p.Title.Contains(keyword))
            .ToList();


        return View(posts);
    }
    public IActionResult UserAdmin(string keyword)
    {
        // Nếu từ khóa trống, trả về trang tìm kiếm trống
        if (string.IsNullOrEmpty(keyword))
        {
            return View();
        }

        // Tìm kiếm các bài viết có tiêu đề chứa từ khóa
        var user = _context.Users
            .Where(p => p.FullName.Contains(keyword))
            .ToList();


        return View(user);
    }
}
