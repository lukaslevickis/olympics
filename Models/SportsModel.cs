using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Olympics.Models
{
    public class SportsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool TeamActivity { get; set; }

        public string SportsName { get; set; }
        public List<SelectListItem> SportsFormSelect { get; set; } = new List<SelectListItem>();
    }
}
