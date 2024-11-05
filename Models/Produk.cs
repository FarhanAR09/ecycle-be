namespace ecycle_be.Models
{
    public class Produk
    {
        public int ProdukID { get; set; } = -1;
        public string Nama { get; set; } = string.Empty;
        public string Deskripsi { get; set; } = string.Empty;
        public int Stok { get; set; } = 0;
        public int Terjual { get; set; } = 0;
        public double Harga { get; set; } = 0;
        public double OngkosKirim { get; set; } = 0;
        public int KategoriID { get; set; } = 0;

        //Foreign Keys
        public int PenjualID { get; set; } = -1;
        public int BahanID { get; set; } = -1;
    }
}
