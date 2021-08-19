using System.Collections.Generic;

namespace Olympics.Models
{
    public class SportsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TeamActivity { get; set; }
        public List<string> TeamActivityProperties { get; set; } = new List<string>() { "Team", "Non-team" };
    }
}
