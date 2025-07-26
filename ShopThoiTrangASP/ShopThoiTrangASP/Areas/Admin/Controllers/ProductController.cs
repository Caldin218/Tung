    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using ShopThoiTrangASP.Models;
    using System.IO;

    namespace ShopThoiTrangASP.Areas.Admin.Controllers
    {
        public class ProductController : Controller
        {
            private ShopThoiTrangASPDBContext db = new ShopThoiTrangASPDBContext();

            // GET: Admin/Product
            public ActionResult Index()
            {
                var list = db.Products
                    .Join(
                        db.Categorys, //Join Bảng Categorys (Products p inner join Categorys c 
                        c => c.CatID, // Định nghĩa biểu thức ( khóa ngoại ) 
                        p=> p.ID, // Định nghĩa biểu thức ( khóa chính )
                        (p,c)=>new ProductCategory // on p.ID = c.CatID  AS ProductCategory
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
                    .Where(m => m.Status != 0)
                    .OrderBy(m => m.Created_At)
                    .ToList();
                return View("Index", list);
            }
            public ActionResult Trash()
            {
                var list = db.Products.Where(m => m.Status == 0)
                   .OrderBy(m => m.Created_At)
                   .ToList();
                return View("Trash", list);
            }

            // GET: Admin/Product/Details/5
            public ActionResult Details(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }

            // GET: Admin/Product/Create
            public ActionResult Create()
            {
                ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name");
                return View();
            }

            // POST: Admin/Product/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create(Product product)
            {
                if (ModelState.IsValid)
                {
                    string slug = SlugHelper.GenerateSlug(product.Name);  //doi ten thanh slug
                    product.Slug = slug;
                    product.Created_At = DateTime.Now;
                    product.Created_By = 1;
                    product.Updated_At = DateTime.Now;
                    product.Updated_By = 1;
                    product.Status = 1;
                    //Khâu xử lý dữ liệu tham số (File hình ảnh) từ User
                    //Sử dụng đối tượng ngầm định Request
                        var Img = Request.Files["FileImg"]; //xử lý tiếp nhận tham số (hình ảnh thì nên dùng kiểu var dễ xử lý)
                        string[] FileExtention = { ".jpg",".png",".gif"};
                        if(Img.ContentLength != 0)
                        {
                               // Kiểm tra đuôi định dạng file
                            if(FileExtention.Contains(Img.FileName.Substring(Img.FileName.LastIndexOf("."))))
                            {
                              //Đúng định dạng thì lưu vào CSDL và Server khi độ dài phụ hợp và định dạng phù hợp

                               //Tạo một biến mới để chứa....vd là thoi-trang-nam.jpg
                                string imgName = slug + Img.FileName.Substring(Img.FileName.LastIndexOf("."));
                                product.Img = imgName; //Cập nhật tên ảnh đã được gán thêm slug vào "đối tượng (Model)product" đại diện trong CSDL chứ không phải lưu trực tiếp vào CSDL nhé và để lưu trực tiếp thì đã có lệnh với sự hỗ trợ của mô hình dữ liệu.
                               //Tạo một biến mới để chứa....vd là ~/images/home/thoi-trang-nam.jpg
                                string PathImg = Path.Combine(Server.MapPath("~/images/home/"), imgName); //Path.
                                Img.SaveAs(PathImg); //Lưu Tên đường dẫn File ảnh vào server
                            }
                        }
                
                    //Thêm và Lưu trực tiếp vào CSDL va chuyen huong trang
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Product");
                } //kiểm tra tính nhất quán của File ảnh
                ViewBag.ListCat = new SelectList(db.Products.ToList(), "Id", "Name", 0);
                ViewBag.ListOrder = new SelectList(db.Products.ToList(), "Orders", "Name", 0);
                return View(product);
            }

            // GET: Admin/Product/Edit/5
            public ActionResult Edit(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0); // Thêm tham số thứ ba để chọn giá trị hiện tại
                return View(product);
            }

            // POST: Admin/Product/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit(Product product)
            {
                if (ModelState.IsValid)
                {
                    string slug = SlugHelper.GenerateSlug(product.Name);  //doi ten thanh slug
                    product.Slug = slug;
                    product.Updated_At = DateTime.Now;
                    product.Updated_By = 1;
                    product.Status = 1;
                    //hinh anh
                    var Img = Request.Files["FileImg"];
                    string[] FileExtention = { ".jpg", ".png", ".gif" };
                    if (Img.ContentLength != 0)
                    {
                        if (FileExtention.Contains(Img.FileName.Substring(Img.FileName.LastIndexOf("."))))
                        {
                            //đúng hình thì upload file thoi-trang-nam.jpg
                            string imgName = slug + Img.FileName.Substring(Img.FileName.LastIndexOf("."));
                            //xoa hinh cu~
                            String DelPath = Path.Combine(Server.MapPath("~/images/home/"), product.Img);
                            if (System.IO.File.Exists(DelPath))
                            {
                                System.IO.File.Delete(DelPath);
                            }
                            //luu tep tin len server
                            product.Img = imgName; 
                            string PathImg = Path.Combine(Server.MapPath("~/images/home/"), imgName);
                            Img.SaveAs(PathImg); //luu File len server
                        }
                    }
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ListCat = new SelectList(db.Categorys.ToList(), "Id", "Name", 0);
                return View(product);
            }

            // GET: Admin/Product/Delete/5
            public ActionResult Delete(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }

            // POST: Admin/Product/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(int id)
            {
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            public ActionResult Status(int id)
            {
                Product product = db.Products.Find(id);
                //cập nhật vào đối tượng 
                int status = (product.Status == 1) ? 2 : 1;
                product.Status = status;
                product.Updated_By = 1; 
                product.Updated_At = DateTime.Now;
                //Thay đối và Lưu Trong CSDL
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");

            }

            public ActionResult DelTrash(int id)
            {
                Product product = db.Products.Find(id);
                product.Status = 0;
                product.Updated_By = 1; 
                product.Updated_At = DateTime.Now;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Product");

            }

            public ActionResult Restore(int id)
            {
                Product product = db.Products.Find(id);
                product.Status = 2;
                product.Updated_By = 1; 
                product.Updated_At = DateTime.Now;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Trash", "Product");

            }
        }
    }
