using ShopThoiTrangASP.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace ShopThoiTrangASP.Controllers
{
    public class AccountController : Controller
    {
        private ShopThoiTrangASPDBContext db = new ShopThoiTrangASPDBContext();

        // GET: /Account/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            string hashedPassword = GetMD5(password);
            var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword && u.Status == 1);


            //nếu là user
            if (user != null)
            {
                Session["UserID"] = user.ID;
                Session["Username"] = user.Username;
                Session["FullName"] = user.FullName;
                Session["Roles"] = user.Roles;

                if (user.Roles == "Admin")
                    return RedirectToAction("Index", "BangDieuKhien", new { Area = "Admin" });
                else
                    return RedirectToAction("Index", "Trangchu");
            }

            //nếu là admin
            if (user.Roles == "Admin")
            {
                return RedirectToAction("Index", "BangDieuKhien", new { Area = "Admin" });
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
            return View();
        }

        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string plainPassword = user.Password; // giữ lại password gốc trước khi bị binding lại
                    string hashedPassword = GetMD5(plainPassword);

                    user.Password = hashedPassword;
                    user.Created_At = DateTime.Now;
                    user.Created_By = null;
                    user.Updated_At = null;
                    user.Updated_By = null;
                    user.Status = 1;
                    user.Roles = "User";

                    db.Users.Add(user);
                    db.SaveChanges();

                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Lỗi khi đăng ký: " + ex.Message;
                }
            }
            else
            {
                ViewBag.Error = "Dữ liệu không hợp lệ.";
            }

            return View(user);
        }



        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        // Hàm băm password
        public static string GetMD5(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(str);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }
    }
}
