namespace ecycle_be.Models
{
    public class PreferensiBeli
    {
        public int PreferensiID { get; set; } = -1;

        //Foreign Keys
        public int PenggunaID { get; set; } = -1;
        public int BahanID { get; set; } = -1;
    }
}
