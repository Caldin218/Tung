using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopThoiTrangASP.Models
{
    public class Cart
    {
        public Product product { get; set; }  // Lưu trữ thông tin của sản phẩm
        public int Quantity { get; set; }   // Số lượng sản phẩm trong giỏ hàng
      //Constructor
        public Cart(Product product, int quantity)
        {
            this.product = product;  // Gán giá trị cho thuộc tính 'product'
            this.Quantity = quantity;  // Gán giá trị cho thuộc tính 'Quantity'
        }
    }
}