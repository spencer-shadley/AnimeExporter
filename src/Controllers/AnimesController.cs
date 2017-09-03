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

        /// <summary>
        /// Gets the top anime between pages <see cref="startPage"/> and <see cref="lastPage"/>
        /// </summary>
        /// <param name="startPage">The page to begin scraping from</param>
        /// <param name="lastPage">The page to end scraping (if unspecified only the <see cref="startPage"/> will be scraped</param>
        /// <returns></returns>
        public static AnimesModel ScrapeTopAnimes(int startPage, int lastPage = -1) {

            Debug.Assert(startPage >= 0, "Start page must be at least 0");
            Debug.Assert(lastPage >= -1, "Last page must be at least -1");
            Debug.Assert(lastPage == -1 || startPage <= lastPage,
                "Either only the startPage should be specified or the startPage should be less than the lastPage");

            var animes = new AnimesModel {
                AnimeModel.Schema(), // Add the schema as its own "anime" so that we get nice titling in our Google Sheet
            };
            
            do {
                PrintPage(startPage);
                animes.Add(ScrapeTopAnimesPage(startPage));
            }
            while (startPage++ < lastPage);
            
            return animes;
        }

        /// <summary>
        /// Scrapes the anime from the top anime page at <see cref="page"/>
        /// </summary>
        /// <param name="page">Represents the page number to scrape</param>
        /// <returns>An <see cref="AnimesModel"/> representation of the top anime at <see cref="page"/></returns>
        public static AnimesModel ScrapeTopAnimesPage(int page) {
            var animes = new AnimesModel();
            List<string> topAnimeUrls = ScrapeTopAnimeUrls(page, MaxRetryCount);
            foreach (string url in topAnimeUrls) {
                animes.Add(AnimeController.ScrapeData(url));
            }
            return animes;
        }

        public static void PrintPage(int page) {
            Log.Info($@"
===============================
===============================
==========   PAGE {page}  ==========
===============================
===============================
            ");
        }
    }
}