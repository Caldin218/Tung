using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace ShopThoiTrangASP.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public int CatID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required]
        public string Detail { get; set; }
        [Required]
        public string Metakey { get; set; }
        [Required]
        public string Metadesc { get; set; }
        public string Img { get; set; }
        public Double OriginalPrice { get; set; }
        public int Number { get; set; }
        public Double Price { get; set; }
        public Double? PriceSale { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Created_At { get; set; }
        public int? Updated_By { get; set; }
        public DateTime? Updated_At { get; set; }
        public int Status { get; set; }
    }
}