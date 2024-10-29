using ecycle_be.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace ecycle_be.Services
{
    public class AuthService(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<Pengguna> Login(Pengguna pengguna)
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

                const string query = "SELECT * FROM \"Pengguna\" WHERE \"username\" = @username AND \"password\" = @password LIMIT 1;";

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", pengguna.Nama);
                command.Parameters.AddWithValue("@password", pengguna.Password);

                using var reader = await command.ExecuteReaderAsync();

                if (!reader.HasRows)
                    throw new Exception("User not found");

                await reader.ReadAsync();

                Pengguna first = new()
                {
                    PenggunaID = reader.GetInt32(reader.GetOrdinal("penggunaID")),
                    Nama = reader.GetString(reader.GetOrdinal("username")),
                    Password = reader.GetString(reader.GetOrdinal("password")),
                    Alamat = reader.GetString(reader.GetOrdinal("alamat")),
                    Telepon = reader.GetString(reader.GetOrdinal("telepon")),
                    Token = reader.GetString(reader.GetOrdinal("telepon")) ?? "",
                };

                return first;
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

        public async Task Register(Pengguna pengguna)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Failed to connect to the database.");
                throw new Exception("Failed to connect to the database.");
            }

            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                const string query = "insert into \"Pengguna\" (\"username\", \"password\", \"alamat\") values (@username, @password, @alamat);";

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", pengguna.Nama);
                command.Parameters.AddWithValue("@password", pengguna.Password);
                command.Parameters.AddWithValue("@alamat", pengguna.Alamat);

                await command.ExecuteNonQueryAsync();
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
