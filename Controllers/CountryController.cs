using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Olympics.Models;
using Olympics.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Olympics.Controllers
{
    public class CountryController : Controller
    {
        private readonly CountryDBService _countryDBService;

        public CountryController(CountryDBService countryDBService)
        {
            _countryDBService = countryDBService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<CountryModel> data = _countryDBService.Read();
            return View(data);
        }

        public IActionResult Submit(CountryModel model)
        {
            _countryDBService.Create(model);
            List<CountryModel> data = _countryDBService.Read();
            return View("Index", data);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
