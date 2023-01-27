using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.BusinessLayer.Abstract
{
    public interface IValidator<T>
    {
        // generic bir yapı oluşturuyorum çünkü hem product için hem de category için validation işlemlerinde kullanabilirim.
        string ErrorMessage { get; set; }   // hata mesajlarını burada burada tutacağız
        bool Validation (T entity);     // gönderilen entity'i validate etsin ve sonuç başarılı mı değil mi geriye true ya da false olarak                                  göndersin
    }
}
