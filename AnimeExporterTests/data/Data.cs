using AnimeExporter;

using static AnimeExporterTests.data.TestConstants;

namespace AnimeExporterTests.data {
    
    /// <summary>
    /// Contains basic data for tests
    /// </summary>
    public class Data {
        public static Anime CreateKimiNoNaWa(string url = null) {
            return new Anime {
                Title = KimiNoNaWa.Title,
                Url = url ?? KimiNoNaWa.Url
            };
        }

        public static AnimeDetailsPage CreateKimiNoNaWaDetailsPage() {
            return new AnimeDetailsPage(KimiNoNaWa.Url);
        }
    }
}