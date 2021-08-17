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

        internal List<AthleteModel> SortBy(ViewsGeneralModel model)
        {
            List<AthleteModel> athletes = Read();

            switch (model.FilterSort.SortOrder)
            {
                case "Name":
                    athletes = athletes.OrderBy(s => s.Name).ToList();
                    break;
                case "Surname":
                    athletes = athletes.OrderBy(s => s.Surname).ToList();
                    break;
                default:
                    athletes = athletes.OrderBy(s => s.CountryName).ToList();
                    break;
            }

            return athletes;
        }

        internal List<AthleteModel> FilterBySports(ViewsGeneralModel model)
        {
            List<AthleteModel> data = Read();
            data = data.Where(x => x.Sports.Contains(model.FilterSort.FilterSport)).ToList();
            return data;
        }

        internal List<AthleteModel> FilterByCountry(ViewsGeneralModel model)
        {
            List<AthleteModel> data = Read();
            data = data.Where(x => x.CountryName.Contains(model.FilterSort.FilterCountry)).ToList();
            return data;
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

        public void Create(ViewsGeneralModel model)//todo multiple select
        {
            List<CountryModel> countries = _countryDBService.Read();
            List<SportsModel> sports = _sportsDBService.Read();
            int countryId = countries.Where(x => x.Name == model.Countries[0].CountryName).FirstOrDefault().Id;
            int sportsId = sports.Where(x => x.Name == model.Sports[0].SportsName).FirstOrDefault().Id;

            _connection.Open();
            using var command = new SqlCommand($"INSERT INTO dbo.Athletes (Name, Surname, CountryId) values ('{model.Athlete.Name}', '{model.Athlete.Surname}', '{countryId}');", _connection);
            command.ExecuteNonQuery();
            _connection.Close();

            List<AthleteModel> id = GetId();
            int athleteId = id.LastOrDefault().Id;

            _connection.Open();
            using var command2 = new SqlCommand($"INSERT INTO dbo.AthleteSports (AthleteId, SportsId) values ('{athleteId}', '{sportsId}');", _connection);
            command2.ExecuteNonQuery();
            _connection.Close();
        }

        public ViewsGeneralModel CreateFilterSortSelects()
        {
            ViewsGeneralModel data = new ViewsGeneralModel();
            List<SelectListItem> sportsSelect = new List<SelectListItem>();
            List<SelectListItem> countrySelect  = new List<SelectListItem>();
            List<SelectListItem> sortSelect  = new List<SelectListItem>();
            data.Sports = _sportsDBService.Read();
            data.Countries = _countryDBService.Read();

            foreach (SportsModel item in data.Sports)
            {
                sportsSelect.Add(new SelectListItem { Value = item.Name, Text = item.Name });
            }

            data.Sports.FirstOrDefault().SportsFormSelect = sportsSelect;

            foreach (CountryModel item in data.Countries)
            {
                countrySelect.Add(new SelectListItem { Value = item.Name, Text = item.Name });
            }

            data.Countries.FirstOrDefault().CountryFormSelect = countrySelect;

            foreach (string item in data.FilterSort.SortProperties)
            {
                sortSelect.Add(new SelectListItem { Value = item, Text = item });
            }

            data.FilterSort.SortFormSelect = sortSelect;

            return data;
        }
    }
}
