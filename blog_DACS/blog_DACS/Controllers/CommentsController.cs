using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using blog_DACS.Models;
using Microsoft.Extensions.Hosting;

namespace blog_DACS.Controllers
{
    public class CommentsController : Controller
    {
        private readonly BlogcanhannContext _context;

        public CommentsController(BlogcanhannContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index(long id)
        {
            var comments = await _context.Comments
          .Where(c => c.IdPost == id)
          .Include(c => c.IdPostNavigation)
          .Include(c => c.IdUserNavigation)
          .ToListAsync();

            // Lấy thông tin của bài viết
            var post = await _context.Posts.FindAsync(id);

            // Chuyển đến view Index với danh sách comment và thông tin của bài viết
            return View(comments);
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.IdPostNavigation)
                .Include(c => c.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult CreateComment()
        {
            ViewData["IdPost"] = new SelectList(_context.Posts, "IdPost", "IdPost");
            ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "IdUser");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment([Bind("ContentComment,CreatedAt,ParentComment,IdPost,IdUser")] Comment comment)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            // Lấy IdUser của người dùng hiện tại
            var userId = GetLoggedInUserId();

            // Tạo một comment mới với ParentComment = 1


            comment.IdUser = userId;

            comment.CreatedAt = DateTime.Now;
            comment.ParentComment = null;// Truyền giá trị 1 cho thuộc tính ParentComment
            

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Sau khi tạo comment thành công, bạn có thể chuyển hướng người dùng đến trang chi tiết bài viết hoặc trang chính của blog
            return RedirectToAction("Index", "Comments", new { id = comment.IdPost }); // Đây là một ví dụ, bạn cần điều chỉnh tùy theo cấu trúc của ứng dụng của bạn
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RepComment([Bind("ContentComment,CreatedAt,ParentComment,IdPost,IdUser")] Comment comment)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            // Lấy IdUser của người dùng hiện tại
            var userId = GetLoggedInUserId();

            // Tạo một comment mới với ParentComment = 1


            comment.IdUser = userId;

            comment.CreatedAt = DateTime.Now;
            comment.ParentComment = 1;// Truyền giá trị 1 cho thuộc tính ParentComment


            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Sau khi tạo comment thành công, bạn có thể chuyển hướng người dùng đến trang chi tiết bài viết hoặc trang chính của blog
            return RedirectToAction("Index", "Comments", new { id = comment.IdPost }); // Đây là một ví dụ, bạn cần điều chỉnh tùy theo cấu trúc của ứng dụng của bạn
        }
        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["IdPost"] = new SelectList(_context.Posts, "IdPost", "IdPost", comment.IdPost);
            ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "IdUser", comment.IdUser);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ContentComment,CreatedAt,ParentComment,IdPost,IdUser")] Comment comment)
        {
            if (id != comment.IdUser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.IdUser))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPost"] = new SelectList(_context.Posts, "IdPost", "IdPost", comment.IdPost);
            ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "IdUser", comment.IdUser);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.IdPostNavigation)
                .Include(c => c.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComments(long idPost, string contentComment)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            // Lấy IdUser của người dùng hiện tại
            var userId = GetLoggedInUserId();

            // Tạo một comment mới với ParentComment = 1
            var comment = new Comment
            {
                IdPost = idPost,
                IdUser = userId,
                ContentComment = contentComment,
                CreatedAt = DateTime.Now,
                ParentComment = null // Truyền giá trị 1 cho thuộc tính ParentComment
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Sau khi tạo comment thành công, bạn có thể chuyển hướng người dùng đến trang chi tiết bài viết hoặc trang chính của blog
            return RedirectToAction("Index", "Comments", new { id = idPost }); // Đây là một ví dụ, bạn cần điều chỉnh tùy theo cấu trúc của ứng dụng của bạn
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
        private bool CommentExists(long id)
        {
            return _context.Comments.Any(e => e.IdUser == id);
        }
    }
}
