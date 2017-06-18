using System.Diagnostics;
using HtmlAgilityPack;

namespace AnimeExporter.Models {
    public static class MyAnimeList {
        
        public const string TopAnimeBaseUrl = "https://myanimelist.net/topanime.php";
        public const string UrlClass = "hoverinfo_trigger fl-l ml12 mr8";
        public const string AnimeTitleClass = "h1";
        
        public static string GetTopAnimeUrl(int page) {
            return TopAnimeBaseUrl + "?limit=" + page*50;
        }

        public static HtmlNodeCollection GetAnchorNodes(HtmlNode node) {
            return FindElementsWithClass(UrlClass, node);
        }

        public static string GetRating(HtmlNode node) {
            const string xPath = "//span[@itemprop=\"ratingValue\"]";
            HtmlNodeCollection nodes = node.SelectNodes(xPath);
            Debug.Assert(nodes.Count == 1);
            HtmlNode ratingNode = nodes[0];
            return ratingNode.InnerText;
        }

        public static HtmlNodeCollection FindElementsWithClass(string className, HtmlNode node) {
            string xPath = $"//*[contains(@class,'{className}')]";
            return node.SelectNodes(xPath);
        }
    }
}