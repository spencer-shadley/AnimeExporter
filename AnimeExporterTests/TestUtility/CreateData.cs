using AnimeExporter.Controllers;
using AnimeExporter.Models;

namespace AnimeExporterTests.TestUtility {
    
    /// <summary>
    /// Contains basic data for tests
    /// </summary>
    public class CreateData {
        public static CharactersController FMABrotherhoodCharacters() {
            return new CharactersController(TestConstants.FMABrotherhood.Url);
        }

        public static CharactersController KimiNiTodokeCharacters() {
            return new CharactersController(TestConstants.KimiNiTodoke.Url);
        }
        
        public static AnimeModel KimiNoNaWa(string url = null) {
            return new AnimeModel(url ?? TestConstants.KimiNoNaWa.Url, new DetailsModel(), new StatsModel(), new VideoModel(), new CharactersModel());
        }
        
        public static DetailsController KimiNoNaWaDetails() {
            return new DetailsController(TestConstants.KimiNoNaWa.Url);
        }

        public static DetailsController SteinsGateDetails() {
            return new DetailsController(TestConstants.SteinsGate.Url);
        }

        public static DetailsController OwariMongatariSecondSeasonDetails() {
            return new DetailsController(TestConstants.OwarimongatariSecondSeason.Url);
        }

        public static StatsController FMABrotherhoodStats() {
            return new StatsController(TestConstants.FMABrotherhood.Url);
        }
    }
}