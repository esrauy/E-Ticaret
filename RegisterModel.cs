using System.ComponentModel.DataAnnotations;

namespace ETicaret.WebUI.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Adınız")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Soyadınız")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Kullanıcı Adınız")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifreniz")]
        public string Password { get; set; }      
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Şifre Tekrar")]
        public string RePassword { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Posta")]
        public string Email { get; set; }

    }
}
