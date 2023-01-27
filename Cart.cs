using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        // ilişkiler
        public List<CartItem> CartItems { get; set; }

    }
}
