using AnimeExporter.Controllers;
using AnimeExporter.Models;
using AnimeExporter.Views;

namespace AnimeExporter {
    internal class Program {

        public static void Main(string[] args) {
            AnimesModel topAnimesModel = AnimesController.ScrapeTopAnimes(0, 150);
            GoogleSheetView.BackupData();
            GoogleSheetView.PublishDataToGoogleSheet(topAnimesModel);
            GoogleSheetView.PublishGenresToGoogleSheet(topAnimesModel);
        }
    }
}