using ETicaret.DataAccessLayer.Abstract;
using ETicaret.DataAccessLayer.Concreate.EfCore;
using ETicaret.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.DataAccessLayer.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category, ETicaretContext>, ICategoryRepository
    {
        public void DeleteFromCategory(int productId, int categoryId)
        {
            using(var context = new ETicaretContext())
            {
                // veritbanı işlemlerini Sql kodları yazarak yapmak istiyorsak aşağıdaki gibi bir yapı kullanabiliriz. önce query stringimi hazırlıyorum daha sonra da context üzerinden ilgili database'e giderek ExecuteSqlRaw() metodu ile sorgumu çalıştırabiliyorum.
                string cmd = "delete from ProductCategory where CategoryId=@p0 and ProductId=@p1";
                context.Database.ExecuteSqlRaw(cmd, categoryId, productId);
            }
        }

        public Category GetByIdWidthProducts(int categoryId)
        {
            using ( var context = new ETicaretContext())
            {
                return context.Categories
                    .Where(x=> x.Id == categoryId)
                    .Include(x=> x.ProductCategories)
                    .ThenInclude(x=> x.Product).FirstOrDefault();
            }
        }

        public List<Category> GetPopularCategories()
        {
            using (var context = new ETicaretContext())
            {
                return context.Categories.ToList();
            }
        }
    }
}
