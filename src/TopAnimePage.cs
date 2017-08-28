using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HtmlAgilityPack;

namespace AnimeExporter {
    
    /// <summary>
    /// Represents the top anime page(s) on MyAnimeList.net
    /// </summary>
    /// <example>https://myanimelist.net/topanime.php?limit=5000</example>
    /// <remarks>
    /// As of 6/18/17 There are 12,972 animes on MyAnimeList in the "top" section.
    /// In one test it took 29.3 minutes to scrape all top anime between pages 0 and 100 (2,850 animes.)
    /// </remarks> 
    public class TopAnimePage : Page {
    
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public TopAnimePage(string url) : base(url) { }
        
        public static string GetTopAnimeUrl(int page) {
            const string topAnimeUrl = MyAnimeListBaseUrl + "/topanime.php?limit=";
            return topAnimeUrl + page*50;
        }

        public HtmlNodeCollection GetAnchorNodes() {
            const string urlClass = "hoverinfo_trigger fl-l ml12 mr8";
            return this.FindElementsWithClass(urlClass);
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
            var topAnimePage = new TopAnimePage(GetTopAnimeUrl(page));
            
            var urls = new List<string>();

            HtmlNodeCollection anchorNodes = topAnimePage.GetAnchorNodes();

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
        /// <param name="retriesLeft">How many times to retry if there are network issues</param>
        /// <returns></returns>
        public static Animes TryScrapeTopAnimes(int startPage, int lastPage = -1, int retriesLeft = 10) {

            Debug.Assert(startPage >= 0, "Start page must be at least 0");
            Debug.Assert(lastPage >= -1, "Last page must be at least -1");
            Debug.Assert(lastPage == -1 || startPage <= lastPage,
                "Either only the startPage should be specified or the startPage should be less than the lastPage");

            var animes = new Animes {
                Anime.Schema(), // Add the schema as its own "anime" so that we get nice titling in our Google Sheet
            };
            
            do {
                PrintPage(startPage);
                try {
                    animes.Add(ScrapeTopAnimesPage(startPage));
                }
                catch (Exception e) {
                    string errorMessage = $"failed to scrape page {startPage} (retry count is {retriesLeft})...";
                    Log.Error(errorMessage, e);
                    
                    BackOff(retriesLeft);

                    // typically network connectivity issues, see if we should try again
                    return retriesLeft == 0 ? animes : TryScrapeTopAnimes(startPage, lastPage, retriesLeft - 1);
                }
            }
            while (startPage++ < lastPage);
            
            return animes;
        }

        /// <summary>
        /// Scrapes the anime from the top anime page at <see cref="page"/>
        /// </summary>
        /// <param name="page">Represents the page number to scrape</param>
        /// <returns>An <see cref="Animes"/> representation of the top anime at <see cref="page"/></returns>
        public static Animes ScrapeTopAnimesPage(int page) {
            var animes = new Animes();
            List<string> topAnimeUrls = ScrapeTopAnimeUrls(page, MaxRetryCount);
            foreach (string url in topAnimeUrls) {
                Anime scrapedAnime = AnimeDetailsPage.TryScrapeAnime(url, MaxRetryCount);
                
                if (scrapedAnime == null) {
                    continue;
                }
                
                animes.Add(scrapedAnime);
            }
            return animes;
        }

        public static void PrintPage(int page) {
            Log.Info(@"
===============================
===============================
==========   PAGE {page}  ==========
===============================
===============================
            ");
        }
    }
}