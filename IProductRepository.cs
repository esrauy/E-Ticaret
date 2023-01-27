using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.DataAccessLayer.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        List<Product> GetProductByCategory(string categoryName, int page, int pageSize);
        List<Product> GetTopFiveProducts();
        List<Product> GetPopularProducts();
        Product GetProductDetails(string url);
        int GetCountByCategory(string category);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResult(string searchString);
        Product GetByIdWithCategories(int id);
        void Update(Product product, int[] categoryIds);
    }
}
