using ETicaret.BusinessLayer.Abstract;
using ETicaret.BusinessLayer.Concrete;
using ETicaret.DataAccessLayer.Abstract;
using ETicaret.DataAccessLayer.Concrete.EfCore;
using ETicaret.WebUI.EmailServices;
using ETicaret.WebUI.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(option => option.UseSqlServer("Server=DESKTOP-HNE43R2;Database=DbETicaretCore;Integrated Security=true;"));

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

//identity ile ilgili bir takým özelliklerin konfigürasyonunu aþaðýdaki gibi yapabiliriz.
builder.Services.Configure<IdentityOptions>(options =>
{
    //password
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 5;
    
    //lockout
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

builder.Services.ConfigureApplicationCookie(options => {
     options.LoginPath = "/account/login";
     options.LogoutPath = "/account/logout";
     options.AccessDeniedPath = "/admin/accessdenied";
     options.SlidingExpiration = true;
     options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

     options.Cookie = new CookieBuilder { HttpOnly = true, Name = ".ETicaret.Security.Cookie" };
});

//IoC: Inversion of Control
builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IProductRepository, EfCoreProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
builder.Services.AddScoped<ICartRepository, EfCoreCartRepository>();
builder.Services.AddScoped<ICartService, CartManager>();
builder.Services.AddScoped<IOrderRepository, EfCoreOrderRepository>();
builder.Services.AddScoped<IOrderService, OrderManager>();

builder.Services.AddScoped<IEmailSender, EmailSender>(m => new EmailSender("smtp.office365.com", 587, true, "dilan_bulak1999@hotmail.com", "48025908444"));

// Add services to the container.
//builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    MyInitialData.Seed();
}

app.UseHttpsRedirection();
app.UseStaticFiles();   // static dosyalara eriþebilmek için bu satýrýn mutlaka eklenmesi gerekiyor. static dosyalarýmýz da wwwroot altýnda bulunan dosyalarýmýz oluyor. wwwroot altýndaki dosyalara eriþebilmek için Örneðin images kalsörünün altýndaki fotoðraflara eriþmek istiyorsam
                        // ~/images/ornek.jpg
                        //~/css/css_dosyasýnýn_ismi

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

//app.MapRazorPages();
//app.MapDefaultControllerRoute();

// sipariþler
app.MapControllerRoute(
    name: "orders",
    pattern: "orders",
    defaults: new { controller = "cart", action = "GetOrders" }
    );

// sepet iþlemleri
app.MapControllerRoute(
    name: "cart",
    pattern: "cart",
    defaults: new { controller = "cart", action = "Index" }
    ); 

app.MapControllerRoute(
    name: "completeshopping",
    pattern: "completeshopping",
    defaults: new { controller = "cart", action = "CompleteShopping" }
    );

//admin user için route tanýmlarý

app.MapControllerRoute(
    name: "adminusercreate",
    pattern: "/admin/user/create",
    defaults: new { controller = "admin", action = "UserCreate" }
    );

app.MapControllerRoute(
    name: "adminuserlist",
    pattern: "/admin/user/list",
    defaults: new { controller = "admin", action = "UserList" }
    );

app.MapControllerRoute(
    name: "adminuseredit",
    pattern: "/admin/user/{id}",
    defaults: new { controller = "admin", action = "UserEdit" }
    );


//role için route tanýmlarý
app.MapControllerRoute(
    name: "adminrolelist",
    pattern: "/admin/role/list",
    defaults: new { controller = "admin", action = "RoleList" }
    );

app.MapControllerRoute(
    name: "adminrolecreate",
    pattern: "/admin/role/create",
    defaults: new { controller = "admin", action = "RoleCreate" }
    );

app.MapControllerRoute(
    name: "adminroleedit",
    pattern: "/admin/role/{id?}",
    defaults: new { controller = "admin", action = "RoleEdit" }
    );

app.MapControllerRoute(
    name: "register",
    pattern: "register",
    defaults: new { controller = "account", action = "register" }
    );

app.MapControllerRoute(
    name: "login",
    pattern: "login",
    defaults: new { controller = "account", action = "login" }
    );

app.MapControllerRoute(
    name: "logout",
    pattern: "logout",
    defaults: new { controller = "account", action = "logout" }
    );

// admin category list
app.MapControllerRoute(
    name: "admincategorylist",
    pattern: "/admin/categories",
    defaults: new { controller = "admin", action = "categorylist" }
    );

// admin category create
app.MapControllerRoute(
    name: "admincategorycreate",
    pattern: "/admin/categories/create",
    defaults: new { controller = "admin", action = "createcategory" }
    );

// admin category edit
app.MapControllerRoute(
    name: "admincategoryedit",
    pattern: "/admin/categories/{id?}",
    defaults: new { controller = "admin", action = "editcategory" }
    );


// admin product create
app.MapControllerRoute(
    name: "adminproductcreate",
    pattern: "admin/products/create",
    defaults: new { controller = "admin", action = "createproduct" }
    );

//admin product edit
app.MapControllerRoute(
    name: "adminproductedit",
    pattern: "admin/products/{id}",
    defaults: new { controller = "admin", action = "editproduct" }
    );


//admin product list
app.MapControllerRoute(
    name: "adminproductlist",
    pattern: "/admin/products",
    defaults: new { controller = "admin", action = "productlist" }
    );

//search
app.MapControllerRoute(
    name: "search",
    pattern: "search",
    defaults: new { controller = "shop", action = "search" }
    );

//localhost/about
app.MapControllerRoute(
    name: "products",
    pattern: "about",
    defaults: new { controller = "home", action = "about" }
    );

//localhost/contact
app.MapControllerRoute(
    name: "products",
    pattern: "contact",
    defaults: new { controller = "home", action = "contact" }
    );

//domain/products
app.MapControllerRoute(
    name: "products",
    pattern: "products/{category?}",
    defaults: new { controller="shop", action="list"}
    );

//aþaðýdaki link route'u ürün ayrýntýsýný listelemek için kullanýlacak.
//domain/samsung-s6
app.MapControllerRoute(
    name: "productdetails",
    pattern: "{url}",
    defaults: new { controller = "shop", action = "details" }
    );

// domain/Home/Index/5
app.MapControllerRoute(
    name:"default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.Run();
