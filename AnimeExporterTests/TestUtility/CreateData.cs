using AnimeExporter.Controllers;
using AnimeExporter.Models;

namespace AnimeExporterTests.TestUtility {
    
    /// <summary>
    /// Contains basic data for tests
    /// </summary>
    public class CreateData {
        
        // Full Models
        
        public static AnimeModel KimiNoNaWa(string url = null) {
            return new AnimeModel(url ?? TestConstants.KimiNoNaWa.Url, new DetailsModel(), new StatsModel(), new VideoModel(), new CharactersModel());
        }
        
        // Characters
        
        public static CharactersController FMABrotherhoodCharacters() {
            return new CharactersController(TestConstants.FMABrotherhood.Url);
        }

        public static CharactersController KimiNiTodokeCharacters() {
            return new CharactersController(TestConstants.KimiNiTodoke.Url);
        }
        
        // Details
        
        public static DetailsController KimiNoNaWaDetails() {
            return new DetailsController(TestConstants.KimiNoNaWa.Url);
        }

        public static DetailsController SteinsGateDetails() {
            return new DetailsController(TestConstants.SteinsGate.Url);
        }

        public static DetailsController OwariMongatariSecondSeasonDetails() {
            return new DetailsController(TestConstants.OwarimongatariSecondSeason.Url);
        }

        // Stats
        
        public static StatsController FMABrotherhoodStats() {
            return new StatsController(TestConstants.FMABrotherhood.Url);
        }
        
        // Videos

        public static VideoController FMABrotherhoodVideo() {
            return new VideoController(TestConstants.FMABrotherhood.Url);
        }

        public static VideoController KimiNiTodokeVideo() {
            return new VideoController(TestConstants.KimiNiTodoke.Url);
        }

        public static VideoController SteinsGateVideo() {
            return new VideoController(TestConstants.SteinsGate.Url);
        }
    }
}