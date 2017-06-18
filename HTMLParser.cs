using System;
using System.Collections.Generic;
using System.Linq;
using AnimeExporter.Models;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace AnimeExporter {
    public class HtmlParser {

        /// <summary>
        /// Gets one page of urls of the top anime
        /// </summary>
        /// <param name="page">Page of anime to retrieve</param>
        /// <returns>MyAnimeList urls to the top anime on page <para>page</para></returns>
        public static List<string> GetTopAnimeUrls(int page)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(MyAnimeList.GetTopAnimeUrl(page));

            var urls = new List<string>();

            HtmlNodeCollection anchorNodes = MyAnimeList.GetAnchorNodes(doc.DocumentNode);
            urls.AddRange(anchorNodes.Select(anchorNode => anchorNode.Attributes["href"].Value));

            return urls;
        }

        public static Animes GetTopAnime(int page) {
            List<string> topAnimeUrls = GetTopAnimeUrls(page);

            var animes = new Animes();
            foreach (string url in topAnimeUrls) {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(url);
                HtmlNode docNode = doc.DocumentNode;
                
                try {
                    HtmlNode titleContainer =
                        MyAnimeList.FindElementsWithClass(MyAnimeList.AnimeTitleClass, docNode)[0];
                    HtmlNode title = titleContainer.ChildNodes[0];

                    Anime anime = new Anime (
                        title.InnerText,
                        url,
                        MyAnimeList.GetRating(docNode)
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