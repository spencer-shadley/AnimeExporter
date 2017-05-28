using HtmlAgilityPack;

namespace AnimeExporter {
    public class HtmlParser {
        
        // PROPERTIES

        public string Html { get; set; }

        // TODO: a lot
        public string[] AnimeTitles {
            get { return null; }
        }

        // CONSTRUCTORS

        public HtmlParser(string url) {
            Html = NetworkHandler.GetHtml(url);
        }
        
        // METHODS

        public string GetAnimeInfo(string name) {
            return null;
        }

        public HtmlDocument GetTopAnime(int limit = 100)
        {
            return new HtmlDocument("");
        }
    }
}