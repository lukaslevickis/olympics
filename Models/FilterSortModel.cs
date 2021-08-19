using System.Collections.Generic;

namespace Olympics.Models
{
    public class FilterSortModel
    {
        public string FilterCountry { get; set; }
        public string FilterSport { get; set; }
        public string FilterActivity { get; set; }
        public string SortOrder { get; set; }
        public List<string> SortProperties { get; set; } = new() { "Name", "Surname", "Country" };
        public List<string> TeamActivityProperties { get; set; } = new List<string>() { "Team", "Non-team" };

    }
}
