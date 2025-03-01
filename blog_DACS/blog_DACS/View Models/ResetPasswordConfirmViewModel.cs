namespace blog_DACS.View_Models
{
    public class ResetPasswordConfirmViewModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
