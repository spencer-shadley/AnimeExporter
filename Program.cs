using System;

namespace AnimeExporter {
    internal class Program {
        public static void Main(string[] args) {
            Console.WriteLine("anime!");

            // get anime as HTML from URL
            Console.WriteLine(MyAnimeListInfo.TopAnimeUrl);
            var htmlParser = new HtmlParser(MyAnimeListInfo.TopAnimeUrl);
            Console.WriteLine(htmlParser.Html);

            // parse HTML

            // make lots of requests to get detailed anime info (".hoverinfo_trigger")

            // parse this to some nice code

            // publish to a Google Sheet

            // connect Tableau to that Sheet

            // Make some cool vizzes

            // Publish to Tableau Online
        }
    }
}