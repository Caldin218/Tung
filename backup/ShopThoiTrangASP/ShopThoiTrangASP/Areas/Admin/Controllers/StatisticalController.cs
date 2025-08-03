using ShopThoiTrangASP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrangASP.Areas.Admin.Controllers
{
    public class StatisticalController : Controller
    {
        private ShopThoiTrangASPDBContext db = new ShopThoiTrangASPDBContext();
        // GET: Admin/Statistical
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetStatistical(string FromDate, string ToDate)
        {    //Select các bảng với các điều kiện và gán vào biến query kiểu var....
             //Từ đó ta nói biến query chính là một "TRUY VẤN CHƯA THỰC THI"
                var query = from o in db.Orders //Gọi bảng Orders
                            join od in db.OrderDetails //join bảng OrderDetails
                            on o.ID equals od.OrderID
                            join p in db.Products //join bảng Products
                            on od.ProductID equals p.ID
                            select new
                            {
                                CreatedDate = o.Created_At, //thuộc tính CreatedDate được gán bằng giá trị ngày tạo của thuộc tính Created_At trong đối tượng Order
                                Quantity = od.Qty,  //thuộc tính Quantity được gán bằng giá trị số lượng đơn đặt hàng của thuộc tính Qty trong đối tượng OrderDetail
                                Price = od.Price,  //thuộc tính Price được gán bằng giá trị giá tiền của thuộc tính Price trong đối tượng OrderDetail
                                OriginalPrice = p.OriginalPrice,//thuộc tính OriginalPrice được gán bằng giá trị giá gốc của sản phẩm của thuộc tính OrgiginalPrice trong đối tượng Product
                            };

            if (!string.IsNullOrEmpty(FromDate))
            {
                DateTime startDate = DateTime.ParseExact(FromDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.CreatedDate >= startDate);
            }
            if (!string.IsNullOrEmpty(ToDate))
            {
                DateTime endDate = DateTime.ParseExact(ToDate, "yyyy-MM-dd", null).AddDays(1); // để bao gồm ngày cuối cùng
                query = query.Where(x => x.CreatedDate < endDate);
            }


            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreatedDate)) 
                .Select(x => new //Biến đồi dữ liệu lần 1
                {
                        Date = x.Key.Value, //Groupby trả kết quả là Key và Chỉ có Select đầu tiên mới lấy được nó....
                        TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice), //TổngVốn
                        TotalSell = x.Sum(y => y.Quantity * y.Price)         //TổngDoanhThu         
                })
                .Select(x => new //Biến đồi dữ liệu lần 2
                {
                        Date = x.Date,
                        DoanhThu = x.TotalSell, 
                        LoiNhuan = x.TotalSell - x.TotalBuy //DoanhThu - Vốn = LợiNhuận
                });
                return Json( result, JsonRequestBehavior.AllowGet);
        }
    }
}