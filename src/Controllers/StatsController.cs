using AnimeExporter.Models;
using AnimeExporter.Utility;

namespace AnimeExporter.Controllers {
    public class StatsController : Page {
        
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public StatsController(string url) : base(url + "/stats") { }
        
        // TODO: generalize the scrape
        public StatsModel TryScrape(int retriesLeft = Page.MaxRetryCount) {
            Log.Info("scraping STATS!!!!!!!!!!!!!");
            
            return new StatsModel {
                Completed = { Value = "345" },
                Dropped = { Value = "634" },
                NumberScoreEight = {Value = "54" }
            };
        }
    }
}