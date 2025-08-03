using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ShopThoiTrangASP.Models;
namespace ShopThoiTrangASP.Controllers
{
    public class TrangchuController : Controller
    {
        private ShopThoiTrangASPDBContext db = new ShopThoiTrangASPDBContext();
        // GET: Trangchu
        public ActionResult Index()
        {
            var listcat = db.Categorys.Where(m => m.Status == 1 && m.ParentID == 0).ToList();
            return View(listcat);
        }
        [ChildActionOnly]
        public ActionResult ProductHome( string namecat, int catid)
        {
            var list = db.Products.Where(p => p.CatID == catid).ToList();
             ViewBag.Catname = namecat;
            return View("ProductHome", list);
        }
        public ActionResult Details(int? id, string Name)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.name = Name;
            var product = db.Products
                    .Join(
                        db.Categorys, //Join Bảng Categorys (Products p inner join Categorys c 
                        c => c.CatID, // Định nghĩa biểu thức ( khóa ngoại ) 
                        p => p.ID, // Định nghĩa biểu thức ( khóa chính )
                        (p, c) => new ProductCategory // on p.ID = c.CatID  AS ProductCategory
                        {
                            ID = p.ID,
                            CatID = p.CatID,
                            Name = p.Name,
                            Slug = p.Slug,
                            Detail = p.Detail,
                            Metadesc = p.Metadesc,
                            Metakey = p.Metakey,
                            Img = p.Img,
                            Number = p.Number,
                            Price = p.Price,
                            PriceSale = p.PriceSale,
                            Created_At = p.Created_At,
                            Created_By = p.Updated_By,
                            Updated_At = p.Updated_At,
                            Updated_By = p.Updated_By,
                            Status = p.Status,
                            CatName = c.Name
                        }
                    )
                    .Where(m => m.Status != 0 && m.ID == id)
                    .OrderBy(m => m.Created_At)
                    .FirstOrDefault(); // Lấy một đối tượng duy nhất

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        //Clock
        public ActionResult ServerTime()
        {
            var text = DateTime.Now.ToString("HH:mm:ss tt");
            return Content(text);
        }
    }
}


