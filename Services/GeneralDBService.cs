using Olympics.Models;
namespace Olympics.Services
{
    public class GeneralDBService
    {
        private readonly AthleteDBService _athleteDBService;
        private readonly CountryDBService _countryDBService;
        private readonly SportsDBService _sportsDBService;

        public GeneralDBService(AthleteDBService athleteDBService,
                                         CountryDBService countryDBService,
                                         SportsDBService sportsDBService)
        {
            _athleteDBService = athleteDBService;
            _countryDBService = countryDBService;
            _sportsDBService = sportsDBService;
        }

        public GeneralModel GetGeneralDBData()
        {
            GeneralModel model = new();
            model.Athletes = _athleteDBService.Read();
            model.Countries = _countryDBService.Read();
            model.Sports = _sportsDBService.Read();

            return model;
        }
    }
}
