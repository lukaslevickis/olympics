using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Olympics.Models
{
    public class FilterSortModel
    {

        public string FilterCountry { get; set; }
        public string FilterSport { get; set; }
        //public string FilerActivity { get; set; } // -1 || 0 || 1 teamactivity dropdownas "pasirinkti actiitivy tipa"(-1), "not team"(0), "team" (1)
        public string SortOrder { get; set; }
        public List<string> SortProperties { get; set; } = new() { "Name", "Surname", "Country" };
        public List<SelectListItem> SortFormSelect { get; set; } = new List<SelectListItem>();

    }
}
