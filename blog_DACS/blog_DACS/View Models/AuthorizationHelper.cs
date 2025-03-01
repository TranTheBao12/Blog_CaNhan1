using blog_DACS.Models;

namespace blog_DACS.View_Models
{
    public class AuthorizationHelper
    {
        public static bool IsUser(User user)
        {
            return user != null && user.IdRole == 2;
        }
    }
}
