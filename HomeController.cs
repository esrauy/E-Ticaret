using ETicaret.BusinessLayer.Abstract;
using ETicaret.BusinessLayer.Concrete;
using ETicaret.WebUI.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Controllers
{
    public class HomeController : Controller
    {
       private IProductService _productService;
        public HomeController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            var productListViewModel = new ProductListViewModel()
            {
                Products = _productService.GetHomePageProducts()
            };
            return View(productListViewModel);
        }
        public IActionResult About()
        {
           
            return View();
        }
        public IActionResult Contact()
        {
           
            return View();
        }
    }
}
