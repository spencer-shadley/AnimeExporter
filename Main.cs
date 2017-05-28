using System;
using System.Collections.Generic;
using AnimeExporter.Models;

namespace AnimeExporter {
    internal class Program {
        public static void Main(string[] args) {
            List<Anime> topAnime = HtmlParser.GetTopAnime(1);
            Console.WriteLine(string.Join(Environment.NewLine, topAnime));
        }
    }
}