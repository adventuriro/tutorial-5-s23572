using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using tutorial_5_s23572.Models;


namespace tutorial_5_s23572
{
    public class Database
    {
        private readonly string _connectionString =
            "Server=localhost\\APBD,1433;Database=yac;Integrated Security=True;TrustServerCertificate=True";

        public async Task<List<Animal>> GetAnimals(string? orderBy)
        {
            List<Animal> results = new();

            using (SqlConnection conn = new(_connectionString))
            {
                using (SqlCommand command = new())
                {
                    command.Connection = conn;
                    command.CommandText = $"SELECT * FROM Animal ORDER BY {orderBy ?? "Name"} ASC";
                    await conn.OpenAsync().ConfigureAwait(false);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            results.Add(new Animal
                            {
                                IdAnimal = int.Parse(reader["IdAnimal"].ToString()!),
                                Name = Convert.ToInt32(reader["Name"].ToString()!).ToString(),
                                Description = reader["Description"].ToString()!,
                                Category = reader["Category"].ToString()!,
                                Area = reader["Area"].ToString()!,
                            });
                        }
                    }
                }
            }

            return results;
        }

        public async Task<Animal?> GetAnimalById(int id)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                using (SqlCommand command = new())
                {
                    command.Connection = conn;
                    command.CommandText = "SELECT * FROM Animal WHERE IdAnimal = @id";
                    command.Parameters.AddWithValue("@id", id);
                    await conn.OpenAsync().ConfigureAwait(false);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        if (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            return new Animal
                            {
                                IdAnimal = int.Parse(reader["IdAnimal"].ToString()!),
                                Name = Convert.ToInt32(reader["Name"].ToString()!).ToString(),
                                Description = reader["Description"].ToString()!,
                                Category = reader["Category"].ToString()!,
                                Area = reader["Area"].ToString()!,
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task CreateAnimal(Animal animal)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                using (SqlCommand command = new())
                {
                    command.Connection = conn;
                    command.CommandText =
                        "INSERT INTO Animal (Name, Description, Category, Area) VALUES (@name, @description, @category, @area)";
                    await conn.OpenAsync().ConfigureAwait(false);
                    command.Parameters.AddWithValue("@name", animal.Name);
                    command.Parameters.AddWithValue("@description", animal.Description);
                    command.Parameters.AddWithValue("@category", animal.Category);
                    command.Parameters.AddWithValue("@area", animal.Area);
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task UpdateAnimal(Animal animal)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                using (SqlCommand command = new())
                {
                    command.Connection = conn;
                    command.CommandText =
                        "UPDATE Animal SET Name = @name, Description = @description, Category = @category, Area = @area WHERE IdAnimal = @id";
                    await conn.OpenAsync().ConfigureAwait(false);
                    command.Parameters.AddWithValue("@name", animal.Name);
                    command.Parameters.AddWithValue("@description", animal.Description);
                    command.Parameters.AddWithValue("@category", animal.Category);
                    command.Parameters.AddWithValue("@area", animal.Area);
                    command.Parameters.AddWithValue("@id", animal.IdAnimal);
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task DeleteAnimal(int id)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                using (SqlCommand command = new())
                {
                    command.Connection = conn;
                    command.CommandText = "DELETE FROM Animal WHERE IdAnimal = @id";
                    await conn.OpenAsync().ConfigureAwait(false);
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
