using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopThoiTrangASP.Models
{
    public class ProductCategory
    {
        public int ID { get; set; }
        public int CatID { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Detail { get; set; }
        public string Metakey { get; set; }
        public string Metadesc { get; set; }
        public string Img { get; set; }
        public int Number { get; set; }
        public Double Price { get; set; }
        public Double? PriceSale { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Created_At { get; set; }
        public int? Updated_By { get; set; }
        public DateTime? Updated_At { get; set; }
        public int Status { get; set; }
        public string CatName { get; set; }

    }
}