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
    }
}