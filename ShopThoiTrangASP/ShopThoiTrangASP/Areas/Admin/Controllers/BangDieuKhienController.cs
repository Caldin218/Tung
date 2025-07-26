using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopThoiTrangASP.Filters;


namespace ShopThoiTrangASP.Areas.Admin.Controllers
{
    [AuthorizeAdmin] // ✅ Dòng này bắt buộc người dùng phải đăng nhập & có Role=Admin
    public class BangDieuKhienController : Controller
    {
        // GET: Admin/BangDieuKhien
        public ActionResult Index()
        {
            return View();
        }
    }
}