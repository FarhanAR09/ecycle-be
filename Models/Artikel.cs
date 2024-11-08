namespace ecycle_be.Models
{
    public class Artikel
    {
        public int? ArtikelID
        {
            get; set;
        }
        public string? Judul
        {
            get; set;
        }
        public string? Konten
        {
            get; set;
        }

        //Foreign Keys
        public int? AdminID
        {
            get; set;
        }
    }
}
