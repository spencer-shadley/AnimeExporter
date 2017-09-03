using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AnimeExporter.Models;
using AnimeExporter.Utility;
using HtmlAgilityPack;

namespace AnimeExporter.Controllers {
    
    /// <summary>
    /// Represents the top anime page(s) on MyAnimeList.net
    /// </summary>
    /// <example>https://myanimelist.net/topanime.php?limit=5000</example>
    /// <remarks>
    /// As of 6/18/17 There are 12,972 animes on MyAnimeList in the "top" section.
    /// In one test it took 29.3 minutes to scrape all top anime between pages 0 and 100 (2,850 animes.)
    /// </remarks> 
    public class AnimesController : Page {
    
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public AnimesController(string url) : base(url) { }
        
        public static string GetTopAnimeUrl(int page) {
            const string topAnimeUrl = MyAnimeListBaseUrl + "/topanime.php?limit=";
            return topAnimeUrl + page*50;
        }

        public HtmlNodeCollection AnchorNodes {
            get {
                const string urlClass = "hoverinfo_trigger fl-l ml12 mr8";
                return this.FindElementsWithClass(urlClass);
            }
        }

        /// <summary>
        /// Gets one page of urls of the top anime
        /// </summary>
        /// <param name="page">Page of anime to retrieve</param>
        /// <param name="retriesLeft">
        /// Number of times to retry if there are unexpected failures (usually network connectivity issues)
        /// </param>
        /// <returns>AnimeDetailsPage urls to the top anime on page <para>page</para></returns>
        public static List<string> ScrapeTopAnimeUrls(int page, int retriesLeft)
        {
            var topAnimePage = new AnimesController(GetTopAnimeUrl(page));
            
            var urls = new List<string>();

            HtmlNodeCollection anchorNodes = topAnimePage.AnchorNodes;

            if (anchorNodes == null) {
                Log.Error($"Page {page} is unable to parse the URLs. Retry count is {retriesLeft}");
                BackOff(retriesLeft);
                return retriesLeft == 0 ? new List<string>() : ScrapeTopAnimeUrls(page, retriesLeft - 1);
            }
            
            urls.AddRange(anchorNodes.Select(anchorNode => anchorNode.Attributes["href"].Value));
            return urls;
        }
    }
}