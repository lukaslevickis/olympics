using Microsoft.AspNetCore.Mvc;
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
            ViewsGeneralModel data = _athleteDBService.CreateFilterSortSelects();
            data.Athletes = _athleteDBService.Read();
            return View(data);
        }

        public IActionResult Submit(ViewsGeneralModel model)
        {
            _athleteDBService.Create(model);
            ViewsGeneralModel data = _athleteDBService.CreateFilterSortSelects();
            data.Athletes = _athleteDBService.Read();
            return View("Index", data);
        }

        public IActionResult Create()
        {
            ViewsGeneralModel data = _athleteDBService.CreateFilterSortSelects();
            return View(data);
        }

        [HttpPost]
        public IActionResult FilterBySports(ViewsGeneralModel model)
        {
            ViewsGeneralModel data = _athleteDBService.CreateFilterSortSelects();
            data.Athletes = _athleteDBService.FilterBySports(model);
            return View("Index", data);
        }

        [HttpPost]
        public IActionResult FilterByCountry(ViewsGeneralModel model)
        {
            ViewsGeneralModel data = _athleteDBService.CreateFilterSortSelects();
            data.Athletes = _athleteDBService.FilterByCountry(model);
            return View("Index", data);
        }

        [HttpPost]
        public IActionResult SortBy(ViewsGeneralModel model)
        {
            ViewsGeneralModel data = _athleteDBService.CreateFilterSortSelects();
            data.Athletes = _athleteDBService.SortBy(model);
            return View("Index", data);
        }
    }
}
