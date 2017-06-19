using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace AnimeExporter.Models {
    
    /// <remarks>
    /// As of 6/18/17 There are 12,972 animes on MyAnimeList in the "top" section
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
        /// <returns>AnimeDetailsPage urls to the top anime on page <para>page</para></returns>
        public static List<string> GetTopAnimeUrls(int page)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(GetTopAnimeUrl(page));
            TopAnimePage topAnimePage = new TopAnimePage(doc.DocumentNode);
            
            var urls = new List<string>();

            HtmlNodeCollection anchorNodes = topAnimePage.GetAnchorNodes();
            urls.AddRange(anchorNodes.Select(anchorNode => anchorNode.Attributes["href"].Value));

            return urls;
        }

        public static Animes GetTopAnimes(int page) {
            List<string> topAnimeUrls = GetTopAnimeUrls(page);
            var animes = new Animes();
            foreach (string url in topAnimeUrls) {
                animes.Add(AnimeDetailsPage.ScrapeAnime(url));
            }
            return animes;
        }
    }
}