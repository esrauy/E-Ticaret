using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Entities
{
    public class ProductCategory
    {
        // Bu Class'ın amacı bir product birden çok kategoriye karşılık gelebilir. Aynı zamanda bir kategory birden çok Product'a karşılık gelebilir. Yani Çoktan çoğa bir ilişki söz konusu. Bunu da ilişki kurulacak Class'rın dışında üçüncü bir class tanımlyarak oluşturabiliyoruz.

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
