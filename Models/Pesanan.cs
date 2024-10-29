namespace ecycle_be.Models
{
    public class Pesanan
    {
        public int PesananID { get; set; } = -1;
        public int Stok { get; set; } = -1;

        //Foreign Keys
        public int ProdukID { get; set; } = -1;
        public int StatusPesananID { get; set; } = -1;
        public int PenjualID { get; set; } = -1;
        public int PembeliID { get; set; } = -1;
    }
}
