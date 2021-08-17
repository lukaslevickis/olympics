using System;
using System.Collections.Generic;

namespace Olympics.Models
{
    public class FilterSortModel
    {

        public string FilterCountry { get; set; }
        public string FilterSport { get; set; }
        //public string FilerActivity { get; set; } // -1 || 0 || 1 teamactivity dropdownas "pasirinkti actiitivy tipa"(-1), "not team"(0), "team" (1)
        //public string Sort { get; set; } // name || surname || sport || country dropdownas
        public List<string> SortProperties { get; set; } = new() { "Name", "Surname", "Country", "Sport" };
    }
}
