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
    public class EfCoreOrderRepository : EfCoreGenericRepository<Order, ETicaretContext>, IOrderRepository
    {
        public List<Order> GetOrders(string userId)
        {
            using (var context = new ETicaretContext())
            {
                var orders = context.Orders.Include(x=> x.OrderItems).ThenInclude(p=> p.Product).Where(u=> u.UserId==userId).ToList();
                return orders;
            }
        }
    }
}
