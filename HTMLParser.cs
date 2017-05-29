using System;
using System.Collections.Generic;
using System.Linq;
using AnimeExporter.Models;
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

            HtmlNodeCollection anchorNodes = FindElementsWithClass(MyAnimeListInfo.UrlClass, doc.DocumentNode);
            urls.AddRange(anchorNodes.Select(anchorNode => anchorNode.Attributes["href"].Value));

            return urls;
        }

        public static List<Anime> GetTopAnime(int page) {
            List<string> topAnimeUrls = GetTopAnimeUrls(page);

            var schema = new List<object> {
                "Title",
                "URL"
            };
            var animes = new List<Anime> {
                new Anime() {
                    Data = schema
                }
            };
            foreach (string url in topAnimeUrls) {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(url);

                try {
                    HtmlNode titleContainer =
                        FindElementsWithClass(MyAnimeListInfo.AnimeTitleClass, doc.DocumentNode)[0];
                    HtmlNode title = titleContainer.ChildNodes[0];

                    var data = new List<object> {
                        title.InnerText,
                        url
                    };
                    animes.Add(new Anime {
                     Data = data
                    });
                }
                catch {
                    Console.WriteLine("failed to add an anime");
                }
            }
            return animes;
        }

        public static HtmlNodeCollection FindElementsWithClass(string className, HtmlNode node) {
            string xPath = $"//*[contains(@class,'{className}')]";
            return node.SelectNodes(xPath);
        }
    }
}