using ETicaret.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.DataAccessLayer.Concrete.EfCore
{
    public class MyInitialData
    {
        private static Category[] Categories = new Category[5]
        {
            new Category() { Name="Telefon", Url="telefon" },
            new Category() { Name="Bilgisayar", Url="bilgisayar" },
            new Category() { Name="Elektronik", Url="elektronik" },
            new Category() { Name="Giyim", Url="giyim" },
            new Category() { Name="Beyaz Eşya", Url="beyaz-esya" }
        };

        private static Product[] Products = new Product[]
        {
            new Product() {Name="Samsung S6", Url="samsung-s6", Description="Samsung Telefonlar çok güzel.", ImageUrl="samsungS6.jpg", Price=100, IsApproved=true },
            new Product() {Name="Samsung S7", Url="samsung-s7", Description="Samsung Telefonlar çok güzel.", ImageUrl="samsungS7.jpg", Price=200, IsApproved=false },
            new Product() {Name="Iphone 6", Url="iphone-6", Description="Iphone Telefonlar çok daha güzel.", ImageUrl="iphone6.jpeg", Price=100, IsApproved=true },
            new Product() {Name="Iphone 11", Url="iphone-11", Description="Telefonlar çok güzel.", ImageUrl="iphone11.jpg", Price=1000, IsApproved=true },
            new Product() {Name="Iphone 14", Url="iphone-14", Description="Telefonlar çok güzel.", ImageUrl="iphone14.jpg", Price=100, IsApproved=true },
            new Product() {Name="Asus", Url="asus", Description="Asus bilgisayar çok güzel.", ImageUrl="asus.jpg", Price=2000, IsApproved=true },
            new Product() {Name="Toshiba A50", Url="toshiba-a50", Description="Toshiba notebook serisinin güzel bir bilgisayarı.", ImageUrl="toshiba.jpg", Price=2500, IsApproved=true },
            new Product() {Name="LG TV", Url="lg-tv", Description="Televizyon çok güzel.", ImageUrl="Lgtv.jpg", Price=15000, IsApproved=true },
            new Product() {Name="Mavi Jeans Gömlek", Url="mavi-jeans-gomlek", Description="Gömlekler", ImageUrl="gomlek.png", Price=100, IsApproved=true },
            new Product() {Name="Arçelik Buzdolabı", Url="arcelik-buzdolabi", Description="Az yakar çok soğutur..", ImageUrl="arcelik_buzdolabi.png", Price=100, IsApproved=true }
        };


        private static ProductCategory[] productCategories = new ProductCategory[] {
            new ProductCategory(){Product=Products[0],Category=Categories[0] },
            new ProductCategory(){Product=Products[1],Category=Categories[0] },
            new ProductCategory(){Product=Products[2],Category=Categories[0] },
            new ProductCategory(){Product=Products[3],Category=Categories[0] },
            new ProductCategory(){Product=Products[4],Category=Categories[0] },
            new ProductCategory(){Product=Products[5],Category=Categories[1] },
            new ProductCategory(){Product=Products[6],Category=Categories[1] },
            new ProductCategory(){Product=Products[7],Category=Categories[2] },
            new ProductCategory(){Product=Products[0],Category=Categories[2] },
            new ProductCategory(){Product=Products[1],Category=Categories[2] },
            new ProductCategory(){Product=Products[2],Category=Categories[2] },
            new ProductCategory(){Product=Products[3],Category=Categories[2] },
            new ProductCategory(){Product=Products[4],Category=Categories[2] },
            new ProductCategory(){Product=Products[5],Category=Categories[2] },
            new ProductCategory(){Product=Products[6],Category=Categories[2] }
        };

        public static void Seed()
        {
            ETicaretContext context = new ETicaretContext();
            if (context.Database.GetPendingMigrations().Count()==0)
            {
                if (context.Categories.Count() == 0)
                {
                    foreach (var cat in Categories)
                    {
                        context.Categories.Add(cat);
                    }
                }
                if (context.Products.Count() == 0)
                {
                    foreach (var prod in Products)
                    {
                        context.Products.Add(prod);
                    }
                    foreach (var prodCat in productCategories)
                    {
                        context.Add(prodCat);
                    }
                }
            }
            context.SaveChanges();
        }




    }
}
