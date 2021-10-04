using System;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.IO;
using Assignment4.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment4
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = LoadConfiguration();
            var connectionString = configuration.GetConnectionString("KanbanBoard");

            var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>().UseSqlServer(connectionString);
            using var context = new KanbanContext(optionsBuilder.Options);

            var cmdText = "SELECT * FROM Users";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(cmdText, connection);

            connection.Open();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var user = new
                {
                    id = reader.GetInt32("id"),
                    name = reader.GetString("name"),
                    email = reader.GetString("email")
                };

                Console.WriteLine(user);
            }

            connection.Close();


            static IConfiguration LoadConfiguration()
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddUserSecrets<Program>();

                return builder.Build();
            }
        }
    }
}
