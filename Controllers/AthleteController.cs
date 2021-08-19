using Microsoft.AspNetCore.Mvc;
using Olympics.Models;
using Olympics.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Olympics.Controllers
{
    public class AthleteController : Controller
    {
        private readonly AthleteDBService _athleteDBService;
        private readonly GeneralDBService _generalDBService;

        public AthleteController(AthleteDBService athleteDBService, GeneralDBService viewsGeneralService)
        {
            _athleteDBService = athleteDBService;
            _generalDBService = viewsGeneralService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            GeneralModel data = _generalDBService.GetGeneralDBData();
            return View(data);
        }

        public IActionResult Submit(GeneralModel model)
        {
            _athleteDBService.Create(model);
            GeneralModel data = _generalDBService.GetGeneralDBData();
            data.Athletes = _athleteDBService.Read();
            return View("Index", data);
        }

        public IActionResult Create()
        {
            GeneralModel data = _generalDBService.GetGeneralDBData();
            return View(data);
        }

        [HttpPost]
        public IActionResult FilterGeneral(GeneralModel model)
        {
            GeneralModel data = _generalDBService.GetGeneralDBData();
            data.Athletes = _athleteDBService.FilterGeneral(model);
            return View("Index", data);
        }

        [HttpPost]
        public IActionResult SortBy(GeneralModel model)
        {
            GeneralModel data = _generalDBService.GetGeneralDBData();
            data.Athletes = _athleteDBService.SortBy(model);
            return View("Index", data);
        }
    }
}
