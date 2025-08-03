using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopThoiTrangASP.Models;

namespace ShopThoiTrangASP.Controllers
{
    public class ModuleController : Controller
    {
        private ShopThoiTrangASPDBContext db = new ShopThoiTrangASPDBContext(); 
        // GET: Module-----MENU NGANG---------
        public ActionResult ListCategoryMenu()
        {
            // Đầu tiên lấy danh sách loại sản phẩm
            var list = db.Categorys.Where(m => m.ParentID == 0 && m.Status == 1).ToList();
            return View("ListCategoryMenu", list);
        }
    }
}