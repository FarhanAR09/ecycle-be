namespace ecycle_be.Models
{
    public class Review
    {
        public int ReviewId { get; set; } = -1;
        public int Rating { get; set; }
        public string Komentar { get; set; } = string.Empty;
        public DateTime Tanggal { get; set; }

        //Foreign Keys
        public int ProdukID { get; set; } = -1;
        public int PenggunaId { get; set; } = -1;
    }
}
