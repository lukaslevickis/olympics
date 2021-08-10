using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Olympics.Models;

namespace Olympics.Services
{
    public class SportsDBService
    {
        private readonly SqlConnection _connection;

        public SportsDBService(SqlConnection connection)
        {
            _connection = connection;
        }

        public List<SportsModel> Read()
        {
            List<SportsModel> items = new();

            _connection.Open();

            using var command = new SqlCommand("SELECT Id, SportsName, TeamActivity FROM dbo.Sports;", _connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                items.Add(
                new SportsModel
                {
                    Name = reader.GetString(1),
                    TeamActivity = reader.GetBoolean(2),
                });
            }

            _connection.Close();

            return items;
        }

        public void Create(SportsModel model)
        {
            _connection.Open();

            using var command = new SqlCommand($"INSERT into dbo.Sports (SportsName, TeamActivity) values ('{model.Name}', '{model.TeamActivity}');", _connection);
            command.ExecuteNonQuery();

            _connection.Close();
        }
    }
}
