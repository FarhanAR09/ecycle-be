namespace ecycle_be.Models
{
    public class Produk
    {
        public int? ProdukID { get; set; }
        public string? Nama { get; set; }
        public string? Deskripsi { get; set; }
        public int? Stok { get; set; }
        public int? Terjual { get; set; }
        public double? Harga { get; set; }
        public double? OngkosKirim { get; set; }
        public int? KategoriID { get; set; }

        //Foreign Keys
        public int? PenjualID { get; set; }
        public int? BahanID { get; set; }
    }
}
