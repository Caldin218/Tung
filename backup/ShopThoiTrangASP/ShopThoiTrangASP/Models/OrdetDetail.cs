using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace ShopThoiTrangASP.Models
{
    [Table("OrderDetails")]
    public class OrdetDetail
    {
        [Key]
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }
        public double Amount { get; set; }

    }
}