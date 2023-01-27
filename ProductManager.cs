using ETicaret.BusinessLayer.Abstract;
using ETicaret.DataAccessLayer.Abstract;
using ETicaret.DataAccessLayer.Concrete.EfCore;
using ETicaret.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.BusinessLayer.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductRepository _productRepository;
        //aşağıdaki gibi new'leyerek kodu yazdığımızda ilgili sınıfa bağımlı hale geliyoruz. bu sorunu ortadan kaldırmak için bağımlılığı soyutlamamız gerekiyor. burada da interface'lerden faydalanıyoruz. yukarıdaki tanımlama ve aşağıdaki constructor ile DI'nı uygulamış olduk.

        //EfCoreProductRepository db = new EfCoreProductRepository();

        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public string ErrorMessage { get ; set; }

        public bool Create(Product product)
        {
            // iş kurallarının/denetimlerinin yapılacağı alan
            if(Validation(product))
            {
                _productRepository.Create(product);
                return true;
            }
            return false;
        }

        public void Delete(Product product)
        {
            _productRepository.Delete(product);
        }

        public List<Product> GetAll()
        {
           return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productRepository.GetByIdWithCategories(id);
        }

        public List<Product> GetHomePageProducts()
        {
            return _productRepository.GetHomePageProducts();
        }

        public List<Product> GetPopularProducts()
        {
            throw new NotImplementedException();
        }

        public List<Product> GetProductByCategory(string categoryName, int page, int pageSize)
        {
            return _productRepository.GetProductByCategory(categoryName, page, pageSize);
        }

        public Product GetProductDetails(string url)
        {
            return _productRepository.GetProductDetails(url);
        }

        public List<Product> GetSearchResult(string searchString)
        {
            return _productRepository.GetSearchResult(searchString);
        }

        public List<Product> GetTopFiveProducts()
        {
            throw new NotImplementedException();
        }

        public void Update(Product product)
        {
            _productRepository.Update(product);
        }

        public bool Update(Product product, int[] categoryIds)
        {
            if(Validation(product))
            {
                if(categoryIds.Length == 0)
                {
                    ErrorMessage += "Ürün için en az bir kategori seçmelisiniz.";
                    return false;
                }
                _productRepository.Update(product, categoryIds);
                return true;
            }            
            return false;
        }

        public bool Validation(Product entity)
        {
            bool isValid = true;
            if(string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "Ürün Adı girilmelidir\n";
                isValid = false;
            }
            if(string.IsNullOrEmpty(entity.Description))
            {
                ErrorMessage += "Ürün açıklaması girilmelidir\n";
                isValid |= false;
            }
            if(entity.Price < 0)
            {
                ErrorMessage += "Ürün fiyatı negatif bir değer olamaz";
                isValid &= false;
            }
            return isValid;
        }

        int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }

        int IProductService.GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }
    }
}
