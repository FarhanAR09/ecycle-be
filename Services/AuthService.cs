using ecycle_be.Models;
using Npgsql;
using System.Data;

namespace ecycle_be.Services
{
    public class AuthService(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<Pengguna> Login(Pengguna pengguna)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection") ??
                throw new Exception("Connection string 'DefaultConnection' not found.");
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            using NpgsqlCommand command = new(
                "SELECT * FROM \"Pengguna\" LIMIT 1;",
                connection);
            
            using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
                throw new Exception("No rows found.");
            await reader.ReadAsync();
            Pengguna first = new()
            {
                PenggunaID = reader.GetInt32("penggunaID"),
                Nama = reader.GetString("username"),
                Password = reader.GetString("password"),
                Alamat = reader.GetString("alamat"),
                Telepon = reader.GetString("telepon"),
                Token = reader.GetString("token"),
            };
            return first;
        }
    }
}
