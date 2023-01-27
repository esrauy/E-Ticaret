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
    public class EfCoreCartRepository : EfCoreGenericRepository<Cart, ETicaretContext>, ICartRepository
    {
        public void ClearCart(int cartId)
        {
            using (var context = new ETicaretContext())
            {
                var queryString = $"delete from CartItems where CartId=@p0";
                context.Database.ExecuteSqlRaw(queryString, cartId);
            }
        }

        public void DeleteFromCart(int cartId, int productId)
        {
            using (var context = new ETicaretContext())
            {
                var queryString = $"delete from CartItems where CartId=@p0 and ProductId=@p1";
                context.Database.ExecuteSqlRaw(queryString, cartId, productId);
            }
        }

        public Cart GetByUserId(string userId)
        {
            using (var context = new ETicaretContext())
            {
                return context.Carts.Include(x=> x.CartItems).ThenInclude(x=> x.Product).FirstOrDefault(u=> u.UserId == userId);
            }
        }

        public override void Update(Cart entity)
        {
            using (var context = new ETicaretContext())
            {
                context.Carts.Update(entity);
                context.SaveChanges();
            }
        }
    }
}
