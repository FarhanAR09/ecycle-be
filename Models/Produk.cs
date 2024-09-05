namespace ecycle_be.Models
{
    public class Produk
    {
        public int ProdukID { get; set; } = -1;
        public string Nama { get; set; } = string.Empty;
        public string Deskripsi { get; set; } = string.Empty;
        public int Stok { get; set; } = 0;
        public int Harga { get; set; }
        public string Kategori { get; set; } = string.Empty;
        public string Bahan { get; set; } = string.Empty;

        //Foreign Keys
        public int PenggunaID { get; set; } = -1;
    }
}
