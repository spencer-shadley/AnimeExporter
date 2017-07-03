using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace AnimeExporter {
    
    /// <summary>
    /// Represents the details page of an anime on MyAnimeList.net
    /// </summary>
    /// <example>https://myanimelist.net/anime/23273/Shigatsu_wa_Kimi_no_Uso?q=shigatsu</example>
    /// <remarks>
    /// Currently most things are selected via xPath and walking the entire DOM.
    /// This could be much faster by walking the DOM and collecting attributes in a single pass (or just
    /// walking a smaller more relevant DOM), however, the perf is currently bottlenecked on retrieving
    /// the webpage itself (each method takes ~0.12% of the overall program time) so I am going to avoid
    /// doing premature optimizations and focus on making this more robust, rather than faster.
    /// There are more perf details in the tracing output at data/performance snapshot.dtp
    /// </remarks>
    public class AnimeDetailsPage : Page {
        
        private static readonly string[] DateDelimter = {" to "}; // array is required for string.split() 

        private const string InvalidAiringMessage = "Unknown airing value: ";
        
        private readonly Airing _airingStatus;

        private enum Airing { Future, InProgress, Finished, Unknown }

        public AnimeDetailsPage(HtmlNode document) : base(document) {
            switch (Status) {
                case "Not yet aired":
                    _airingStatus = Airing.Future;
                    break;
                case "Currently Airing":
                    _airingStatus = Airing.InProgress;
                    break;
                case "Finished Airing":
                    _airingStatus = Airing.Finished;
                    break;
                case null:
                    _airingStatus = Airing.Unknown;
                    break;
                default:
                    throw new InvalidEnumArgumentException(InvalidAiringMessage + Status);
            }
        }

        public string Title {
            get {
                const string animeTitleClass = "h1";
                HtmlNode titleContainer = this.FindElementsWithClass(animeTitleClass)[0];
                HtmlNode title = titleContainer.ChildNodes[0];
                return title.InnerText;
            }
        }
        
        public string Rank {
            get {
                HtmlNodeCollection rankRows = this.FindElementsWithClass("js-statistics-info");
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
                const string xPath = "//div[@id='content']";
                HtmlNode table = Doc.SelectSingleNode(xPath);
                HtmlNodeCollection images = table.SelectNodes("//img");
                return images[1].Attributes["src"].Value;
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
                switch (_airingStatus) {
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
                        throw new InvalidEnumArgumentException(InvalidAiringMessage + _airingStatus);
                }
            }
        }

        public string AirFinishDate {
            get {
                switch (_airingStatus) {
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
                        throw new InvalidEnumArgumentException(InvalidAiringMessage + _airingStatus);
                }
            }
        }

        /// <summary>
        /// Scrapes the anime at the given <see cref="url"/> to construct an anime object. By default this
        /// will retry scraping the page twice due to inconsistent network connections before giving up.
        /// </summary>
        /// <param name="url">The url to scrape</param>
        /// <param name="retriesLeft">Number of times to retry</param>
        /// <returns>An <see cref="Anime"/> representation of the page at <see cref="url"/></returns>
        public static Anime ScrapeAnime(string url, int retriesLeft) {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            AnimeDetailsPage animeDetailsPage = new AnimeDetailsPage(doc.DocumentNode);
                
            try {
                
                // TODO: Use a dictionary instead
                // Key is the type of data (score, rank, etc.)
                
                string[] genres = animeDetailsPage.Genres.Split(
                    new string[] { Page.Delimiter }, StringSplitOptions.None);
                
                foreach (string genre in genres) {
                    Animes.Genres.Add(genre);
                }

                var anime = new Anime {
                    Title          = { Value = animeDetailsPage.Title },
                    Url            = { Value = url },
                    Score          = { Value = animeDetailsPage.Score },
                    NumRatings     = { Value = animeDetailsPage.NumberOfRatings },
                    Rank           = { Value = animeDetailsPage.Rank },
                    Popularity     = { Value = animeDetailsPage.Popularity },
                    NumMembers     = { Value = animeDetailsPage.NumberOfMembers },
                    NumFavorites   = { Value = animeDetailsPage.NumberOfFavorites },
                    MediaType      = { Value = animeDetailsPage.MediaType },
                    NumEpisodes    = { Value = animeDetailsPage.NumberOfEpisodes },
                    Status         = { Value = animeDetailsPage.Status },
                    DateStarted    = { Value = animeDetailsPage.AirStartDate },
                    DateFinished   = { Value = animeDetailsPage.AirFinishDate },
                    Producers      = { Value = animeDetailsPage.Producers },
                    Licensors      = { Value = animeDetailsPage.Licensors },
                    Studios        = { Value = animeDetailsPage.Studios },
                    Genres         = { Value = animeDetailsPage.Genres },
                    Duration       = { Value = animeDetailsPage.Duration },
                    Rating         = { Value = animeDetailsPage.Rating },
                    Source         = { Value = animeDetailsPage.Source },
                    Synopsis       = { Value = animeDetailsPage.Synopsis },
                    Background     = { Value = animeDetailsPage.Background },
                    Image          = { Value = animeDetailsPage.Image }
                };

                Console.WriteLine(anime + Environment.NewLine);
                return anime;
            }
            catch(Exception e) {
                Console.Error.WriteLine($"failed to export an anime (retry count is {retriesLeft})...");
                Console.Error.WriteLine(e.ToString());

                BackOff(retriesLeft);

                // typically network connectivity issues, see if we should try again
                return retriesLeft == 0 ? Anime.Fail() : ScrapeAnime(url, retriesLeft - 1);
            }
        }
    }
}
