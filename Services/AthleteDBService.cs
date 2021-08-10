using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Olympics.Models;

namespace Olympics.Services
{
    public class AthleteDBService
    {
        private readonly SqlConnection _connection;

        public AthleteDBService(SqlConnection connection)
        {
            _connection = connection;
        }

        public List<AthleteModel> Read()
        {
            List<AthleteModel> items = new();

            _connection.Open();

            using var command = new SqlCommand("SELECT Id, Name, Surname, CountryName FROM dbo.AthletesWithCountries;", _connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                items.Add(
                new AthleteModel
                {
                    Name = reader.GetString(1),
                    Surname = reader.GetString(2),
                    CountryName = reader.GetString(3),
                });
            }

            _connection.Close();

            return items;
        }

        public void Create(AthleteModel model, List<CountryModel> countries)
        {

            _connection.Open();
            model.CountryId = countries.Where(x => x.ISO3 == model.ISO3).FirstOrDefault().Id;

            using var command = new SqlCommand($"INSERT into dbo.Athletes (Name, Surname, CountryId) values ('{model.Name}', '{model.Surname}', '{model.CountryId}');", _connection);
            command.ExecuteNonQuery();

            _connection.Close();
        }
    }
}
