using ETicaret.Entities;
using System.ComponentModel.DataAnnotations;

public class ProductModel
{
    public int Id { get; set; }

    [Display(Name = "Ürün Adı", Prompt = "Ürün Adını Giriniz")]
    //[Required(ErrorMessage = "Ürün Adı zorunlu bir alandır, boş geçilemez.")]
    //[StringLength(100, MinimumLength = 5, ErrorMessage = "Ürün Adı 5-100 karakter arasında olmalı.")]
    public string Name { get; set; }

    [Display(Name = "Ürün Fiyatı", Prompt = "Ürün Fiyatını Giriniz.")]
    //[Required(ErrorMessage = "Ürün Fiyatı zorunlu bir alandır, boş geçilemez.")]
    //[Range(1, 100000, ErrorMessage = "Ürün fiyatı için 1-100.000 arasında bir değer girmelisiniz.")]
    public double? Price { get; set; }

    [Display(Name = "Ürün Açıklaması", Prompt = "Ürün Açıklamasını Giriniz")]
    //[Required(ErrorMessage = "Ürün Açıklaması zorunlu bir alandır, boş geçilemez.")]
    //[StringLength(1000, MinimumLength = 5, ErrorMessage = "Ürün Açıklaması 5-1000 karakter arasında olmalı.")]
    public string Description { get; set; }

    [Display(Name = "Ürün Fotoğrafı", Prompt = "Ürün Fotoğrafını Giriniz")]
    [Required(ErrorMessage = "Ürün Fotoğrafı zorunlu bir alandır, boş geçilemez.")]
    public string ImageUrl { get; set; }

    [Display(Name = "Ürün Linki", Prompt = "Ürün Linkini Giriniz")]
    [Required(ErrorMessage = "Ürün Linki alanı zorunlu bir alandır, boş geçilemez.")]
    public string Url { get; set; }
    [Display(Name ="Onaylı Mı")]
    public bool IsApproved { get; set; }
    [Display(Name ="Anasayfada Gösterilsin Mi")]
    public bool IsHome { get; set; }

    public List<Category> SelectedCategories { get; set; }

    public ProductModel()
    {
        SelectedCategories = new List<Category>();
    }
}
