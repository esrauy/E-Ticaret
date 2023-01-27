using Microsoft.AspNetCore.Identity;

namespace ETicaret.WebUI.Identity
{
    public class User : IdentityUser
    {
        // aşağıdaki kolonların dışında ekstra alanlar IdentityUser içinden gelecek. Bunlar User için kullanacağımız gerekli olan alanlar..
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
