using AnimeExporter.Models;
using AnimeExporter.Views;

namespace AnimeExporter {
    internal class Program {

        public static void Main(string[] args) {
            AnimesModel animes = AnimesView.ScrapeTopAnimes(0, 100);
            GoogleSheetView.BackupData();
            GoogleSheetView.PublishDataToGoogleSheet(animes);
            GoogleSheetView.PublishGenresToGoogleSheet(animes);
        }
    }
}