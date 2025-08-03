using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ShopThoiTrangASP.Models;
using PagedList;

namespace ShopThoiTrangASP.Controllers
{
    public class SanphamController : Controller
    {
        private ShopThoiTrangASPDBContext db = new ShopThoiTrangASPDBContext();
        // GET: FormLoaiSanpham => Đây là nơi nhận "dữ liệu tham số" từ View vd như từ Url,.v.v..Thông thường nơi mà chứa dữ liệu tạm thời là Viewbag..Sau đó ta có thể sử dụng dữ liệu đó tiếp tục làm việc và gửi lại về cho View..=> Cách hoạt động 1-1 giữa View-Controller
        public ActionResult Category (int catid, string slug, int? page, string namecat) // Sau khi nhận được tham số từ Module thì sẽ gửi tham số này vào cho Linq xử lý 
        {
            ViewBag.Catname = namecat;
            ViewBag.slug = slug;
            ViewBag.catid = catid;
            var pageNumber = page ?? 1;
            var pageSize = 6;
            // còn về tham số slug thì đã có route ánh xạ kể từ khi truyền vào action rồi. Nhưng đầu tiên phải hiểu linq sử dung để làm gì thì linq
            // ở đây dùng để lọc sản phẩm từ tham số nhưng ở đây nếu mục điích chỉ là cho SEO hoặc hiện thị cho URL đẹp hơn thì không cần thiết query
            // trong vì đã nói rồi thì linq để lọc dữ liệu tức là sản phẩm, còn ở đây thì chỉ cần hiển thị thôi hoàn toàn không liên quan nhưng cách
            // xài thì dễ bị nhầm lẫn. Tuy nhiên để cho route hoạt động thì Action vẫn phải cho phép truyền tham số mà route cần để nó hoạt động(nhớ
            // là phải đúng action đúng controller)

            var list = db.Products
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
                  .Where(p => p.CatID == catid ) // xử lý tham số catid từ URL trong module gửi về
                  .OrderBy(p => p.ID)
                  .ToPagedList(pageNumber, pageSize);
            //khi lọc sản phẩm xong thì gủi dữ liệu lên view thôi...Ngẫm lại thì cội nguồn từ controller Module nhỉ? sau đó nó gửi list lên view menu
            //rồi nó đẻ ra đống thẻ liên kết <a> rồi khi bấm vào rồi thì helper gửi cái url kèm theo 2 tham số về cho SanphamController rồi quăng 2 tham
            //số xử lý lần nữa để lấy sản phẩm theo loại cụ thể trước đó đã phân ra hết ở thẻ liên kết <a> từ trang view menu của Module mà nghĩ lại ghê
            //thật nếu không có vòng lặp thì ghi ra các thẻ chắc ói máu
            return View("ProductCategory", list);
        }
    }
}