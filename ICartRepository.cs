using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.DataAccessLayer.Abstract
{
    public interface ICartRepository : IRepository<Cart>
    {
        void DeleteFromCart(int cartId, int productId);
        Cart GetByUserId(string userId);
        void ClearCart(int cartId);
    }
}
