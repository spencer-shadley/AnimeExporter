using AnimeExporter.Controllers;
using AnimeExporter.Models;

namespace AnimeExporterTests.TestUtility {
    
    /// <summary>
    /// Contains basic data for tests
    /// </summary>
    public class CreateData {
        public static AnimeModel KimiNoNaWa(string url = null) {
            return new AnimeModel(url ?? TestConstants.KimiNoNaWa.Url, new DetailsModel(), new StatsModel(), new VideoModel());
        }

        public static DetailsController KimiNoNaWaDetailsPage() {
            return new DetailsController(TestConstants.KimiNoNaWa.Url);
        }

        public static DetailsController SteinsGateDetailsPage() {
            return new DetailsController(TestConstants.SteinsGate.Url);
        }

        public static DetailsController OwariMongatariSecondSeasonDetailsPage() {
            return new DetailsController(TestConstants.OwarimongatariSecondSeason.Url);
        }
    }
}