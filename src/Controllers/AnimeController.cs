using AnimeExporter.Models;

namespace AnimeExporter.Controllers {
    public class AnimeController {
        public static AnimeModel ScrapeData(string url) {
            var details = new DetailsController(url);
            // TODO: scrape other pages (stats, videos, etc.)
            
            return new AnimeModel(url, details.TryScrape());
        }
    }
}