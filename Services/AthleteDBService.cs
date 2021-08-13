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
        private Dictionary<int, AthleteModel> _athletes = new Dictionary<int, AthleteModel>();

        public AthleteDBService(SqlConnection connection)
        {
            _connection = connection;
        }

        public Dictionary<int, AthleteModel> Read()
        {
            List<AthleteModel> items = new();

            _connection.Open();

            //many to many left joins
            using var command = new SqlCommand("SELECT dbo.AthletesWithCountries.ID, dbo.AthletesWithCountries.Name , dbo.AthletesWithCountries.Surname, dbo.AthletesWithCountries.CountryName, dbo.Sports.SportsName " +
                                                "FROM dbo.AthletesWithCountries " +
                                               "LEFT OUTER JOIN dbo.AthleteSports " +
                                                "ON dbo.AthletesWithCountries.ID = dbo.AthleteSports.AthleteId " +
                                               "LEFT OUTER JOIN dbo.Sports " +
                                                "ON dbo.AthleteSports.SportsId = dbo.Sports.ID;", _connection);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                items.Add(
                new AthleteModel
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Surname = reader.GetString(2),
                    CountryName = reader.GetString(3),
                    SportsName = reader.GetString(4),
                });
            }

            _connection.Close();

            _athletes = GetAthletes(items);

            return _athletes;
        }

        private Dictionary<int, AthleteModel> GetAthletes(List<AthleteModel> allAthletes)
        {
            foreach (AthleteModel athlete in allAthletes)
            {
                if (!_athletes.ContainsKey(athlete.Id))
                {
                    _athletes.Add(athlete.Id, athlete);
                }

                _athletes[athlete.Id].Sports.Add(athlete.SportsName);
            }

            return _athletes;
        }

        public void Create(AthleteModel model, List<CountryModel> countries)
        {

            _connection.Open();
            model.CountryId = countries.Where(x => x.ISO3 == model.ISO3).FirstOrDefault().Id;

            using var command = new SqlCommand($"Ic(Name, Surname, CountryId) values ('{model.Name}', '{model.Surname}', '{model.CountryId}');", _connection);
            command.ExecuteNonQuery();

            _connection.Close();
        }
    }
}
