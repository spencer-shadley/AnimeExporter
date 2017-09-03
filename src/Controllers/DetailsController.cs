using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using AnimeExporter.Models;
using AnimeExporter.Utility;
using HtmlAgilityPack;

namespace AnimeExporter.Controllers {
    
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
    /// Note: This taking longer actaully helps to combat the rate throttling on MyAnimeList
    /// </remarks>
    public class AnimeDetailsPage : Page {
        
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string[] DateDelimter = {" to "}; // array is required for string.split() 

        private const string InvalidAiringMessage = "Unknown airing value: ";

        private const string NoBackground = "No background information has been added to this title. Help improve our database by adding background information";
        
        private readonly Airing _airingStatus;

        private enum Airing { Future, InProgress, Finished, Unknown }

        public AnimeDetailsPage(string url) : base(url) {
            this.FindRelatedAnime();
            
            switch (this.Status) {
                case "Not yet aired":
                    this._airingStatus = Airing.Future;
                    break;
                case "Currently Airing":
                    this._airingStatus = Airing.InProgress;
                    break;
                case "Finished Airing":
                    this._airingStatus = Airing.Finished;
                    break;
                case null:
                    this._airingStatus = Airing.Unknown;
                    break;
                default:
                    throw new InvalidEnumArgumentException(InvalidAiringMessage + this.Status);
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

        /// <summary>
        /// Grabs the type of media (movie, music, etc.)
        /// </summary>
        /// <remarks>
        /// MediaTypes on MyAnimeList follow two different structures. Sometimes they are hyperlinked and sometimes
        /// they are not. On the return this is checked to use the correct parsing.
        /// </remarks>
        public string MediaType {
            get {
                const string selector = "Type:";
                string xPath = $"//span[text() = '{selector}']";

                HtmlNodeCollection typeNodes = this.Node.SelectNodes(xPath);
                HtmlNode typeNode = typeNodes[0].NextSibling.NextSibling;

                return typeNode == null ? this.SelectValueAfterText(selector) : typeNode.InnerText;
            }
        }

        public string ImageUrl {
            get {
                const string xPath = "//div[@id='content']";
                HtmlNode table = this.Node.SelectSingleNode(xPath);
                HtmlNodeCollection images = table.SelectNodes("//img");
                return images[1].Attributes["src"].Value;
            }
        }
        
        public string Score => this.SelectValueOfItemProp("ratingValue");

        public string NumberOfRatings => this.SelectValueOfItemProp("ratingCount");

        public string Synopsis => this.SelectValueOfItemProp("description");

        public string Background {
            get {
                HtmlNode descriptionNode = this.SelectElementByItemProp("description");
                if (descriptionNode == null) return null;
                
                string description = WebUtility.HtmlDecode(descriptionNode.NextSibling.NextSibling.InnerText).Trim();
                return description.Equals(NoBackground) ? null : description;
            }
        }

        public string EnglishTitle => this.SelectValueAfterText("English:");

        public string JapaneseTitle => this.SelectValueAfterText("Japanese:");

        public string Synonyms => this.SelectValueAfterText("Synonyms:");

        public string Popularity => this.SelectValueAfterText("Popularity:").Substring(1); // substring to remove "#"

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
                switch (this._airingStatus) {
                    case Airing.Future:
                        return this.AirDates;
                    case Airing.InProgress:
                    case Airing.Finished:
                        return this.AirDates.Contains(DateDelimter[0]) ? this.AirDates.Split(DateDelimter, StringSplitOptions.None)[0] : this.AirDates;
                    case Airing.Unknown:
                        return "Unknown";
                    default:
                        throw new InvalidEnumArgumentException(InvalidAiringMessage + this._airingStatus);
                }
            }
        }

        public string AirFinishDate {
            get {
                switch (this._airingStatus) {
                    case Airing.Future:
                    case Airing.InProgress:
                        return "Still in progress";
                    case Airing.Finished:
                        return this.AirDates.Contains(DateDelimter[0]) ? this.AirDates.Split(DateDelimter, StringSplitOptions.None)[1] : this.AirDates;
                    case Airing.Unknown:
                        return "Unknown";
                    default:
                        throw new InvalidEnumArgumentException(InvalidAiringMessage + this._airingStatus);
                }
            }
        }

        // "Related" anime properties
        public string Adapation          { get; set; }
        public string AlternativeSetting { get; set; }
        public string Prequel            { get; set; }
        public string Sequel             { get; set; }
        public string Summary            { get; set; }
        public string ParentStory        { get; set; }
        public string FullStory          { get; set; }
        public string SideStory          { get; set; }
        public string SpinOff            { get; set; }
        public string Character          { get; set; }
        public string Other              { get; set; }
        public string AlternativeVersion { get; set; }
        
        /// <summary>
        /// There is a table of "Related" anime. This method walks the DOM for that table parsing each
        /// type of related anime.
        /// </summary>
        public void FindRelatedAnime() {
            HtmlNode table = this.FindElementWithClass("anime_detail_related_anime");

            // When an anime has no related animes the table is not generated
            if (table == null) { return; }
            
            HtmlNodeCollection rows = this.SelectElementsByType(table, "tr");
            foreach (HtmlNode row in rows) {
                HtmlNodeCollection values = row.ChildNodes[1].ChildNodes;

                string currRelatedAnime = string.Empty;
                foreach (HtmlNode node in values) {
                    BuildUrls(ref currRelatedAnime, node);
                }

                string text = row.InnerText;
                if (text.Contains("Adaptation:")) {
                    this.Adapation = currRelatedAnime;
                }
                else if (text.Contains("Alternative setting:")) {
                    this.AlternativeSetting = currRelatedAnime;
                }
                else if (text.Contains("Prequel:")) {
                    this.Prequel = currRelatedAnime;
                }
                else if (text.Contains("Sequel:")) {
                    this.Sequel = currRelatedAnime;
                }
                else if (text.Contains("Side story:")) {
                    this.SideStory = currRelatedAnime;
                }
                else if (text.Contains("Parent story:")) {
                    this.ParentStory = currRelatedAnime;
                }
                else if (text.Contains("Full story:")) {
                    this.FullStory = currRelatedAnime;
                }
                else if (text.Contains("Summary:")) {
                    this.Summary = currRelatedAnime;
                }
                else if (text.Contains("Spin-off:")) {
                    this.SpinOff = currRelatedAnime;
                }
                else if (text.Contains("Character:")) {
                    this.Character = currRelatedAnime;
                }
                else if (text.Contains("Other:")) {
                    this.Other = currRelatedAnime;
                }
                else if (text.Contains("Alternative version:")) {
                    this.AlternativeVersion = currRelatedAnime;
                }
                else {
                    Log.Error($"{text} wasn't scraped!");
                }
            }
        }

        /// <summary>
        /// Scrapes the details page to construct an anime object.
        /// </summary>
        /// <param name="retriesLeft">Number of times to retry</param>
        /// <returns>An <see cref="Anime"/> representation of the details page</returns>
        public Anime TryScrapeAnime(int retriesLeft) {
            Log.Info($"Scraping {this.Url}");
            
            try {
                string[] genres = this.Genres.Split(
                    new string[] { Delimiter }, StringSplitOptions.None);
                
                foreach (string genre in genres) {
                    Animes.Genres.Add(genre);
                }

                var anime = new Anime {
                    Url                   = { Value = this.Url },
                    Title                 = { Value = this.Title },
                    EnglishTitle          = { Value = this.EnglishTitle },
                    JapaneseTitle         = { Value = this.JapaneseTitle },
                    Synonyms              = { Value = this.Synonyms },
                    Score                 = { Value = this.Score },
                    NumRatings            = { Value = this.NumberOfRatings },
                    Rank                  = { Value = this.Rank },
                    Popularity            = { Value = this.Popularity },
                    NumMembers            = { Value = this.NumberOfMembers },
                    NumFavorites          = { Value = this.NumberOfFavorites },
                    MediaType             = { Value = this.MediaType },
                    NumEpisodes           = { Value = this.NumberOfEpisodes },
                    Status                = { Value = this.Status },
                    DateStarted           = { Value = this.AirStartDate },
                    DateFinished          = { Value = this.AirFinishDate },
                    Producers             = { Value = this.Producers },
                    Licensors             = { Value = this.Licensors },
                    Studios               = { Value = this.Studios },
                    Genres                = { Value = this.Genres },
                    Duration              = { Value = this.Duration },
                    Rating                = { Value = this.Rating },
                    Source                = { Value = this.Source },
                    Synopsis              = { Value = this.Synopsis },
                    Background            = { Value = this.Background },
                    Image                 = { Value = this.ImageUrl },
                    Adaptation            = { Value = this.Adapation },
                    AlternativeSetting    = { Value = this.AlternativeSetting },
                    Prequel               = { Value = this.Prequel },
                    Sequel                = { Value = this.Sequel },
                    SideStory             = { Value = this.SideStory },
                    ParentStory           = { Value = this.ParentStory },
                    FullStory             = { Value = this.FullStory },
                    Summary               = { Value = this.Summary },
                    SpinOff               = { Value = this.SpinOff },
                    Character             = { Value = this.Character },
                    Other                 = { Value = this.Other },
                    AlternativeVersion    = { Value = this.AlternativeVersion }
                };

                Log.Debug(anime + Environment.NewLine);
                return anime;
            }
            catch(Exception e) {
                Log.Error($"failed to export an anime (retry count is {retriesLeft})...", e);

                BackOff(retriesLeft);

                // typically network connectivity issues, see if we should try again
                return retriesLeft == 0 ? null : this.TryScrapeAnime(retriesLeft - 1);
            }
        }
    }
}
