using blog_DACS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using blog_DACS.View_Models;
using Microsoft.CodeAnalysis.Scripting;
using BCrypt.Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Mail;
using System.Net;
namespace blog_DACS.Controllers
{
    public class DefaultController : Controller
    {
        private readonly BlogcanhannContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;
   
        private readonly IConfiguration _configuration;
        public DefaultController(BlogcanhannContext context, IHttpContextAccessor httpContextAccessor, IMemoryCache cache,  IConfiguration configuration)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
           
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {

            var session = _httpContextAccessor.HttpContext.Session;
            if (session.GetString("user") != null)
            {
                return RedirectToAction("Index", "Posts");
            }
            return View();

        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var usr = email;
            var pass = password;

            // Tìm người dùng trong cơ sở dữ liệu bằng email
            var user = _context.Users.FirstOrDefault(x => x.Email == usr);

            if (user != null)
            {
                // Mật khẩu đã lưu trong cơ sở dữ liệu
                string hashedPasswordFromDatabase = user.Pass;

                // So sánh mật khẩu đã nhập từ form và mật khẩu đã được mã hóa từ cơ sở dữ liệu
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(pass, hashedPasswordFromDatabase);

                if (isPasswordValid)
                {
                    HttpContext.Session.SetString("userId", user.IdUser.ToString());
                    // Mật khẩu hợp lệ, kiểm tra vai trò của người dùng
                    if (user.IdRole == 1)
                    {
                        HttpContext.Session.SetString("user", email);
                        TempData["AlertMessage"] = "Mã thành viên: " + user.IdUser;
                        return RedirectToAction("Index", "Home1");
                    }
                    else if (user.IdRole == 2)
                    {
                        HttpContext.Session.SetString("user", email);
                        TempData["AlertMessage"] = "Mã thành viên: " + user.IdUser;
                        return RedirectToAction("Index", "Posts");
                    }
                }
            }

            // Người dùng không tồn tại hoặc mật khẩu không hợp lệ
            TempData["ErrorMessage"] = "Đăng nhập thất bại!";
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Logout()
        {
            // Xóa session khi đăng xuất
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register([Bind("IdUser,FullName,PhoneNumber,Email,DateOfBirth,Gender,PlaceOfBirth,CreatedAt,LastUpdatedAt,ExistenceStatus,Pass,IdRole")] User user)
        {

            // Mã hóa mật khẩu trước khi lưu vào CSDL
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Pass);
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);

            if (existingUser == null)
            {
                // Tạo đối tượng User mới và thêm vào cơ sở dữ liệu nếu chưa tồn tại
                var newUser = new User
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Pass = hashedPassword,
                    IdRole = 2,
                    // Các thuộc tính khác bạn muốn thiết lập
                };

                // Thêm mới IdUser dựa trên giá trị lớn nhất hiện tại trong cơ sở dữ liệu
                newUser.IdUser = _context.Users.Max(p => p.IdUser) + 1;

                _context.Users.Add(newUser);
                _context.SaveChanges();
                return RedirectToAction("RegistrationSuccess");
            }
            else
            {
                // Code xử lý khi ModelState không hợp lệ

                // Trả về một giá trị hợp lệ
                return View("Login", "Default");
            }

        }



        public IActionResult RegistrationSuccess()
        {
            return View();
        }



        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(string currentPassword, string newPassword)
        {
            // Lấy thông tin người dùng hiện tại từ session hoặc bất kỳ phương thức nào khác
            var currentUserEmail = HttpContext.Session.GetString("user");

            // Kiểm tra xem người dùng có tồn tại không
            var currentUser = _context.Users.FirstOrDefault(u => u.Email == currentUserEmail);

            if (currentUser != null)
            {
                // Kiểm tra mật khẩu hiện tại
                if (BCrypt.Net.BCrypt.Verify(currentPassword, currentUser.Pass))
                {
                    // Kiểm tra tính hợp lệ của mật khẩu mới
                    if (IsPasswordValid(newPassword))
                    {
                        // Mã hóa mật khẩu mới
                        string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

                        // Cập nhật mật khẩu mới trong cơ sở dữ liệu
                        currentUser.Pass = hashedNewPassword;
                        _context.SaveChanges();

                        // Chuyển hướng hoặc trả về thông báo thành công
                        return RedirectToAction("Index", "Posts");
                    }
                    else
                    {
                        // Trả về thông báo lỗi nếu mật khẩu mới không hợp lệ
                        TempData["ErrorMessage"] = "Mật khẩu mới không hợp lệ!";
                    }
                }
                else
                {
                    // Trả về thông báo lỗi nếu mật khẩu hiện tại không chính xác
                    TempData["ErrorMessage"] = "Mật khẩu hiện tại không chính xác!";
                }
            }
            else
            {
                // Trả về thông báo lỗi nếu người dùng không tồn tại
                TempData["ErrorMessage"] = "Người dùng không tồn tại!";
            }

            // Chuyển hướng hoặc trả về trang Change Password nếu có lỗi
            return RedirectToAction("ChangePassword");
        }
        // Phương thức kiểm tra tính hợp lệ của mật khẩu mới
        private bool IsPasswordValid(string newPassword)
        {
            // Kiểm tra độ dài tối thiểu của mật khẩu
            if (newPassword.Length < 8)
            {
                return false; // Mật khẩu quá ngắn
            }

            // Kiểm tra xem mật khẩu có chứa ít nhất một ký tự số không
            bool hasNumber = false;
            foreach (char c in newPassword)
            {
                if (char.IsDigit(c))
                {
                    hasNumber = true;
                    break;
                }
            }

            if (!hasNumber)
            {
                return false; // Mật khẩu không chứa số
            }

            // Kiểm tra xem mật khẩu có chứa ít nhất một ký tự in hoa không
            bool hasUpperCase = false;
            foreach (char c in newPassword)
            {
                if (char.IsUpper(c))
                {
                    hasUpperCase = true;
                    break;
                }
            }

            if (!hasUpperCase)
            {
                return false; // Mật khẩu không chứa chữ in hoa
            }

            // Nếu tất cả các điều kiện đều được đáp ứng, mật khẩu được coi là hợp lệ
            return true;
        }
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
 
             [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string pass, string newpass)
        {
            if (pass != newpass)
            {
                TempData["AlertMessage"] = "Mật khẩu không khớp";
                return View();
            }
            var resetToken = HttpContext.Session.GetString("ResetToken");

            var email = HttpContext.Session.GetString("Email");

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (resetToken != token)
            {
                TempData["AlertMessage"] = "Mã token này hết hạn, vui lòng gửi lại token mới";
                return View();
            }

            user.Pass = BCrypt.Net.BCrypt.HashPassword(pass);
            await _context.SaveChangesAsync();
            TempData["AlertMessage"] = "Đổi mật khẩu thành công";
            return RedirectToAction("Login", "Default");
        }
  [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                TempData["AlertMessage"] = "Email không tồn tại trong hệ thống";
                return View();
            }

            var token = Guid.NewGuid().ToString();
            var tokenExpiry = DateTime.UtcNow.AddMinutes(10);

            // Lưu token và tokenExpiry vào bộ nhớ đệm với key là email
            _cache.Set(email, new { Token = token, Expiry = tokenExpiry }, tokenExpiry);

            _httpContextAccessor.HttpContext.Session.SetString("ResetToken", token);
            _httpContextAccessor.HttpContext.Session.SetString("Email", email);

            var emailMessage = new MailMessage
            {
                From = new MailAddress("admin@gmail.com", "AdminBaotran"),
                Subject = "Yêu cầu đặt lại mật khẩu",
                Body = $"Để đặt lại mật khẩu, vui lòng sử dụng token này: {token} mã token có thời hạn 10 phút",
                IsBodyHtml = false,
            };

            emailMessage.To.Add(new MailAddress(email));

            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("tranthebaobt8@gmail.com", "kaiv zped dlrx xfgx");
                await smtpClient.SendMailAsync(emailMessage);
            }

          
            return RedirectToAction("ResetPassword", "Default");
        }


        // Phương thức xử lý yêu cầu đặt lại mật khẩu

        // Phương thức hiển thị trang xác nhận đã gửi email
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // Phương thức hiển thị trang đặt lại mật khẩu
    
        // Phương thức xử lý đặt lại mật khẩu
      
        // Phương thức hiển thị trang xác nhận đặt lại mật khẩu thành công
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}


    


    
    



  
