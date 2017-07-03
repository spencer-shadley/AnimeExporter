namespace AnimeExporter {
    internal class Program {

        public static void Main(string[] args) {
            Animes topAnimes = TopAnimePage.ScrapeTopAnimes(0);
            GoogleSheet.PublishDataToGoogleSheet(topAnimes);
            GoogleSheet.PublishGenresToGoogleSheet(topAnimes);
        }
    }
}