using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Olympics.Models
{
    public class AthleteModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int CountryId { get; set; }
        public string ISO3 { get; set; }
        public string CountryName { get; set; }
        public List<SelectListItem> Countries { get; set; } = new List<SelectListItem>();
    }
}
