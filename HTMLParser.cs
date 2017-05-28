using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace AnimeExporter {
    public class HtmlParser {

        // METHODS
        
        /// <summary>
        /// Gets one page of urls of the top anime
        /// </summary>
        /// <param name="page">Page of anime to retrieve</param>
        /// <returns>MyAnimeList urls to the top anime on page <para>page</para></returns>
        public static List<string> GetTopAnimeUrls(int page)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(MyAnimeListInfo.GetTopAnimeUrl(page));

            var urls = new List<string>();

            HtmlNodeCollection anchorNodes = FindElementsWithClass("hoverinfo_trigger fl-l ml12 mr8", doc.DocumentNode);
            urls.AddRange(anchorNodes.Select(anchorNode => anchorNode.Attributes["href"].Value));

            return urls;
        }

        public static List<Anime> GetTopAnime(int page) {
            List<string> topAnimeUrls = GetTopAnimeUrls(page);
            
            var animes = new List<Anime>();
            foreach (string url in topAnimeUrls) {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(url);
                
                animes.Add(new Anime {
                    Url = url
                });
            }
            return animes;
        }

        public static HtmlNodeCollection FindElementsWithClass(string className, HtmlNode node) {
            string xPath = $"//*[contains(@class,'{className}')]";
            return node.SelectNodes(xPath);
        }
    }
}