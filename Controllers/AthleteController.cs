using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Olympics.Models;
using Olympics.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Olympics.Controllers
{
    public class AthleteController : Controller
    {
        private readonly AthleteDBService _athleteDBService;
        private readonly CountryDBService _countryDBService;

        public AthleteController(AthleteDBService athleteDBService, CountryDBService countryDBService)
        {
            _athleteDBService = athleteDBService;
            _countryDBService = countryDBService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<AthleteModel> data = _athleteDBService.Read();
            return View(data);
        }

        public IActionResult Submit(AthleteModel model)
        {
            List<CountryModel> countriesData = _countryDBService.Read();
            _athleteDBService.Create(model, countriesData);
            List<AthleteModel> data = _athleteDBService.Read();
            return View("Index", data);
        }

        public IActionResult Create()
        {
            List<CountryModel> data = _countryDBService.Read();
            AthleteModel model = new AthleteModel();

            foreach (CountryModel item in data)
            {
                model.Countries.Add(new SelectListItem { Value = item.ISO3, Text = item.ISO3 });
            }

            return View(model);
        }
    }
}
