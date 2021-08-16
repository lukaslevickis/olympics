using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Olympics.Models;

namespace Olympics.Services
{
    public class AthleteDBService
    {
        private readonly SqlConnection _connection;
        private readonly CountryDBService _countryDBService;
        private readonly SportsDBService _sportsDBService;

        public AthleteDBService(CountryDBService countryDBService, SportsDBService sportsDBService, SqlConnection connection)
        {
            _connection = connection;
            _countryDBService = countryDBService;
            _sportsDBService = sportsDBService;
        }

        public List<AthleteModel> Read()
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

            items = GetAthletes(items);

            return items;
        }

        private List<AthleteModel> GetAthletes(List<AthleteModel> athletes)
        {
            Dictionary<int, AthleteModel> athletesDic = new Dictionary<int, AthleteModel>();

            foreach (AthleteModel athlete in athletes)
            {
                if (!athletesDic.ContainsKey(athlete.Id))
                {
                    athletesDic.Add(athlete.Id, athlete);
                }

                athletesDic[athlete.Id].Sports.Add(athlete.SportsName);
            }

            athletes = athletesDic.Select(x => x.Value).ToList();

            return athletes;
        }

        public List<AthleteModel> GetId()
        {
            List<AthleteModel> items = new();

            _connection.Open();

            using var command = new SqlCommand("SELECT dbo.Athletes.ID from dbo.Athletes", _connection);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                items.Add(
                new AthleteModel
                {
                    Id = reader.GetInt32(0),
                });
            }

            _connection.Close();

            return items;
        }

        public void Create(AthleteModel model)//todo multiple select
        {
            List<CountryModel> countries = _countryDBService.Read();
            List<SportsModel> sports = _sportsDBService.Read();
            model.CountryId = countries.Where(x => x.ISO3 == model.ISO3).FirstOrDefault().Id;
            model.SportsId = sports.Where(x => x.Name == model.SportsName).FirstOrDefault().Id;

            _connection.Open();
            using var command = new SqlCommand($"INSERT INTO dbo.Athletes (Name, Surname, CountryId) values ('{model.Name}', '{model.Surname}', '{model.CountryId}');", _connection);
            command.ExecuteNonQuery();
            _connection.Close();

            List<AthleteModel> id = GetId();
            model.Id = id.LastOrDefault().Id;

            _connection.Open();
            using var command2 = new SqlCommand($"INSERT INTO dbo.AthleteSports (AthleteId, SportsId) values ('{model.Id}', '{model.SportsId}');", _connection);
            command2.ExecuteNonQuery();
            _connection.Close();
        }

        public AthleteModel CreateAthlete()
        {
            List<CountryModel> countryData = _countryDBService.Read();
            List<SportsModel> sportsData = _sportsDBService.Read();
            AthleteModel model = new AthleteModel();

            foreach (CountryModel item in countryData)//todo
            {
                model.CountriesFormSelect.Add(new SelectListItem { Value = item.ISO3, Text = item.ISO3 });
            }

            foreach (SportsModel item2 in sportsData)
            {
                model.SportsFormSelect.Add(new SelectListItem { Value = item2.Name, Text = item2.Name });
            }

            return model;
        }
    }
}
