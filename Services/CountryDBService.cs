using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Olympics.Models;

namespace Olympics.Services
{
    public class CountryDBService
    {
        private readonly SqlConnection _connection;

        public CountryDBService(SqlConnection connection)
        {
            _connection = connection;
        }

        public List<CountryModel> Read()
        {
            List<CountryModel> items = new();

            _connection.Open();

            using var command = new SqlCommand("SELECT ID, CountryName, ISO3 FROM dbo.Countries;", _connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                items.Add(
                new CountryModel
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    ISO3 = reader.GetString(2),
                });
            }

            _connection.Close();

            return items;
        }

        public void Create(CountryModel model)
        {
            _connection.Open();

            using var command = new SqlCommand($"INSERT into dbo.Countries (CountryName, ISO3) values ('{model.Name}', '{model.ISO3}');", _connection);
            command.ExecuteNonQuery();

            _connection.Close();
        }
    }
}
