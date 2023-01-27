using ETicaret.BusinessLayer.Abstract;
using ETicaret.Entities;
using ETicaret.WebUI.Models;
using ETicaret.WebUI.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;
        public ShopController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult List(string category, int page=1)  //kullanıcıdan sayfa bilgisi gelmezse burada bir exception oluşacak program patlayacak. bunun için page parametresine default olarak 1 değeri veriyorum.
        {
            const int pageSize = 3; //sayfada kaç ürün göstereceksem bunun sayısını bu değişkende tutuyorum.
            var productListViewModel = new ProductListViewModel()
            {
                //Products = _productService.GetAll()
                PageInfo = new PageInfo { TotalItems = _productService.GetCountByCategory(category), 
                CurrentPage = page, 
                ItemsPerPage = pageSize, 
                CurrentCategory = category},
                Products = _productService.GetProductByCategory(category, page, pageSize)
            };
            return View(productListViewModel);
        }
        public IActionResult Details(string url)
        {
            if(url == null)
            {
                return NotFound();
            }

            Product product = _productService.GetProductDetails(url);
            if(product == null)
            {
                return NotFound();
            }
            return View(new ProductDetailModel()
            {
                Product = product,
                Categories = product.ProductCategories.Select(x=> x.Category).ToList()
            });
        }

        public IActionResult Search(string search)
        {
            var productListViewModel = new ProductListViewModel()
            {
                Products = _productService.GetSearchResult(search)
            };
             return View(productListViewModel);
        }
    }
}
