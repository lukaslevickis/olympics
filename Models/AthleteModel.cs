using System.Collections.Generic;

namespace Olympics.Models
{
    public class AthleteModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int CountryId { get; set; }

        public string CountryName { get; set; }
        public string SportsName { get; set; }
        public bool TeamActivity { get; set; }
        public List<string> Sports { get; set; } = new List<string>();

    }
}
