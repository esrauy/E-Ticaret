using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.BusinessLayer.Abstract
{
    public interface IProductService : IValidator<Product>
    {
        Product GetById(int id);
        List<Product> GetAll();
        bool Create(Product product);
        void Update(Product product);
        void Delete(Product product);
        List<Product> GetProductByCategory(string categoryName, int page, int pageSize);
        List<Product> GetTopFiveProducts();
        List<Product> GetPopularProducts();
        Product GetProductDetails(string url);
        int GetCountByCategory(string category);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResult(string searchString);
        Product GetByIdWithCategories(int id);
        bool Update(Product product, int[] categoryIds);
    }
}
