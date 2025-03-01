using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using blog_DACS.Models;

namespace blog_DACS.Controllers
{
    public class QLPostsController : Controller
    {
        private readonly BlogcanhannContext _context;

        public QLPostsController(BlogcanhannContext context)
        {
            _context = context;
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

        // GET: QLPosts
        public async Task<IActionResult> Index()

        {
            if (await IsUser())
            {

                return RedirectToAction("AccessDenied", "Default");
            }
            else
            {
                var blogcanhannContext = _context.Posts.Include(p => p.IdUserNavigation);
                return View(await blogcanhannContext.ToListAsync());
            }
        }

        // GET: QLPosts/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (await IsUser())
            {

                return RedirectToAction("AccessDenied", "Default");
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                var post = await _context.Posts
                    .Include(p => p.IdUserNavigation)
                    .FirstOrDefaultAsync(m => m.IdPost == id);
                if (post == null)
                {
                    return NotFound();
                }

                return View(post);
            }
        }

        // GET: QLPosts/Create
        public async Task<IActionResult> Create()

        {
            if (await IsUser())
            {

                return RedirectToAction("AccessDenied", "Default");
            }
            else
            {
                ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "IdUser");
                return View();
            }
        }

        // POST: QLPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPost,Title,ContentPost,CreatedAt,ImagePost,LastAccessedAt,ExistenceStatus,IsPublic,IdUser")] Post post, IFormFile image)
        {
            if (await IsUser())
            {

                return RedirectToAction("AccessDenied", "Default");
            }
            else
            {
              
                long currentUserId = GetLoggedInUserId();
                if (image != null && image.Length > 0)
                {
                    post.ImagePost = await SaveImage(image);
                }
                post.IdPost = _context.Posts.Max(p => p.IdPost) + 1;
                post.IdUser = currentUserId;
                post.CreatedAt = DateTime.Now;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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

        // GET: QLPosts/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (await IsUser())
            {

                return RedirectToAction("AccessDenied", "Default");
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                var post = await _context.Posts.FindAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "IdUser", post.IdUser);
                return View(post);
            }
        }
  

        // POST: QLPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("IdPost,Title,ContentPost,CreatedAt,ImagePost,LastAccessedAt,ExistenceStatus,IsPublic,IdUser")] Post post)
        {
            if (await IsUser())
            {

                return RedirectToAction("AccessDenied", "Default");
            }
            else
            {
                if (id != post.IdPost)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(post);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PostExists(post.IdPost))
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
                ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "IdUser", post.IdUser);
                return View(post);
            }
        }

        // GET: QLPosts/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.IdPost == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: QLPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (await IsUser())
            {

                return RedirectToAction("AccessDenied", "Default");
            }
            else
            {
                var post = await _context.Posts.FindAsync(id);
                if (post != null)
                {
                    _context.Posts.Remove(post);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        private bool PostExists(long id)
        {
            return _context.Posts.Any(e => e.IdPost == id);
        }

    }
}
