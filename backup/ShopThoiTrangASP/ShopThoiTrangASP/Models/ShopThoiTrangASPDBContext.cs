using ShopThoiTrangASP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShopThoiTrangASP.Models
{
    public class ShopThoiTrangASPDBContext : DbContext //Kế thừa
    {
        public ShopThoiTrangASPDBContext() : base("name=ChuoiKN") { }                //Hàm tạo
                                                                                      //Ánh xạ các thực thể với CSDL

        public DbSet<Category> Categorys { get; set; } //Categorys: Đây là gọi theo tên Table đã tạo trước đó ở model đó
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrdetDetail> OrderDetails { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<User> Users { get; set; }
    }
}