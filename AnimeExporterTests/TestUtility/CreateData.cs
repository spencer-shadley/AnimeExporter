using AnimeExporter.Controllers;
using AnimeExporter.Models;

namespace AnimeExporterTests.TestUtility {
    
    /// <summary>
    /// Contains basic data for tests
    /// </summary>
    public class CreateData {
        public static DetailsModel KimiNoNaWa(string url = null) {
            return new DetailsModel {
                Title = TestConstants.KimiNoNaWa.Title,
                Url = url ?? TestConstants.KimiNoNaWa.Url
            };
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