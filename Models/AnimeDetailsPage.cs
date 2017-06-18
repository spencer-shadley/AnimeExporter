using System;
using System.Diagnostics;
using HtmlAgilityPack;

namespace AnimeExporter.Models {
    public class AnimeDetailsPage : Page {

        public AnimeDetailsPage(HtmlNode document) : base(document) { }

        public string GetTitle() {
            const string animeTitleClass = "h1";
            HtmlNode titleContainer = FindElementsWithClass(Doc, animeTitleClass)[0];
            HtmlNode title = titleContainer.ChildNodes[0];
            return title.InnerText;
        }
        
        /// <summary>
        /// Gets the score of the anime 
        /// </summary>
        /// <remarks>
        /// Could be much faster by walking a smaller DOM, however, the perf is currently bottlenecked
        /// on retrieving the webpage itself (this method takes 0.12% of the overall program time.)
        /// </remarks>
        public string GetScore() {
            const string xPath = "//span[@itemprop=\"ratingValue\"]";
            return GetValue(Doc, xPath);
        }

        public string GetNumberOfRatings() {
            const string xPath = "//span[@itemprop=\"ratingCount\"]";
            return GetValue(Doc, xPath);
        }

        public string GetRank() {
            HtmlNodeCollection rankRows = FindElementsWithClass(Doc, "js-statistics-info");
            Debug.Assert(rankRows.Count == 2);

            HtmlNode rankRow = rankRows[1];
            Debug.Assert(rankRow.ChildNodes.Count >= 2);
            
            HtmlNode rank = rankRow.ChildNodes[2];
            
            return rank.InnerText.Trim().Substring(1);
        }

        public string GetPopularity() {
            const string xPath = "//span[text() = 'Popularity:']";
            return this.GetValueAfter(xPath).Substring(1);
        }

        public string GetNumberOfMembers() {
            const string xPath = "//span[text() = 'Members:']";
            return this.GetValueAfter(xPath);
        }

        /// <summary>
        /// Many of the statistics are stored as floating plaintext after an element. This method makes it
        /// easier to grab that floating text.
        /// </summary>
        /// <returns>The trimmed InnerText of the next child of the node at <see cref="xPath"/></returns>
        public string GetValueAfter(string xPath) {
            HtmlNodeCollection selectedNodes = Doc.SelectNodes(xPath);
            Debug.Assert(selectedNodes.Count == 1);

            HtmlNode valueNode = selectedNodes[0].NextSibling;
            
            return valueNode.InnerText.Trim();
        }

        public static Anime ScrapeAnime(string url, int retryCount = 2) {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            AnimeDetailsPage animeDetailsPage = new AnimeDetailsPage(doc.DocumentNode);
                
            try {
                Anime anime = new Anime (
                    animeDetailsPage.GetTitle(),
                    url,
                    animeDetailsPage.GetScore(),
                    animeDetailsPage.GetNumberOfRatings(),
                    animeDetailsPage.GetRank(),
                    animeDetailsPage.GetPopularity(),
                    animeDetailsPage.GetNumberOfMembers()
                );
                    
                Console.WriteLine("Exported: " + anime + Environment.NewLine);
                return anime;
            }
            catch(Exception e) {
                Console.Error.WriteLine($"failed to export an anime (retry count is {retryCount})..."); // typically network connectivity issues
                Console.Error.WriteLine(e.ToString());
                Console.WriteLine();

                return retryCount == 0 ? null : ScrapeAnime(url, retryCount - 1);
            }
        }
    }
}