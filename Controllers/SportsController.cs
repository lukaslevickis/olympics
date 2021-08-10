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
    public class SportsController : Controller
    {
        private readonly SportsDBService _sportsDBService;

        public SportsController(SportsDBService sportsDBService)
        {
            _sportsDBService = sportsDBService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<SportsModel> data = _sportsDBService.Read();
            return View(data);
        }

        public IActionResult Submit(SportsModel model)
        {
            _sportsDBService.Create(model);
            List<SportsModel> data = _sportsDBService.Read();
            return View("Index", data);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
