using System.Collections.Generic;
using System.Diagnostics;
using AnimeExporter.Controllers;
using AnimeExporter.Models;
using AnimeExporter.Utility;

namespace AnimeExporter.Views {
    public class AnimesView {
    
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the top anime between pages <see cref="startPage"/> and <see cref="lastPage"/>
        /// </summary>
        /// <param name="startPage">The page to begin scraping from</param>
        /// <param name="lastPage">The page to end scraping (if unspecified only the <see cref="startPage"/> will be scraped</param>
        /// <returns></returns>
        public static AnimesModel ScrapeTopAnimes(int startPage, int lastPage = -1) {

            Debug.Assert(startPage >= 0, "Start page must be at least 0");
            Debug.Assert(lastPage >= -1, "Last page must be at least -1");
            Debug.Assert(lastPage == -1 || startPage <= lastPage,
                "Either only the startPage should be specified or the startPage should be less than the lastPage");

            var animes = new AnimesModel {
                AnimeModel.Schema(), // Add the schema as its own "anime" so that we get nice titling in our Google Sheet
            };
            
            do {
                PrintPage(startPage);
                animes.Add(ScrapeTopAnimesPage(startPage));
            }
            while (startPage++ < lastPage);
            
            return animes;
        }

        /// <summary>
        /// Scrapes the anime from the top anime page at <see cref="page"/>
        /// </summary>
        /// <param name="page">Represents the page number to scrape</param>
        /// <returns>An <see cref="AnimesModel"/> representation of the top anime at <see cref="page"/></returns>
        public static AnimesModel ScrapeTopAnimesPage(int page) {
            var animesModel = new AnimesModel();
            List<string> topAnimeUrls = AnimesController.ScrapeTopAnimeUrls(page, Page.MaxRetryCount);
            foreach (string url in topAnimeUrls) {
                animesModel.Add(AnimeController.ScrapeData(url));
            }
            return animesModel;
        }

        public static void PrintPage(int page) {
            Log.Info($@"
===============================
===============================
==========   PAGE {page}  ==========
===============================
===============================
            ");
        }
    }
}