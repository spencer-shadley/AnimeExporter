﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
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
        /// Scrapes the anime at the given <see cref="url"/> to construct an anime object. By default this
        /// will retry scraping the page twice due to inconsistent network connections before giving up.
        /// </summary>
        /// <param name="url">The url to scrape</param>
        /// <param name="retriesLeft">Number of times to retry</param>
        /// <returns>An <see cref="Anime"/> representation of the page at <see cref="url"/></returns>
        public static Anime TryScrapeAnime(string url, int retriesLeft) {
            Log.Info($"Scraping {url}");
            try {
                var animeDetailsPage = new AnimeDetailsPage(url);
                
                string[] genres = animeDetailsPage.Genres.Split(
                    new string[] { Delimiter }, StringSplitOptions.None);
                
                foreach (string genre in genres) {
                    Animes.Genres.Add(genre);
                }

                var anime = new Anime {
                    Url                   = { Value = url },
                    Title                 = { Value = animeDetailsPage.Title },
                    EnglishTitle          = { Value = animeDetailsPage.EnglishTitle },
                    JapaneseTitle         = { Value = animeDetailsPage.JapaneseTitle },
                    Synonyms              = { Value = animeDetailsPage.Synonyms },
                    Score                 = { Value = animeDetailsPage.Score },
                    NumRatings            = { Value = animeDetailsPage.NumberOfRatings },
                    Rank                  = { Value = animeDetailsPage.Rank },
                    Popularity            = { Value = animeDetailsPage.Popularity },
                    NumMembers            = { Value = animeDetailsPage.NumberOfMembers },
                    NumFavorites          = { Value = animeDetailsPage.NumberOfFavorites },
                    MediaType             = { Value = animeDetailsPage.MediaType },
                    NumEpisodes           = { Value = animeDetailsPage.NumberOfEpisodes },
                    Status                = { Value = animeDetailsPage.Status },
                    DateStarted           = { Value = animeDetailsPage.AirStartDate },
                    DateFinished          = { Value = animeDetailsPage.AirFinishDate },
                    Producers             = { Value = animeDetailsPage.Producers },
                    Licensors             = { Value = animeDetailsPage.Licensors },
                    Studios               = { Value = animeDetailsPage.Studios },
                    Genres                = { Value = animeDetailsPage.Genres },
                    Duration              = { Value = animeDetailsPage.Duration },
                    Rating                = { Value = animeDetailsPage.Rating },
                    Source                = { Value = animeDetailsPage.Source },
                    Synopsis              = { Value = animeDetailsPage.Synopsis },
                    Background            = { Value = animeDetailsPage.Background },
                    Image                 = { Value = animeDetailsPage.ImageUrl },
                    Adaptation            = { Value = animeDetailsPage.Adapation },
                    AlternativeSetting    = { Value = animeDetailsPage.AlternativeSetting },
                    Prequel               = { Value = animeDetailsPage.Prequel },
                    Sequel                = { Value = animeDetailsPage.Sequel },
                    SideStory             = { Value = animeDetailsPage.SideStory },
                    ParentStory           = { Value = animeDetailsPage.ParentStory },
                    Summary               = { Value = animeDetailsPage.Summary },
                    SpinOff               = { Value = animeDetailsPage.SpinOff },
                    Character             = { Value = animeDetailsPage.Character },
                    Other                 = { Value = animeDetailsPage.Other },
                    AlternativeVersion    = { Value = animeDetailsPage.AlternativeVersion }
                };

                Log.Debug(anime + Environment.NewLine);
                return anime;
            }
            catch(Exception e) {
                Log.Error($"failed to export an anime (retry count is {retriesLeft})...", e);

                BackOff(retriesLeft);

                // typically network connectivity issues, see if we should try again
                return retriesLeft == 0 ? null : TryScrapeAnime(url, retriesLeft - 1);
            }
        }
    }
}
