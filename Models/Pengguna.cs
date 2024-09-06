namespace ecycle_be.Models
{
    public class Pengguna
    {
        public int PenggunaID { get; set; } = -1;
        public string Nama { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Alamat { get; set; } = string.Empty;
        public string Telepon { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
