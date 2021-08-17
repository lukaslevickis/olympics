using System;
using System.Collections.Generic;

namespace Olympics.Models
{
    public class ViewsGeneralModel
    {
        public AthleteModel Athlete { get; set; }
        public List<AthleteModel> Athletes { get; set; } = new List<AthleteModel>();
        public List<CountryModel> Countries { get; set; } = new List<CountryModel>();
        public List<SportsModel> Sports { get; set; } = new List<SportsModel>();
        public FilterSortModel FilterSort { get; set; }
    }
}
