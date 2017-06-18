using System;
using System.Diagnostics;
using HtmlAgilityPack;

namespace AnimeExporter.Models {
    
    /// <summary>
    /// Represents the details page of an anime
    /// </summary>
    /// <remarks>
    /// This could be much faster walking the DOM and collecting attributes, however,
    /// the perf is currently bottlenecked on retrieving the webpage itself
    /// (each method takes ~0.12% of the overall program time) so I am going to avoid doing
    /// premature optimizations and focus on making this more robust, rather than faster.
    /// </remarks>
    public class AnimeDetailsPage : Page {

        public AnimeDetailsPage(HtmlNode document) : base(document) { }

        public string Title {
            get {
                const string animeTitleClass = "h1";
                HtmlNode titleContainer = FindElementsWithClass(Doc, animeTitleClass)[0];
                HtmlNode title = titleContainer.ChildNodes[0];
                return title.InnerText;
            }
        }
        
        public string Rank {
            get {
                HtmlNodeCollection rankRows = FindElementsWithClass(Doc, "js-statistics-info");
                Debug.Assert(rankRows.Count == 2);

                HtmlNode rankRow = rankRows[1];
                Debug.Assert(rankRow.ChildNodes.Count >= 2);

                HtmlNode rank = rankRow.ChildNodes[2];

                return rank.InnerText.Trim().Substring(1);
            }
        }

        public string MediaType {
            get {
                const string xPath = "//span[text() = 'Type:']";

                HtmlNodeCollection typeNodes = Doc.SelectNodes(xPath);
                HtmlNode typeNode = typeNodes[0].NextSibling.NextSibling;
                return typeNode.InnerText;
            }
        }
        
        public string Score => this.SelectValueOfItemProp("ratingValue");

        public string NumberOfRatings => this.SelectValueOfItemProp("ratingCount");

        public string Popularity => this.SelectValueAfterText("Popularity:").Substring(1);

        public string NumberOfMembers => this.SelectValueAfterText("Members:");

        public string NumberOfFavorites => this.SelectValueAfterText("Favorites:");

        public string NumberOfEpisodes => this.SelectValueAfterText("Episodes:");

        /// <summary>
        /// Scrapes the anime at the given <see cref="url"/> to construct an anime object. By default this
        /// will retry scraping the page twice due to inconsistent network connections before giving up.
        /// </summary>
        /// <param name="url">The url to scrape</param>
        /// <param name="retryCount">Number of times to retry</param>
        /// <returns>An <see cref="Anime"/> representation of the page at <see cref="url"/></returns>
        public static Anime ScrapeAnime(string url, int retryCount = 2) {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            AnimeDetailsPage animeDetailsPage = new AnimeDetailsPage(doc.DocumentNode);
                
            try {
                Anime anime = new Anime (
                    animeDetailsPage.Title,
                    url,
                    animeDetailsPage.Score,
                    animeDetailsPage.NumberOfRatings,
                    animeDetailsPage.Rank,
                    animeDetailsPage.Popularity,
                    animeDetailsPage.NumberOfMembers,
                    animeDetailsPage.NumberOfFavorites,
                    animeDetailsPage.MediaType,
                    animeDetailsPage.NumberOfEpisodes
                );
                    
                Console.WriteLine("Exported: " + anime + Environment.NewLine);
                return anime;
            }
            catch(Exception e) {
                Console.Error.WriteLine($"failed to export an anime (retry count is {retryCount})...");
                Console.Error.WriteLine(e.ToString());
                Console.WriteLine();

                // typically network connectivity issues, see if we should try again
                return retryCount == 0 ? null : ScrapeAnime(url, retryCount - 1);
            }
        }
    }
}