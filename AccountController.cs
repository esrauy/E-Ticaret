using ETicaret.BusinessLayer.Abstract;
using ETicaret.WebUI.EmailServices;
using ETicaret.WebUI.Identity;
using ETicaret.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace ETicaret.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _emailSender;
        private ICartService _cartService;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ICartService cartService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _cartService = cartService;
        }
        public IActionResult Login(string ReturnUrl = null)
        {
            return View(new LoginModel() { ReturnUrl = ReturnUrl});
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            ModelState.Remove("ReturnUrl");
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            //var user = await _userManager.FindByEmailAsync(model.Email); // giriş işlemini email ile yapacaksak bu şekilde sorgulama yapmamız gerekiyor.
            var user = await _userManager.FindByNameAsync(model.UserName); // giriş işlemini username ile yapıyoruz bu yüzden aşağıdaki kodlar ile kullanıcı adı sorgulaması yaptık.
            if(user == null)
            {
                ModelState.AddModelError("", "Bu kullanıcı adı ile daha önce bir hesap oluşturulmuş.");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
            // PasswordSignInAsync 4 farklı parametre alır. ilk parametre User nesnesi, 2. parametre password bilgisi(modelden yani kullanıcıdan aldığımız password'ü gönderiyoruz.), 3. parametre isPersistent=false tarayıcı kapandığında ya da 30 dakikalık (program.cs içinde verdiğimiz süre) süre bittiğinde cookie silinir ve otomatik logout işlemi yapılır. 4. parametre lockOutOnFailure=false belirli bir sayıda yanlış giriş yapılırsa true olduğunda hesabı kilitler; false olduğunda kilitlemez.
            if(result.Succeeded)
            {               
                //return RedirectToAction("Index", "Home");
                return Redirect(model.ReturnUrl??"~/");
                // 2 soru işareti ile null kontrolü yapılıyor. Eğer model.ReturnUrl null ise ~/ linkine yani roota gidiyor. içinde bir değer yani link varsa, bu durumda linke gidiyor.
            }
            ModelState.AddModelError("","Girilen kullanıcı adı ya da şifre hatalı.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();    // Logout işlemi için _signInManager içindeki SignOutAsync() metodu logout işlemini                                         yapıyor.
            //return RedirectToAction("Index", "Home");
            return Redirect("~/");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                // token yaratılacak
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // url oluşturuldu
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });
                // mail gönderilecek
                //var _emailSender = new EmailSender("smtp.office365.com", 587, true, "dilan_bulak1999@hotmail.com", "48025908444");
                await _emailSender.SendEmailAsync(model.Email, "Eticaret sitesi için üyeliğinizi onaylayın.", $"Lütfen email hesabınızı doğrulamak için linke <a href='https://localhost:7099{url}'> tıklayınız</a>");
                return RedirectToAction("Login");
            }
            ModelState.AddModelError("", "Bir hata oluştu lütfen tekrar deneyin.");
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            // kullanıcı mailindeki linkten gelen userId ve token kontrol edilir. null gelirse hata mesajı ekranda gösterilecek.
            if(userId == null || token == null)
            {
                // ekranda hata mesajı göster
                CreateMessage("Geçersiz userId ya da token", "danger");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                // yukarıdaki satırda verilen değerler ile veritabanından eşleştirme yapıyor.
                if(result.Succeeded)
                {
                    _cartService.InitializerCart(user.Id);
                    // mesaj gösterilecek
                    CreateMessage("Hesabınız Onaylandı", "success");
                    return View();
                }
            }
            CreateMessage("Hesabınız onaylanmadı.", "danger");
            return View();
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
