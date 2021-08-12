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

        private List<AthleteModel> GetAthletes(List<AthleteModel> items)
        {
            List<List<AthleteModel>> athletes = new List<List<AthleteModel>>();

            //adds same rows into one list
            foreach (AthleteModel item in items)
            {
                List<AthleteModel> athlete = new List<AthleteModel>();
                foreach (AthleteModel itemInner in items) //TODO naming itemInner???s
                {
                    if (item.Id == itemInner.Id)
                    {
                        athlete.Add(itemInner);
                    }
                }

                athletes.Add(athlete);
            }

            //removes dublicates
            athletes = athletes.GroupBy(x => x.FirstOrDefault().Id).Select(x => x.First()).ToList();

            items = new();

            //adds sports into AthleteModel list of strings
            foreach (var athlete in athletes)
            {
                List<string> sports = new();

                foreach (AthleteModel athleteModel in athlete)
                {
                    sports.Add(athleteModel.SportsName);
                }

                items.Add(
                new AthleteModel
                {
                    Name = athlete.FirstOrDefault().Name,
                    Surname = athlete.FirstOrDefault().SportsName,
                    CountryName = athlete.FirstOrDefault().CountryName,
                    Sports = sports,
                });
            }

            return items;
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
