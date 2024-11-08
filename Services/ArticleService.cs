using ecycle_be.Models;
using ecycle_be.Services;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace ecycle_be.Services
{
    public class ArticleService(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<List<Artikel>> GetArticles()
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

                const string query = "SELECT * FROM \"Artikel\";";

                using var command = new NpgsqlCommand(query, connection);

                using var reader = await command.ExecuteReaderAsync();

                List<Artikel> articles = [];
                while (await reader.ReadAsync())
                {
                    articles.Add(new Artikel()
                    {
                        ArtikelID = reader.GetInt32(reader.GetOrdinal("artikelID")),
                        Judul = reader.GetString(reader.GetOrdinal("judul")),
                        Konten = reader.GetString(reader.GetOrdinal("konten")),
                        AdminID = reader.GetInt32(reader.GetOrdinal("adminID")),
                    });
                }
                return articles;
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
    }
}
