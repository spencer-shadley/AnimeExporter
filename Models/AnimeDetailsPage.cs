using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
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
        
        private static readonly string[] DateDelimter = {" to "}; // array is required for string.split() 

        private const string InvalidAiringMessage = "Unknown airing value: ";
        
        private readonly Airing AiringStatus;

        private enum Airing { Future, InProgress, Finished, Unknown }

        public AnimeDetailsPage(HtmlNode document) : base(document) {
            switch (Status) {
                case "Not yet aired":
                    AiringStatus = Airing.Future;
                    break;
                case "Currently Airing":
                    AiringStatus = Airing.InProgress;
                    break;
                case "Finished Airing":
                    AiringStatus = Airing.Finished;
                    break;
                case null:
                    AiringStatus = Airing.Unknown;
                    break;
                default:
                    throw new InvalidEnumArgumentException(InvalidAiringMessage + Status);
            }
        }

        public string Title {
            get {
                const string animeTitleClass = "h1";
                HtmlNode titleContainer = FindElementsWithClass(animeTitleClass)[0];
                HtmlNode title = titleContainer.ChildNodes[0];
                return title.InnerText;
            }
        }
        
        public string Rank {
            get {
                HtmlNodeCollection rankRows = FindElementsWithClass("js-statistics-info");
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

        public string Image {
            get {
                const string xPath = "//*[@id='content']/table/tbody/tr/td[1]/div/div[1]/a/img";
                var link = Doc.SelectSingleNode(xPath).GetAttributeValue("href");
                return link;
            }
        }
        
        public string Score => this.SelectValueOfItemProp("ratingValue");

        public string NumberOfRatings => this.SelectValueOfItemProp("ratingCount");

        public string Synopsis => this.SelectValueOfItemProp("description");

        public string Background {
            get {
                HtmlNode synopsis = this.SelectElementByItemProp("description");
                return WebUtility.HtmlDecode(synopsis.NextSibling.NextSibling.InnerText);
            }
        }

        public string Popularity => this.SelectValueAfterText("Popularity:").Substring(1);

        public string NumberOfMembers => this.SelectValueAfterText("Members:");

        public string NumberOfFavorites => this.SelectValueAfterText("Favorites:");

        public string NumberOfEpisodes => this.SelectValueAfterText("Episodes:");

        public string Status => this.SelectValueAfterText("Status:");

        public string AirDates => this.SelectValueAfterText("Aired:");

        public string Duration => this.SelectValueAfterText("Duration:");

        public string Rating => this.SelectValueAfterText("Rating:");

        public string Source => this.SelectValueAfterText("Source:");

        public string Producers => this.SelectAllSiblingAnchorElements("Producers:");

        public string Licensors => this.SelectAllSiblingAnchorElements("Licensors:");

        public string Studios => this.SelectAllSiblingAnchorElements("Studios:");

        public string Genres => this.SelectAllSiblingAnchorElements("Genres:");
        
        public string AirStartDate {
            get {
                switch (AiringStatus) {
                    case Airing.Future:
                        return AirDates;
                    case Airing.InProgress:
                    case Airing.Finished:
                        return AirDates.Contains(DateDelimter[0]) ?
                            AirDates.Split(DateDelimter, StringSplitOptions.None)[0] :
                            AirDates;
                    case Airing.Unknown:
                        return "Unknown";
                    default:
                        throw new InvalidEnumArgumentException(InvalidAiringMessage + AiringStatus);
                }
            }
        }

        public string AirFinishDate {
            get {
                switch (AiringStatus) {
                    case Airing.Future:
                    case Airing.InProgress:
                        return "Still in progress";
                    case Airing.Finished:
                        return AirDates.Contains(DateDelimter[0]) ?
                            AirDates.Split(DateDelimter, StringSplitOptions.None)[1] :
                            AirDates;
                    case Airing.Unknown:
                        return "Unknown";
                    default:
                        throw new InvalidEnumArgumentException(InvalidAiringMessage + AiringStatus);
                }
            }
        }

        /// <summary>
        /// Scrapes the anime at the given <see cref="url"/> to construct an anime object. By default this
        /// will retry scraping the page twice due to inconsistent network connections before giving up.
        /// </summary>
        /// <param name="url">The url to scrape</param>
        /// <param name="retryCount">Number of times to retry</param>
        /// <returns>An <see cref="Anime"/> representation of the page at <see cref="url"/></returns>
        public static Anime ScrapeAnime(string url, int retryCount = 5) {
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
                    animeDetailsPage.NumberOfEpisodes,
                    animeDetailsPage.Status,
                    animeDetailsPage.AirStartDate,
                    animeDetailsPage.AirFinishDate,
                    animeDetailsPage.Producers,
                    animeDetailsPage.Licensors,
                    animeDetailsPage.Studios,
                    animeDetailsPage.Genres,
                    animeDetailsPage.Duration,
                    animeDetailsPage.Rating,
                    animeDetailsPage.Source,
                    animeDetailsPage.Synopsis,
                    animeDetailsPage.Background,
                    animeDetailsPage.Image
                );
                    
                Console.WriteLine("Exported: " + anime + Environment.NewLine);
                return anime;
            }
            catch(Exception e) {
                Console.Error.WriteLine($"failed to export an anime (retry count is {retryCount})...");
                Console.Error.WriteLine(e.ToString());
                Console.WriteLine();

                // typically network connectivity issues, see if we should try again
                return retryCount == 0 ? Anime.Fail() : ScrapeAnime(url, retryCount - 1);
            }
        }
    }
}