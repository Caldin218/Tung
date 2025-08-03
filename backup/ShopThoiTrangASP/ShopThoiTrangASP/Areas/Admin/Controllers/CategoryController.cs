using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopThoiTrangASP.Models;

namespace ShopThoiTrangASP.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        //khai báo 1 lần duy nhất khỏi phải gọi đối tượng mới liên tục trong action nữa...và chỉ sử dụng trong nội bộ
        private ShopThoiTrangASPDBContext db = new ShopThoiTrangASPDBContext();

        // GET: Admin/Category
        public ActionResult Index()
        {
            var list = db.Categorys.Where(m => m.Status != 0)
                .OrderBy(m => m.Created_At)
                .ToList();
            return View("Index", list);
        }

        public ActionResult Trash()
        {
           //List<Category> list =........
            var list = db.Categorys.Where(m => m.Status == 0)
               .OrderByDescending(m => m.Created_At)
               .ToList();
            return View("Trash", list);
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categorys.Find(id); //<=> Var category = db.Categorys.Find(id); 
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Category/Create------------------------------------------

        [HttpGet] //Hiện thị giao diện - tạo mới một Category
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name");
            ViewBag.Orders = new SelectList(db.Categorys.ToList(), "Orders", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Category category)
        {
            if (ModelState.IsValid)
            {
                if(category.ParentID == null)
                {
                    category.ParentID = 0;
                }
                if (category.Orders == null)
                {
                    category.Orders = 0;
                }
                string slug = SlugHelper.GenerateSlug(category.Name);  //doi ten thanh slug
                category.Slug = slug; //Lưu cái slug mới này vào field Slug của CSDL
                category.Created_At = DateTime.Now;
                category.Created_By = 1;
                category.Updated_At = DateTime.Now;
                category.Updated_By = 1;
               //luu va chuyen huong trang
                db.Categorys.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index","Category");
            }
            // Nếu không hợp lệ, trả về lại form với các lựa chọn đã chọn và quay lại [HttpGet]
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(db.Categorys.ToList(), "Orders", "Name", 0);
            return View("Create",category);
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categorys.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            // Nếu không hợp lệ, trả về lại form với các lựa chọn đã chọn
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name");
            ViewBag.Orders = new SelectList(db.Categorys.ToList(), "Orders", "Name");
            return View(category);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentID == null)
                {
                    category.ParentID = 0;
                }

                if (category.Orders == null)
                {
                    category.Orders = 0;
                }

                string slug = SlugHelper.GenerateSlug(category.Name);  //doi ten thanh slug
                category.Slug = slug;
                category.Created_At = DateTime.Now;
                category.Created_By = 1;
              
                //luu va chuyen huong trang
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // Nếu không hợp lệ, trả về lại form với các lựa chọn đã chọn
            ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name");
            ViewBag.Orders = new SelectList(db.Categorys.ToList(), "Orders", "Name");
            return View(category);
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categorys.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categorys.Find(id);
            db.Categorys.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Status(int id) //cho phép và yêu cầu nhận tham số. Nêu không thì không thể tìm id trong CSDL được
        { 
         //tìm bản ghi trong CSDL tương ứng
            Category category = db.Categorys.Find(id);
         //kiểm tra điều kiện để phân loại
            int status = (category.Status == 1) ? 2 : 1 ;
        //Gán biến vào để cập nhật 
            category.Status = status;
            category.Updated_By = 1; 
            category.Updated_At = DateTime.Now;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index","Category");
        }

        public ActionResult DelTrash(int id)
        {
            Category category = db.Categorys.Find(id);
            category.Status = 0  ;
            category.Updated_By = 1; ;
            category.Updated_At = DateTime.Now;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();  
            return RedirectToAction("Index","Category");

        }

        public ActionResult Restore(int id)
        {
            Category category = db.Categorys.Find(id);
            category.Status = 2;
            category.Updated_By = 1; ;
            category.Updated_At = DateTime.Now;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash","Category");

        }
    }
}

