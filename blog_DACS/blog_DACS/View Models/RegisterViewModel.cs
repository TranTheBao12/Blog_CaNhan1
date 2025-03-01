using System.ComponentModel.DataAnnotations;

namespace blog_DACS.View_Models
{
    public class RegisterViewModel
    {
        public long IdUser { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Pass { get; set; }
        public long IdRole { get; set; }

    }
}
