using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using blog_DACS.Models;
using blog_DACS.ViewModels;
using System.Drawing.Printing;
using Microsoft.Extensions.Hosting;

namespace blog_DACS.Controllers
{
    public class PostsController : Controller
    {
        private readonly BlogcanhannContext _context;

        public PostsController(BlogcanhannContext context)
        {
            _context = context;
        }
 
        // GET: Posts
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            var blogcanhannContext = _context.Posts.Include(p => p.IdUserNavigation);

            var totalCount = await blogcanhannContext.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var paginatedPosts = await blogcanhannContext.Skip((page - 1) * pageSize)
                                                        .Take(pageSize)
                                                        .ToListAsync();
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            ViewData["PageNumber"] = page;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalPages"] = totalPages;

            return View(paginatedPosts);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            long currentUserId = GetLoggedInUserId();
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
            var isFavoritedByUser = await _context.FavoritePosts.AnyAsync(fp => fp.IdUser == currentUserId && fp.IdPost == id);

            var likeCount = await CountLikesForPostByUser(post.IdPost);
            var isLikedByUser = await IsPostLikedByUser(post.IdPost, currentUserId);
            var isSharedByUser = await _context.Shares.AnyAsync(s => s.IdUser == currentUserId && s.IdPost == id);
            ViewBag.IsFavoritedByUser = isFavoritedByUser;
            ViewBag.IsLikedByUser = isLikedByUser;
            ViewBag.ShareCount = await _context.Shares.CountAsync(s => s.IdPost == id);
            // Đưa số lượt like vào ViewBag hoặc ViewModel để sử dụng trong view
            ViewBag.LikeCount = likeCount;
            ViewBag.IsSharedByUser = isSharedByUser;
            var postViewModel = new PostViewModel
            {
                IdPost = post.IdPost,
                Title = post.Title,
                ContentPost = post.ContentPost,
                CreatedAt = post.CreatedAt,
                ImagePost = post.ImagePost,
                FullName = post.IdUserNavigation.FullName // Giả sử FullName là trường chứa tên người dùng trong IdUserNavigation
            };
         
            return View(postViewModel);
        }
        //  truy van vao bang like xem doi tuong thich bai viet chua
        private async Task<bool> IsPostLikedByUser(long postId, long userId)
        {
            var existingLike = await _context.Likeds.FirstOrDefaultAsync(l => l.IdUser == userId && l.IdPost == postId);
            return existingLike != null;
        }
        private async Task<int> CountLikesForPostByUser(long postId)
        {
            // Lấy số lượt like cho bài viết dựa trên IdUser của IdPost
            var likeCount = await _context.Likeds.CountAsync(l => l.IdPost == postId);
            return likeCount;
        }
        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["IdUser"] = new SelectList(_context.Users, "IdUser", "IdUser");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPost,Title,ContentPost,CreatedAt,ImagePost,LastAccessedAt,ExistenceStatus,IsPublic,IdUser")] Post post, IFormFile image)
        {
            //if (ModelState.IsValid)
            //{
            //    if (image != null && image.Length > 0)
            //    {
            //        post.ImagePost = await SaveImage(image);
            //    }
            //    _context.Add(post);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            long currentUserId = GetLoggedInUserId();
            if (image != null && image.Length > 0)
            {
                post.ImagePost = await SaveImage(image);
            }
            post.CreatedAt = DateTime.Now;
            post.IdPost = _context.Posts.Max(p => p.IdPost) + 1;
            post.IdUser = currentUserId;
            _context.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //  return View(post);
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

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(long? id)
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

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("IdPost,Title,ContentPost,CreatedAt,ImagePost,LastAccessedAt,ExistenceStatus,IsPublic,IdUser")] Post post)
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

        // GET: Posts/Delete/5
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

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            long currentUserId = GetLoggedInUserId();

            // Tìm bài viết cần xóa
            var post = await _context.Posts.FindAsync(id);

            // Kiểm tra xem bài viết tồn tại và có thuộc về người dùng đăng nhập không
            if (post != null && post.IdUser == currentUserId)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Posts", new { id = id });
            }
            else
            {
                // Nếu người dùng không có quyền xóa bài viết, đặt thông báo lỗi vào TempData
                TempData["ErrorMessage"] = "Bạn không có quyền xóa bài viết này.";
            }

            return RedirectToAction("Details","Posts", new { id = id });
        }

        private bool PostExists(long id)
        {
            return _context.Posts.Any(e => e.IdPost == id);
        }
        [HttpPost]
        public async Task<IActionResult> ToggleLike(long idPost)
        {
            // Lấy IdUser của người dùng hiện tại từ session hoặc từ hệ thống xác thực
            long currentUserId = GetLoggedInUserId();

            // Kiểm tra xem mối quan hệ like đã tồn tại chưa
            var existingLike = await _context.Likeds.FindAsync(currentUserId, idPost);
            if (existingLike != null)
            {
                // Nếu đã tồn tại, xóa mối quan hệ like.
                _context.Likeds.Remove(existingLike);
                await _context.SaveChangesAsync();

                return Json(new { isLiked = false });
            }
            else
            {
                // Nếu chưa tồn tại, tạo một mối quan hệ mới Like.
                var like = new Liked
                {
                    IdUser = currentUserId,  // IdUser là Id của người dùng đăng nhập
                    IdPost = idPost,         // IdPost là Id của bài post được yêu thích
                    CreatedAt = DateTime.Now // Hoặc thời gian khác nếu cần
                };

                _context.Likeds.Add(like);
                await _context.SaveChangesAsync();

                return Json(new { isLiked = true });
            }
        }
        [HttpPost]
        public async Task<IActionResult> LikePost(long idPost)
        {
            // Lấy IdUser của người dùng hiện tại từ session hoặc từ hệ thống xác thực
            long currentUserId = GetLoggedInUserId();

            // Kiểm tra xem mối quan hệ like đã tồn tại chưa
            var existingLike = await _context.Likeds.FindAsync(currentUserId, idPost);
            if (existingLike != null)
            {
                TempData["Message"] = "Bài viết đã được yêu thích trước đó";
                // Nếu đã tồn tại, không cần thêm vào lại.
                return RedirectToAction("Index", "Posts", new { id = idPost });
            }

            // Tạo một mối quan hệ mới Like.
            var like = new Liked
            {
                IdUser = currentUserId,  // IdUser là Id của người dùng đăng nhập
                IdPost = idPost,         // IdPost là Id của bài post được yêu thích
                CreatedAt = DateTime.Now // Hoặc thời gian khác nếu cần
            };

            _context.Likeds.Add(like);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Posts", new { id = idPost });
        }

        [HttpPost]
        public async Task<IActionResult> UnlikePost(long idPost)
        {
            // Lấy IdUser của người dùng hiện tại từ session hoặc từ hệ thống xác thực
            long currentUserId = GetLoggedInUserId();

            // Tìm mối quan hệ like để xóa
            var likeToRemove = await _context.Likeds.FirstOrDefaultAsync(l => l.IdUser == currentUserId && l.IdPost == idPost);
            if (likeToRemove != null)
            {
                _context.Likeds.Remove(likeToRemove);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Posts", new { id = idPost });
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
