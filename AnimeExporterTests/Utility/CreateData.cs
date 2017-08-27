using AnimeExporter;

using static AnimeExporterTests.data.TestConstants;

namespace AnimeExporterTests.data {
    
    /// <summary>
    /// Contains basic data for tests
    /// </summary>
    public class CreateData {
        public static Anime KimiNoNaWa(string url = null) {
            return new Anime {
                Title = TestConstants.KimiNoNaWa.Title,
                Url = url ?? TestConstants.KimiNoNaWa.Url
            };
        }

        public static AnimeDetailsPage KimiNoNaWaDetailsPage() {
            return new AnimeDetailsPage(TestConstants.KimiNoNaWa.Url);
        }
    }
}