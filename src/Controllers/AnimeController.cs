using System;
using AnimeExporter.Models;

namespace AnimeExporter.Controllers {
    public class AnimeController {
        
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public static AnimeModel ScrapeData(string url) {
            Log.Info($"Scraping {url}");
            
            var details = new DetailsController(url);
            var stats = new StatsController(url);
            var anime = new AnimeModel(url, details.TryScrape(), stats.TryScrape());
            
            Log.Debug($"Scraped {Environment.NewLine} {anime}");
            
            return anime;
        }
    }
}