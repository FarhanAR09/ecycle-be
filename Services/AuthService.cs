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

                if (pengguna.Nama == null || pengguna.Password == null)
                {
                    throw new Exception("Username or password is null");
                }

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

        public async Task<Pengguna> Register(Pengguna pengguna)
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

                const string query = "insert into \"Pengguna\" (\"username\", \"password\", \"alamat\") values (@username, @password, @alamat) returning *;";

                using var command = new NpgsqlCommand(query, connection);

                if (pengguna.Nama == null || pengguna.Password == null)
                {
                    throw new Exception("Username or password is null");
                }

                command.Parameters.AddWithValue("@username", pengguna.Nama);
                command.Parameters.AddWithValue("@password", pengguna.Password);
                command.Parameters.AddWithValue("@alamat", (object?)pengguna.Alamat ?? DBNull.Value);

                var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    pengguna.PenggunaID = reader.GetInt32(reader.GetOrdinal("penggunaID"));
                    pengguna.Nama = reader.GetString(reader.GetOrdinal("username"));
                    pengguna.Password = reader.GetString(reader.GetOrdinal("password"));
                    pengguna.Alamat = reader.GetString(reader.GetOrdinal("alamat"));
                    pengguna.Telepon = reader.GetString(reader.GetOrdinal("telepon"));
                    pengguna.Token = reader.GetString(reader.GetOrdinal("token")) ?? "";
                }

                return pengguna;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception("Failed to connect to the database. " + ex.Message, ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public class UpdatingPengguna : Pengguna
        {
            public string? NewPassword { get; set; }
        }
        public async Task UpdatePenggunaAsync(UpdatingPengguna pengguna)
        {
            // Retrieve the connection string from app settings
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("Failed to connect to the database.");

            try
            {
                Pengguna loggedInPengguna = await Login(pengguna);

                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                const string query = @"
                    UPDATE ""Pengguna"" 
                    SET 
                        ""username"" = COALESCE(@nama, ""username""),
                        ""password"" = COALESCE(@password, ""password""),
                        ""alamat"" = COALESCE(@alamat, ""alamat""),
                        ""telepon"" = COALESCE(@telepon, ""telepon""),
                        ""token"" = COALESCE(@token, ""token"")
                    WHERE ""penggunaID"" = @penggunaID;
                ";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@penggunaID", loggedInPengguna.PenggunaID ?? throw new Exception("penggunaID is null"));
                command.Parameters.AddWithValue("@nama", (object?)pengguna.Nama ?? DBNull.Value);
                command.Parameters.AddWithValue("@password", (object?)pengguna.NewPassword ?? DBNull.Value);
                command.Parameters.AddWithValue("@alamat", (object?)pengguna.Alamat ?? DBNull.Value);
                command.Parameters.AddWithValue("@telepon", (object?)pengguna.Telepon ?? DBNull.Value);
                command.Parameters.AddWithValue("@token", (object?)pengguna.Token ?? DBNull.Value);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating Pengguna data. {ex.Message}", ex);
            }
        }
    }
}
