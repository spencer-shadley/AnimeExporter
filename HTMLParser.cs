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

        public string GetAnimeInfo(string name) {
            return null;
        }
    }
}