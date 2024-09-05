namespace ecycle_be.Models
{
    public class Artikel
    {
        public int ArtikelID { get; set; } = -1;
        public string Judul { get; set; } = string.Empty;
        public string Konten { get; set; } = string.Empty;

        //Foreign Keys
        public int AdminID { get; set; } = -1;
    }
}
