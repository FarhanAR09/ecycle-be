using ecycle_be.Models;
using ecycle_be.Services;
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

        public async Task<Produk> PostProduk(Produk produk)
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

                const string query = "INSERT INTO \"Produk\" (\"nama\", \"deskripsi\", \"harga\", \"stok\", \"penjualID\", \"kategoriID\", \"bahanID\")\r\n\tVALUES (@nama, @desc, @harga, @stok, @penjualID, 0, 0) RETURNING *;";

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@nama", produk.Nama ?? "");
                command.Parameters.AddWithValue("@desc", produk.Deskripsi ?? "");
                command.Parameters.AddWithValue("@harga", produk.Harga ?? 0);
                command.Parameters.AddWithValue("@stok", produk.Stok ?? 0);
                command.Parameters.AddWithValue("@penjualID", produk.PenjualID ?? 0);

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    produk.ProdukID = reader.GetInt32(reader.GetOrdinal("produkID"));
                    produk.Nama = reader.GetString(reader.GetOrdinal("nama"));
                    produk.Deskripsi = reader.GetString(reader.GetOrdinal("deskripsi"));
                    produk.Harga = reader.GetDouble(reader.GetOrdinal("harga"));
                    produk.Stok = reader.GetInt32(reader.GetOrdinal("stok"));
                    produk.PenjualID = reader.GetInt32(reader.GetOrdinal("penjualID"));
                    produk.KategoriID = reader.GetInt32(reader.GetOrdinal("kategoriID"));
                    produk.BahanID = reader.GetInt32(reader.GetOrdinal("bahanID"));
                }
                
                return produk;
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

        public async Task PatchProduk(Produk updatedProduk)
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

                const string query = @"
            UPDATE ""Produk"" 
            SET 
                ""nama"" = COALESCE(@nama, ""nama""),
                ""deskripsi"" = COALESCE(@desc, ""deskripsi""),
                ""harga"" = COALESCE(@harga, ""harga""),
                ""stok"" = COALESCE(@stok, ""stok""),
                ""kategoriID"" = COALESCE(@kategoriID, ""kategoriID""),
                ""bahanID"" = COALESCE(@bahanID, ""bahanID"")
            WHERE ""produkID"" = @id;";

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", updatedProduk.ProdukID ?? -1);
                command.Parameters.AddWithValue("@nama", updatedProduk.Nama ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@desc", updatedProduk.Deskripsi ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@harga", updatedProduk.Harga ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@stok", updatedProduk.Stok ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@kategoriID", updatedProduk.KategoriID ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@bahanID", updatedProduk.BahanID ?? (object)DBNull.Value);

                await command.ExecuteNonQueryAsync();

                throw new Exception($"Produk with ID {updatedProduk.ProdukID} not found.");
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
    }
}
