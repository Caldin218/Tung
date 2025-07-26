using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopThoiTrangASP.Models;

namespace ShopThoiTrangASP.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ShopThoiTrangASPDBContext dbContext = new ShopThoiTrangASPDBContext();
        private string strCart = "Cart"; 

        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }

     
        public ActionResult OrderNow(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Session[strCart] == null)
            {
                List<Cart> newCart = new List<Cart>
                {
                    new Cart(dbContext.Products.Find(Id), 1)
                };
                Session[strCart] = newCart;
            }
            else
            {
                List<Cart> listCart = (List<Cart>)Session[strCart];
                int index = IsExistingCheck(Id);

                if (index == -1)
                {
                    listCart.Add(new Cart(dbContext.Products.Find(Id), 1));
                }
                else
                {
                    listCart[index].Quantity++;
                }

                Session[strCart] = listCart; //Cập nhật lại giỏ hàng để đồng bộ với client và server
            }

            return RedirectToAction("Index");
        }

        private int IsExistingCheck(int? Id)
        {
            List<Cart> listCart = (List<Cart>)Session[strCart]; //check "cart hiện tại"
            for (int i = 0; i < listCart.Count; i++)
            {
                if (listCart[i].product.ID == Id)
                {
                    return i; // Tìm được sp trùng
                }
            }
            return -1; //ko tìm thấy sp trùng
        }

        public ActionResult RemoveItem(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int Index = IsExistingCheck(Id);
            List<Cart> listCart = (List<Cart>)Session[strCart];

            if (Index != -1)
            {
                //Xóa
                listCart.RemoveAt(Index);
                if (listCart.Count == 0)
                {
                    Session[strCart] = null;
                }
                else
                {
                    Session[strCart] = listCart; //Cập nhật lại giỏ hàng để đồng bộ với client và server
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateCart(FormCollection field)
        {
            string[] quantities = field.GetValues("quantity");
            List<Cart> listCart = (List<Cart>)Session[strCart]; 

            for (int i = 0; i < listCart.Count; i++)
            {
                listCart[i].Quantity = Convert.ToInt32(quantities[i]);
            }

            Session[strCart] = listCart;
            return RedirectToAction("Index");
        }

        public ActionResult ClearCart()
        {
            Session[strCart] = null;
            return RedirectToAction("Index");
        }

        public ActionResult Checkout()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult ProcessOrder(FormCollection field)
        {
            List<Cart> listCart = (List<Cart>)Session[strCart];

            // 1. Save the order into the Order table
            var order = new Order()
            {
                Name = field["cusName"],
                Phone = field["cusPhone"],
                Email = field["cusEmail"],
                Created_At = DateTime.Now,
                Status = 1 //Processing
            };
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();

            // 2. Save the order details into the OrdetDetail table
            foreach (var cart in listCart)
            {
                var orderDetail = new OrdetDetail()
                {
                    ID = order.ID,
                    ProductID = cart.product.ID,
                    Qty = Convert.ToInt32(cart.Quantity),
                    Price = Convert.ToDouble(cart.product.Price)
                };
                dbContext.OrderDetails.Add(orderDetail);
                try
                {
                    dbContext.SaveChanges();
                }

                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.InnerException?.Message);
                    Console.WriteLine(ex.InnerException?.StackTrace);


                }
            }

            // 3. Clear the shopping cart session
            Session.Remove(strCart); //Kết thúc một phiên làm việc 
            return View("OrderSuccess");
        }
    }
}
