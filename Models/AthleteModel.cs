using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Olympics.Models
{
    public class AthleteModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int CountryId { get; set; }

        public string ISO3 { get; set; }
        public string CountryName { get; set; }
        public List<SelectListItem> CountriesFormSelect { get; set; } = new List<SelectListItem>();

        public string SportsName { get; set; }
        public List<string> Sports { get; set; } = new List<string>();
        public List<SelectListItem> SportsFormSelect { get; set; } = new List<SelectListItem>();
        public int SportsId { get; set; }

    }
}
