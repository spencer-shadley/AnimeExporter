using System;
using System.Collections.Generic;

namespace AnimeExporter {
    internal class Program {
        public static void Main(string[] args) {

            // get anime as HTML from URL
            //List<string> topAnimeUrls = HtmlParser.GetTopAnimeUrls(1);
            //Console.WriteLine(string.Join(Environment.NewLine, topAnimeUrls));

            List<Anime> topAnime = HtmlParser.GetTopAnime(1);

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