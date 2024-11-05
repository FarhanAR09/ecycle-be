using ecycle_be.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace ecycle_be.Services
{
    public class ProductService(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<List<Produk>> GetProductsThumbnails()
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Failed to connect to the database.");
            }

            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                const string query = "SELECT \"produkID\", \"nama\", \"harga\" FROM \"Produk\";";

                using var command = new NpgsqlCommand(query, connection);

                using var reader = await command.ExecuteReaderAsync();

                List<Produk> products = [];
                while (await reader.ReadAsync())
                {
                    products.Add(new Produk()
                    {
                        ProdukID = reader.GetInt32(reader.GetOrdinal("produkID")),
                        Nama = reader.GetString(reader.GetOrdinal("nama")),
                        Harga = reader.GetDouble(reader.GetOrdinal("harga")),
                    });
                }

                return products;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception("Failed to connect to the database.", ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public class DetailedUIProduk
        {
            public int ProdukID { get; set; } = -1;
            public string Nama { get; set; } = string.Empty;
            public string Deskripsi { get; set; } = string.Empty;
            public int Stok { get; set; } = 0;
            public int Terjual { get; set; } = 0;
            public double Harga { get; set; } = 0;
            public double OngkosKirim { get; set; } = 0;
            public int KategoriID { get; set; } = 0;

            //Foreign Fields
            public string NamaPenjual { get; set; } = string.Empty;
            public string Alamat { get; set; } = string.Empty;
            public int BahanID { get; set; } = -1;
        }

        public async Task<DetailedUIProduk> GetById(int id)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Failed to connect to the database.");
            }

            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                const string query = "SELECT \"Produk\".\"produkID\", \"Produk\".\"nama\", \"Produk\".\"deskripsi\", \"Produk\".\"harga\", \"Produk\".\"ongkosKirim\", \"Produk\".\"stok\", \"Produk\".\"terjual\", \"Produk\".\"kategoriID\", \"Produk\".\"bahanID\", \"Pengguna\".\"username\" as \"namaPenjual\", \"Pengguna\".\"alamat\"\r\n\tFROM \"Produk\" LEFT JOIN \"Pengguna\"\r\n\t\tON \"Produk\".\"penjualID\" = \"Pengguna\".\"penggunaID\"\r\n\tWHERE \"Produk\".\"produkID\" = @produkID LIMIT 1;";

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@produkID", id);

                using var reader = await command.ExecuteReaderAsync();

                if (!reader.HasRows)
                    throw new Exception($"ProdukID {id} not found");

                await reader.ReadAsync();

                DetailedUIProduk product = new()
                {
                    ProdukID = reader.GetInt32(reader.GetOrdinal("produkID")),
                    Nama = reader.GetString(reader.GetOrdinal("nama")),
                    Deskripsi = reader.GetString(reader.GetOrdinal("deskripsi")),
                    Stok = reader.GetInt32(reader.GetOrdinal("stok")),
                    Terjual = reader.GetInt32(reader.GetOrdinal("terjual")),
                    Harga = reader.GetDouble(reader.GetOrdinal("harga")),
                    OngkosKirim = reader.GetDouble(reader.GetOrdinal("ongkosKirim")),
                    KategoriID = reader.GetInt32(reader.GetOrdinal("kategoriID")),
                    NamaPenjual = reader.GetString(reader.GetOrdinal("namaPenjual")),
                    Alamat = reader.GetString(reader.GetOrdinal("Alamat")),
                    BahanID = reader.GetInt32(reader.GetOrdinal("bahanID"))
                };

                return product;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception("Failed to connect to the database.", ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task PostProduk()
        {

        }
    }
}
