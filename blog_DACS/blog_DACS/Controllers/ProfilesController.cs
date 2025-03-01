using blog_DACS.Models;
using blog_DACS.View_Models;
using blog_DACS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blog_DACS.Controllers
{
    public class ProfileController : Controller
    {
        private readonly BlogcanhannContext _context;

        public ProfileController(BlogcanhannContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var currentUserEmail = HttpContext.Session.GetString("user");

            if (currentUserEmail == null)
            {
                return RedirectToAction("Login", "Default");
            }

            var currentUser = _context.Users.FirstOrDefault(u => u.Email == currentUserEmail);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Default");
            }

            var sharesByUser = _context.Shares.Where(s => s.IdUser == currentUser.IdUser).ToList();

            var sharedPostIds = sharesByUser.Select(s => s.IdPost).ToList();

            // Thay vì truy vấn trực tiếp các bài viết từ database, chúng ta sẽ tạo view model ProfileViewModel 
            // chứa thông tin cần thiết từ các bài viết và truyền danh sách các ProfileViewModel này vào view
            var sharedPosts = _context.Posts.Where(p => sharedPostIds.Contains(p.IdPost))
                                             .Select(p => new ProfileViewModel
                                             {
                                                 // Thêm các thuộc tính của bài viết mà bạn muốn hiển thị trong ProfileViewModel
                                                 // Ví dụ:
                                                 IdPost = p.IdPost,
                                                 Title = p.Title,
                                                 ContentPost = p.ContentPost,
                                                 // ...

                                             })
                                             .ToList();
            var totalIdFollower = _context.Follows.Count(f => f.IdFollower == currentUser.IdUser);
            var totalIdFollowing = _context.Follows.Count(f => f.IdFollowing == currentUser.IdUser);
            var totalfavoriteuser = _context.FavoriteUsers.Count(f => f.IdFavoriteUser == currentUser.IdUser);
            // Đếm tổng số lượt favorite userƯ
            var totalFavoriteUsers = _context.FavoriteUsers.Count(fu => fu.IdUser == currentUser.IdUser);
            var like = _context.Likeds.Count(fu => fu.IdUser == currentUser.IdUser);
            // Lưu fullname của người dùng vào ViewBag để sử dụng trong view
            ViewBag.like = like;
            ViewBag.FullName = currentUser.FullName;
            ViewBag.TotalIdFollower = totalIdFollower;
            ViewBag.TotalIdFollowing = totalIdFollowing;
            ViewBag.TotalIdFollowing = totalIdFollowing;
            // Lưu tổng số lượt favorite user vào ViewBag để sử dụng trong view
            ViewBag.totalfavoriteuser = totalFavoriteUsers;
            // Trả về view với danh sách các bài viết đã chia sẻ, nhưng sử dụng các ProfileViewModel thay vì các bài viết
            return View(sharedPosts);
        }
        [HttpPost]
        public async Task<IActionResult> FollowUser(long idFollowing)
        {
            // Lấy IdFollower từ session
            long idFollower = GetLoggedInUserId();

            // Kiểm tra xem đã tồn tại mối quan hệ follow này chưa
            var existingFollow = await _context.Follows.FindAsync(idFollower, idFollowing);
            if (existingFollow != null)
            {
                // Nếu đã tồn tại, không cần thêm vào lại.
                return RedirectToAction("Index", "Profile");
            }

            // Tạo một mối quan hệ mới Follow.
            var follow = new Follow
            {
                IdFollower = idFollower,
                IdFollowing = idFollowing,
                ExistenceStatus = "Active" // Hoặc bất kỳ trạng thái nào bạn muốn gán cho mối quan hệ này.
            };

            _context.Follows.Add(follow);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Profile");
        }
        [HttpPost]
        public async Task<IActionResult> UnfollowUser(long idFollowing)
        {
            // Lấy IdFollower từ session
            long idFollower = GetLoggedInUserId();

            // Tìm mối quan hệ follow dựa trên IdFollower và IdFollowing
            var existingFollow = await _context.Follows.FindAsync(idFollower, idFollowing);
            if (existingFollow == null)
            {
                TempData["ErrorMessage"] = "You haven't followed this user";
                return RedirectToAction("Index", "Profile");
            }

            // Xóa mối quan hệ follow khỏi cơ sở dữ liệu
            _context.Follows.Remove(existingFollow);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Profile");
        }


        [HttpPost]
        public async Task<IActionResult> FavoriteUser(long idFavoriteUser)
        {
            // Lấy IdUser của người dùng hiện tại từ session hoặc từ hệ thống xác thực
            long currentUserId = GetLoggedInUserId();

            // Kiểm tra xem mối quan hệ yêu thích đã tồn tại chưa
            var existingFavorite = await _context.FavoriteUsers.FindAsync(currentUserId, idFavoriteUser);
            if (existingFavorite != null)
            {
                // Nếu đã tồn tại, không cần thêm vào lại.
                return RedirectToAction("Index", "Profile");
            }

            // Tạo một mối quan hệ mới FavoriteUser.
            var favoriteUser = new FavoriteUser
            {
                IdUser = currentUserId,        // IdUser là Id của người dùng đăng nhập
                IdFavoriteUser = idFavoriteUser,  // IdFavoriteUser là Id của người dùng được yêu thích
                CreatedAt = DateTime.Now // Hoặc thời gian khác nếu cần
            };

            _context.FavoriteUsers.Add(favoriteUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Profile");
        }
        [HttpPost]
        public async Task<IActionResult> FavoritePost(long idPost)
        {
            // Lấy IdUser của người dùng hiện tại từ session hoặc từ hệ thống xác thực
            long currentUserId = GetLoggedInUserId();

            // Kiểm tra xem mối quan hệ yêu thích đã tồn tại chưa
            var existingFavorite = await _context.FavoritePosts.FindAsync(currentUserId, idPost);
            if (existingFavorite != null)
            {
                // Nếu đã tồn tại, không cần thêm vào lại.
                return RedirectToAction("Index", "Profile");
            }

            // Tạo một mối quan hệ mới FavoritePost.
            var favoritePost = new FavoritePost
            {
                IdUser = currentUserId,  // IdUser là Id của người dùng đăng nhập
                IdPost = idPost,         // IdPost là Id của bài post được yêu thích
                CreatedAt = DateTime.Now // Hoặc thời gian khác nếu cần
            };

            _context.FavoritePosts.Add(favoritePost);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Profile");
        }
        public IActionResult FavoritePosts()
        {
            // Lấy Id của người dùng hiện tại từ session hoặc từ hệ thống xác thực
            long currentUserId = GetLoggedInUserId();

            // Truy vấn cơ sở dữ liệu để lấy danh sách các bài viết mà người dùng đã yêu thích
            var favoritePosts = _context.FavoritePosts
                                    .Where(fp => fp.IdUser == currentUserId)
                                    .Select(fp => fp.IdPostNavigation)
                                    .ToList();

            return View(favoritePosts);
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
                // Nếu đã tồn tại, không cần thêm vào lại.
                return RedirectToAction("Details", "Posts");
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

            return RedirectToAction("Details", "Posts");
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

            return RedirectToAction("Index", "Posts");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Share(long idPost)
        {
            // Get current user's id
            var currentUserId = GetLoggedInUserId();

            // Create a new shared post object
            var sharedPost = new Share
            {
                SharedAt = DateTime.Now,
                IdUser = currentUserId,
                IdPost = idPost
            };

            // Add the shared post to the database
            _context.Shares.Add(sharedPost);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details","Posts", new { id = idPost }); // Redirect to wherever you want after sharing
        }
        public async Task<IActionResult> sharePosts()
        {
            // Get the current user's id
            var currentUserId = GetLoggedInUserId();

            // Retrieve the list of shared posts by the current user
            var sharedPosts = _context.Shares.Where(sp => sp.IdUser == currentUserId)
                                                  .Select(sp => new ProfileViewModel
                                                  {
                                                      Title = sp.IdPostNavigation.Title,
                                                      ContentPost = sp.IdPostNavigation.ContentPost,
                                                      ImagePost = sp.IdPostNavigation.ImagePost,
                                                       // Assuming FullName is the name of the user who shared the post
                                                  })
                                                  .ToList();

            return View(sharedPosts);
        }
        [HttpPost]
        public async Task<IActionResult> UnsharePost(long idPost)
        {
            long currentUserId = GetLoggedInUserId();

            var shareToRemove = await _context.Shares.FirstOrDefaultAsync(s => s.IdUser == currentUserId && s.IdPost == idPost);
            if (shareToRemove != null)
            {
                _context.Shares.Remove(shareToRemove);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details","Posts", new { id = idPost });
        }
        //[HttpGet]
        //public IActionResult Followers()
        //{
        //    // Lấy userId của người dùng hiện tại từ session
        //    var userId = HttpContext.Session.GetString("userId");

        //    // Lấy danh sách người theo dõi của người dùng từ cơ sở dữ liệu
        //    var followers = _context..Where(f => f.UserId == userId).ToList();

        //    return View(followers);
        //}
        //[HttpGet]
        //public IActionResult Following()
        //{
        //    // Lấy userId của người dùng hiện tại từ session
        //    var userId = HttpContext.Session.GetString("userId");

        //    // Lấy danh sách người đang theo dõi bởi người dùng từ cơ sở dữ liệu
        //    var following = _context.Followers.Where(f => f.FollowerId == userId).ToList();

        //    return View(following);
        //}

        [HttpGet]
        public async Task<IActionResult> Followers()
        {
            // Lấy Id của người dùng hiện tại từ session hoặc từ hệ thống xác thực
            long currentUserId = GetLoggedInUserId();

            // Truy vấn cơ sở dữ liệu để lấy danh sách các người theo dõi (followers) của người dùng hiện tại
            var followers = await _context.Follows
                                    .Include(f => f.IdFollowerNavigation) // Include để lấy thông tin của người theo dõi
                                    .Where(f => f.IdFollowing == currentUserId)
                                    .Select(f => f.IdFollowerNavigation.FullName) // Chọn thuộc tính cần hiển thị
                                    .ToListAsync();

            return View(followers);
        }

        [HttpGet]
        public async Task<IActionResult> Following()
        {
            // Lấy Id của người dùng hiện tại từ session hoặc từ hệ thống xác thực
            long currentUserId = GetLoggedInUserId();

            // Truy vấn cơ sở dữ liệu để lấy danh sách các người mà người dùng hiện tại đang theo dõi (following)
            var following = await _context.Follows
                                    .Include(f => f.IdFollowingNavigation) // Include để lấy thông tin của người được theo dõi
                                    .Where(f => f.IdFollower == currentUserId)
                                    .Select(f => f.IdFollowingNavigation.FullName) // Chọn thuộc tính cần hiển thị
                                    .ToListAsync();

            return View(following);
        }
        [HttpPost]
        public async Task<IActionResult> UnfavoriteUser(long idFavoriteUser)
        {
            // Lấy IdUser của người dùng hiện tại từ session hoặc từ hệ thống xác thực
            long currentUserId = GetLoggedInUserId();

            // Tìm mối quan hệ yêu thích để xóa
            var favoriteToRemove = await _context.FavoriteUsers
                .FirstOrDefaultAsync(f => f.IdUser == currentUserId && f.IdFavoriteUser == idFavoriteUser);

            if (favoriteToRemove != null)
            {
                _context.FavoriteUsers.Remove(favoriteToRemove);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UnfavoritePost(long idPost)
        {
            // Lấy IdUser của người dùng hiện tại từ session hoặc từ hệ thống xác thực
            long currentUserId = GetLoggedInUserId();

            // Tìm mối quan hệ yêu thích để xóa
            var favoriteToRemove = await _context.FavoritePosts
                .FirstOrDefaultAsync(f => f.IdUser == currentUserId && f.IdPost == idPost);

            if (favoriteToRemove != null)
            {
                _context.FavoritePosts.Remove(favoriteToRemove);
                await _context.SaveChangesAsync();
            }
            TempData["ErrorMessage"] = "xóa yêu thích bài viết thành công.";
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
      
        public IActionResult Profile()
        {

            return View();
        }
    }
}