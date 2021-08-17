using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Olympics.Models
{
    public class CountryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ISO3 { get; set; }

        public string CountryName { get; set; }
        public List<SelectListItem> CountryFormSelect { get; set; } = new List<SelectListItem>();
    }
}
