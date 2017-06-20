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
        public TopAnimePage(HtmlNode document) : base(document) { }
        
        public static string GetTopAnimeUrl(int page) {
            const string topAnimeUrl = "https://myanimelist.net/topanime.php?limit=";
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
        /// <param name="retryCount">
        /// Number of times to retry if there are unexpected failures (usually network connectivity issues)
        /// </param>
        /// <returns>AnimeDetailsPage urls to the top anime on page <para>page</para></returns>
        public static List<string> ScrapeTopAnimeUrls(int page, int retryCount = 10)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(GetTopAnimeUrl(page));
            TopAnimePage topAnimePage = new TopAnimePage(doc.DocumentNode);
            
            var urls = new List<string>();

            HtmlNodeCollection anchorNodes = topAnimePage.GetAnchorNodes();

            if (anchorNodes == null) {
                Console.Error.WriteLine($"Page {page} is unable to parse the URLs. Retry count is {retryCount}");
                return retryCount == 0 ? new List<string>() : ScrapeTopAnimeUrls(page, retryCount - 1);
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
        public static Animes ScrapeTopAnimes(int startPage, int lastPage = -1) {

            Debug.Assert(startPage >= 0, "Start page must be at least 0");
            Debug.Assert(lastPage >= -1, "Last page must be at least -1");
            Debug.Assert(lastPage == -1 || startPage <= lastPage,
                "Either only the startPage should be specified or the startPage should be less than the lastPage");

            Animes animes = new Animes {
                Anime.Schema(), // Add the schema as its own "anime" so that we get nice titling in our Google Sheet
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
        /// <returns>An <see cref="Animes"/> representation of the top anime at <see cref="page"/></returns>
        public static Animes ScrapeTopAnimesPage(int page) {
            Animes animes = new Animes();
            List<string> topAnimeUrls = ScrapeTopAnimeUrls(page);
            foreach (string url in topAnimeUrls) {
                animes.Add(AnimeDetailsPage.ScrapeAnime(url));
            }
            return animes;
        }

        public static void PrintPage(int page) {
            Console.WriteLine("===============================");
            Console.WriteLine("===============================");
            Console.WriteLine($"==========   PAGE {page}  ==========");
            Console.WriteLine("===============================");
            Console.WriteLine("===============================");
            Console.WriteLine();
        }
    }
}