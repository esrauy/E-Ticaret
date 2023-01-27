using ETicaret.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
namespace ETicaret.WebUI.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private ICategoryService _categoryService;
        public CategoriesViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IViewComponentResult Invoke()
        {
            //burada ilgili kodların olması gerekiyor. örneğin veritabanından istenen verilerin alınması gibi.
            //if(RouteData.Values["category"] != null)
            //{
            //    ViewBag.SelectCategory = RouteData?.Values["category"];
            //}
            return View(_categoryService.GetAll());
        }
    }
}

/*
  ViewComponent nedir?
PartialView'de bir data kullanılacaksa bu data kesinlikle Controller'dan gelmek zorunda. Başka yolu yok.
ComponentView ise Controller'ı devreden çıkarır ve kendisine ait olan (yani bu class CategoriesViewComponent) sınıfa gider verisini alır ve View'inde kullanır bu datayı.
Öncelikle cs kısmını yazın (yani bu dosya). WebUI katmanı içinde ViewComponents isimli(bu isim zorunlu değil) bir klasör oluşturun. İçinde de bir class oluşturuyoruz (CategoriesViewComponent sınıfı). Class'a bir isim veriyoruz(Categories), ismin sonuna da ViewComponent ekliyoruz.
Daha sonra bu sınıfı ViewComponent'ten türetiyoruz. Yani bu sınıfın miras almasını sağlıyoruz (using Microsoft.AspNetCore.Mvc; altında bulunan bir sınıftır).
ViewComponent'in tetiklenmesi için de aşağıdaki metodu barındırması gerekiyor. Bu bir kuraldır.
      
       public IViewComponentResult Invoke()
        {
            //burada ilgili kodların olması gerekiyor. örneğin veritabanından istenen verilerin alınması gibi.

            return View(_categoryService.GetAll());
        }
İlgili view nerede konumlandırılacak?
 - 2 farklı yerde konumlandırılabilir.
    1- view klasörü altında ilgili Controller klasörünün altında Components klasörü olmalı. Bu klasörün altında da bu sınıf ismi ile aynı ada sahip bir klasör daha oluşturmalıyız. bu klasörün içinde ilgili view olmalı.
View/Home/Components/Categories/Default.cshtml
    2- Shared klasörü altında Components altında ve class ismi ile oluşturulan klasörün içinde barındırmamız gerekiyor.
View/Shared/Components/Categories/Default.cshtml

View ismi ne olmalı?
Eğer farklı bir isim vermeyeceksek Default ismini kullanabiliriz. Bu durumda bunu return View() kısmında parametre olarak vermemize gerek yok.
Farklı bir isim vereceksek bu durumda bu ismi Invoke() metodu içindeki View içinde belirtmemiz gerekecek.

ViewComponent'i kullanmak istediğimizde de ilgili View içinde aşağıdaki gibi kullanabiliriz.
@await Component.InvokeAsync("Categories");
ya da
@Component Invoke("Categories");
 */
