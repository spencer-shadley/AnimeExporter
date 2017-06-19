using System;

namespace AnimeExporter {
    internal class Program {

        public static void Main(string[] args) {
            const int numPages = 1;

            // Add the schema as its own "anime" so that we get nice titling in our Google Sheet
            Animes topAnimes = new Animes {Anime.Schema()}; 
            for (var i = 0; i < numPages; ++i) {
                Console.WriteLine("===============================");
                Console.WriteLine("===============================");
                Console.WriteLine("===============================");
                Console.WriteLine("===============================");
                Console.WriteLine($"==========   PAGE {i}  ==========");
                Console.WriteLine("===============================");
                Console.WriteLine("===============================");
                Console.WriteLine("===============================");
                Console.WriteLine("===============================");
                Console.WriteLine();
                topAnimes.Add(TopAnimePage.GetTopAnimes(i));
            }
            GoogleSheet.PublishGoogleSheet(topAnimes);
        }
    }
}