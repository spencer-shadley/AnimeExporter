namespace AnimeExporter {
    internal class Program {

        public static void Main(string[] args) {
            Animes topAnimes = TopAnimePage.TryScrapeTopAnimes(0, 150);
            GoogleSheet.BackupData();
            GoogleSheet.PublishDataToGoogleSheet(topAnimes);
            GoogleSheet.PublishGenresToGoogleSheet(topAnimes);
        }
    }
}