namespace ETicaret.WebUI.ViewModel
{
    public class PageInfo
    {
        public int TotalItems { get; set; }     //toplam ürün sayısını tutacak
        public int ItemsPerPage { get; set; }   //her sayfada kaç veri tutulacak
        public int CurrentPage { get; set; }    //kaçıncı sayfadayız bilgisini tutacak
        public string CurrentCategory { get; set; } //linkte kategori var mı yok mu bilgisini tutacağız
        public int TotalPage()
        {
            return (int)Math.Ceiling((decimal)TotalItems/ItemsPerPage);   // 10/3
        }
    }
}
