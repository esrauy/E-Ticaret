using System.ComponentModel.DataAnnotations;

namespace ETicaret.WebUI.Models
{
    public class LoginModel
    {
        //[Required]
        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }
        [Required]
        [Display(Name = "Kullanıcı Adınız")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifreniz")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }


    }
}
