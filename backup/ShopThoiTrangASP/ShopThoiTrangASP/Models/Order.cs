using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace ShopThoiTrangASP.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public DateTime? Created_At { get; set; }
        public int? Updated_By { get; set; }
        public DateTime? Updated_At { get; set; }
        public int Status { get; set; }
    }
    public enum Status
    {
        Pending = 0,       // Đang chờ xử lý
        Processing = 1,    // Đang xử lý
        Shipped = 2,       // Đã giao hàng
        Delivered = 3,     // Đã nhận hàng
        Cancelled = 4      // Đã hủy
    }

}