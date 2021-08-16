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

        public AthleteController(AthleteDBService athleteDBService)
        {
            _athleteDBService = athleteDBService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<AthleteModel> data = _athleteDBService.Read();
            return View(data);
        }

        public IActionResult Submit(AthleteModel model)
        {
            _athleteDBService.Create(model);
            List<AthleteModel> data = _athleteDBService.Read();
            return View("Index", data);
        }

        public IActionResult Create()
        {
            AthleteModel model = _athleteDBService.CreateAthlete();

            return View(model);
        }
    }
}
