using ETicaret.BusinessLayer.Abstract;
using ETicaret.DataAccessLayer.Abstract;
using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.BusinessLayer.Concrete
{
    public class CartManager : ICartService
    {
        private ICartRepository _cartRepository;
        public CartManager(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public void AddToCart(string userId, int productId, int quantity)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                // 1. eklenmek istenen ürün sepette var mı?
                // eğer sepette ise ürün adedini 1 ya da istenilen miktarda arttır (update).
                // eğer sepette yoksa o zaman yeni bir kayıt oluştur (create).

                var index = cart.CartItems.FindIndex(x => x.ProductId == productId);
                if (index < 0)
                {
                    // create işlemi yapılacak
                    cart.CartItems.Add(new CartItem()
                    {
                        ProductId = productId,
                        CartId = cart.Id,
                        Quantity = quantity
                    });
                }
                else
                {
                    // update işlemi yapılacak
                    cart.CartItems[index].Quantity += quantity;
                }
                _cartRepository.Update(cart);
            }
        }

        public void ClearCart(int cartId)
        {
            _cartRepository.ClearCart(cartId);
        }

        public void DeleteFromCart(string userId, int productId)
        {
            var cart = GetCartByUserId(userId);
                if (cart != null)
            {
                _cartRepository.DeleteFromCart(cart.Id, productId);
            }
        }

        public Cart GetCartByUserId(string userId)
        {
            return _cartRepository.GetByUserId(userId);
        }

        public void InitializerCart(string userId)
        {
            _cartRepository.Create(new Cart() { UserId = userId });
        }
    }
}
