using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace AnimeExporter.Models {
    public class TopAnimePage : Page {
        public TopAnimePage(HtmlNode document) : base(document) { }
        
        public static string GetTopAnimeUrl(int page) {
            const string topAnimeUrl = "https://myanimelist.net/topanime.php?limit=";
            return topAnimeUrl + page*50;
        }

        public HtmlNodeCollection GetAnchorNodes() {
            const string urlClass = "hoverinfo_trigger fl-l ml12 mr8";
            return FindElementsWithClass(Doc, urlClass);
        }
        
        /// <summary>
        /// Gets one page of urls of the top anime
        /// </summary>
        /// <param name="page">Page of anime to retrieve</param>
        /// <returns>AnimeDetailsPage urls to the top anime on page <para>page</para></returns>
        public static List<string> GetTopAnimeUrls(int page)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(TopAnimePage.GetTopAnimeUrl(page));
            TopAnimePage topAnimePage = new TopAnimePage(doc.DocumentNode);
            
            var urls = new List<string>();

            HtmlNodeCollection anchorNodes = topAnimePage.GetAnchorNodes();
            urls.AddRange(anchorNodes.Select(anchorNode => anchorNode.Attributes["href"].Value));

            return urls;
        }

        public static Animes GetTopAnime(int page) {
            List<string> topAnimeUrls = GetTopAnimeUrls(page);

            var animes = new Animes();
            foreach (string url in topAnimeUrls) {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(url);
                AnimeDetailsPage animeDetailsPage = new AnimeDetailsPage(doc.DocumentNode);
                
                try {
                    Anime anime = new Anime (
                        animeDetailsPage.GetTitle(),
                        url,
                        animeDetailsPage.GetScore(),
                        animeDetailsPage.GetNumberOfRatings()
                    );
                    animes.Add(anime);
                    
                    Console.WriteLine("Exported: " + anime + Environment.NewLine);
                }
                catch(Exception e) {
                    Console.Error.WriteLine("failed to export an anime..."); // typically network connectivity issues
                    Console.Error.WriteLine(e.ToString());
                    Console.WriteLine();
                }
            }
            return animes;
        }
    }
}