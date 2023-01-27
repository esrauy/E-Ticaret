using ETicaret.BusinessLayer.Abstract;
using ETicaret.Entities;
using ETicaret.WebUI.Identity;
using ETicaret.WebUI.Models;
using ETicaret.WebUI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ETicaret.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]   
    public class AdminController : Controller
    {
        private ICategoryService _categoryService;

        private IProductService _productService;
        
        private RoleManager<IdentityRole> _roleManager;

        private UserManager<User> _userManager;

        public AdminController(IProductService productService, ICategoryService categoryService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // role işlemleri
        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }

        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                if(result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var members = new List<User>();
            var nonMembers = new List<User>();
            foreach(var user in _userManager.Users.ToList())
            {
                // IsInRoleAsync(user, role.Name) metodu bir user nesnesi alır bir de role ismini alır ve veritabanından kontrol eder. Eğer ilgili role user'a atanmış ise True değerini döndürür. Atanmamış ise false değerini döndürür.

                // 1. yol
                //if(await _userManager.IsInRoleAsync(user, role.Name))
                //{
                //    members.Add(user);
                //}
                //else
                //{
                //    nonMembers.Add(user);
                //}

                // 2. yol
                // aşağıdaki satırda tanımladığım list değişkeninin referansını değiştiriyorum. IsInRoleAsync metodundan gelen True/False değerine göre list değişkenin referansı değişiyor. True ise list değişkeni members'ın referansına sahip oluyor. False ise nonMembers'ın referansına sahip oluyor
                // list.Add(User); satırı ile de user ilgili listenin içine eklenmiş oluyor.
                var list = await _userManager.IsInRoleAsync(user, role.Name)? members:nonMembers;
                list.Add(user);
            }
            var model = new RoleDetails()
            {
                Members = members,
                NonMembers = nonMembers,
                Role = role
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            ModelState.Remove("IdsToAddRole");
            ModelState.Remove("IdsRemoveFromRole");
            if(ModelState.IsValid)
            {
                // role eklenecek kullanıcılar
                foreach(var userId in model.IdsToAddRole?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if(user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if(!result.Succeeded)
                        {
                            foreach(var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }                           
                        }
                    }
                }
                // user'ı role'den silmek için
                foreach (var userId in model.IdsRemoveFromRole ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
            }
            return Redirect("/admin/role/" + model.RoleId);
        }

        // erişimin olmadığı durumlarda kullanılacak AccessDenied Action'ı
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // user işlemleri
        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }

        public IActionResult UserCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UserCreate(UserDetailsModel model)
        {
            return View();
        }
        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(x=> x.Name);
                ViewBag.Roles = roles;
                return View(new UserDetailsModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles
                });
            }
            return Redirect("/admin/user/list");
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user); // identity içerisindeki metotlardan. User nesnesini                                                                verince, ilgili user'a ait role'leri getiriyor.
                        selectedRoles = selectedRoles??new string[] {};
                        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());
                        // Except ile selectedRoles içinden kullanıcının sahip olduğu roller çıkarılıyor ve kalanlar veritabanına ekleniyor. Yani yeni eklenen roller bu kullanıcı için ilgili tabloya eklenmiş oluyor.

                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());
                        // burada da yukarıdaki işlemin tam tersi bir işlem yapılıyor. user rolleri içerisinden selectedRole'ler çıkarılıyor ve kalanlar siliniyor.

                        return Redirect("/admin/user/list");
                    }
                }
                return Redirect("/admin/user/list");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult UserDelete(string id)
        {
            return View();
        }

        // product işlemleri
        public IActionResult ProductList()
        {
            var productListViewModel = new ProductListViewModel()
            {
                Products = _productService.GetAll()
            };
            return View(productListViewModel);
        }
        [HttpGet]
        public IActionResult CreateProduct()
        {
            // boş bir form gönderecek
            return View();
        }
        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                // formu doldurduktan sonra burada da kayıt işlemini gerçekleştireceğiz.
                // boş product nesnesi oluşturulacak(entity'deki product classından).
                Product product = new Product();
                // parametredeki model nesnesi içindeki veriler product nesnesi içine aktarılır.
                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.Url = model.Url;
                product.ImageUrl = model.ImageUrl;

                // verileri alan product nesnesi veritabanına kaydedilmek için ilgili metoda parametre olarak verilir
                if(_productService.Create(product))
                {
                    CreateMessage("Kayıt Eklendi!", "success");
                    return RedirectToAction("ProductList");
                }               
                CreateMessage(_productService.ErrorMessage, "danger");
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditProduct(int? id)
        {
            // ilgili ürünü edit sayfasına göndereceğiz
            if (id == null)
            {
                return NotFound();
            }
            Product entity = _productService.GetByIdWithCategories((int)id);
            if (entity == null)
            {
                return NotFound();
            }
            ProductModel model = new ProductModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Url = entity.Url,
                ImageUrl = entity.ImageUrl,
                Price = entity.Price,
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome,
                SelectedCategories = entity.ProductCategories.Select(c => c.Category).ToList()
            };
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }
        [HttpPost]
        public IActionResult EditProduct(ProductModel model, int[] categoryIds, IFormFile? file)
        {
            // fotoğrafı yakalamak için parametreye IFormFile ekliyoruz.
            //validation işlemi yapıldıktan sonra kurala uymayan veri olup olmadığının kontrolünü ModelState.Isvalid ile yapıyoruz.
            //hata mesajlarını da ilgili cshtml'de asp-validation-for ile ekrana yazdırıyoruz.
            if (ModelState.IsValid)
            {
                // formda güncellenen ürün bilgisini veritabanına kaydedeceğiz
                //veritabanından ilgili kayıt alınır
                Product product = _productService.GetById(model.Id);

                //model içindeki veriler güncellenen veriler olduğu için veritabanından gelen product nesnesinin içine yerleştirilir
                if (product == null)
                {
                    return NotFound();
                }
                product.Name = model.Name;
                product.Description = model.Description;
                product.Url = model.Url;
                //product.ImageUrl = model.ImageUrl;
                product.Price = model.Price;
                product.IsApproved = model.IsApproved;
                product.IsHome = model.IsHome;

                //değiştirilen product nesnesi update metodu ile veritabanına gönderilir ve güncelleme işlemi biter.

                if (file != null)
                {
                    // parametredeki file null gelmediyse yani dosya, bu durumda dosyaya eşsiz bir isim ile wwwroot/images altına kaydetmemiz gerekecek.
                    // aşağıdaki kod ile dosyanın uzantısını alıyoruz.
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    //aşağıda dosyayı kadetmek için kullanacağımız isme eşsiz bir isim yaratıyoruz. Guid'den faydalanıyoruz.
                    var randomName = string.Format($"{Guid.NewGuid()} {extension}");
                    // product içindeki alana oluşturduğumuz dosya adını yazdırıyoruz.
                    product.ImageUrl = randomName;
                    // dosyanın kaydedileceği path'i ve dosya adını veriyoruz.
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", randomName);
                    // aşağıdaki satırda da dosya adı ve yolu verilen yere fotoğrafı kaydediyoruz.
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                if (_productService.Update(product, categoryIds))
                {
                    CreateMessage("Ürün güncellendi.", "success");
                    return RedirectToAction("ProductList");
                }    
                CreateMessage(_productService.ErrorMessage, "danger");
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            // id'si gönderilen ürünü sileceğiz.
            //parametredeki Id kullanılarak veritabanından ilgili product nesnesi alınır
            Product product = _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            //product nesnesi delete metoduna parametre olarak verilerek silme işlemi veritabanından yapılır.
            _productService.Delete(product);
            var msg = new AlertMessage()
            {
                Message = $"{product.Name} isimli ürün silindi.",
                AlertType = "danger"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("ProductList");
        }

        public IActionResult CategoryList()
        {
            var categoryListViewModel = new CategoryListViewModel()
            {
                Categories = _categoryService.GetAll()
            };
            return View(categoryListViewModel);
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            // boş bir form gönderecek
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            ModelState.Remove("Products");
            if (ModelState.IsValid)
            {
                Category category = new Category();
                category.Name = model.Name;
                category.Url = model.Url;

                _categoryService.Create(category);
                var msg = new AlertMessage()
                {
                    Message = $"{category.Name} isimli kategori eklendi.",
                    AlertType = "success"
                };
                TempData["message"] = JsonConvert.SerializeObject(msg);
                return RedirectToAction("CategoryList");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditCategory(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Category entity = _categoryService.GetByIdWidthProducts((int)id);
            if (entity == null)
            {
                return NotFound();
            }
            CategoryModel model = new CategoryModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories.Select(p => p.Product).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult EditCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                Category category = _categoryService.GetById(model.Id);

                if (category == null)
                {
                    return NotFound();
                }
                category.Name = model.Name;
                category.Name = model.Name;
                category.Url = model.Url;

                _categoryService.Update(category);
                var msg = new AlertMessage()
                {
                    Message = $"{category.Name} isimli kategori güncellendi.",
                    AlertType = "success"
                };
                TempData["message"] = JsonConvert.SerializeObject(msg);
                return RedirectToAction("CategoryList");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            Category category = _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            _categoryService.Delete(category);
            var msg = new AlertMessage()
            {
                Message = $"{category.Name} isimli kategori silindi.",
                AlertType = "danger"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("CategoryList");
        }

        public IActionResult DeleteFromCategory(int productId, int categoryId)
        {
            _categoryService.DeleteFromCategory(productId, categoryId);
            return Redirect("/admin/categories/" + categoryId);
        }

        private void CreateMessage(string message, string alertType)
        {
            var msg = new AlertMessage()
            {
                Message = message,
                AlertType = alertType
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
        }
    }
}
