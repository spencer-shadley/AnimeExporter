using AnimeExporter;

using static AnimeExporterTests.data.TestConstants;

namespace AnimeExporterTests.data {
    public class Data {
        public static Anime CreateKimiNoNaWa(string url = null) {
            return new Anime {
                Title = KimiNoNaWa.Title,
                Url = url ?? KimiNoNaWa.Url
            };
        }
    }
}