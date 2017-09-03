using AnimeExporter.Models;
using AnimeExporter.Utility;

namespace AnimeExporter.Controllers {
    public class StatsController : Page {
        
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public StatsController(string url) : base(url + "/stats") { }
        
        protected override DataModel Scrape() {
            return new StatsModel {
                Watching         = { Value = this.SelectValueAfterText("Watching:")},
                Completed        = { Value = this.SelectValueAfterText("Completed:")},
                OnHold           = { Value = this.SelectValueAfterText("On-Hold:")},
                Dropped          = { Value = this.SelectValueAfterText("Dropped:")},
                PlanToWatch      = { Value = this.SelectValueAfterText("Plan to Watch:")},
                Total            = { Value = this.SelectValueAfterText("Total:")},
                NumberScoreEight = { Value = "54" }
            };
        }
    }
}
