using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrangASP.Filters
{
    public class AuthorizeAdminAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            var username = session["Username"];
            var role = session["Roles"];

            // Kiểm tra nếu chưa đăng nhập hoặc không phải Admin và Employee thì redirect
            if (username == null || role == null || role.ToString() != "Admin" & role.ToString() != "Employee")
            {
                // Redirect về trang login
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }






    }
}
