using AnimeExporter.Models;
using AnimeExporter.Utility;
using HtmlAgilityPack;

namespace AnimeExporter.Controllers {
    public class CharactersController : Page {
        
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HtmlNode FirstCharacterTable { get; }

        public CharactersController(string url) : base(url + "/characters") {
            this.FirstCharacterTable = this.FindFirstCharacterTable();
        }

        private HtmlNode FindFirstCharacterTable() {
            HtmlNodeCollection addCharacterAnchors = this.SelectByTypeContainsText("a", "Add character");
            if (addCharacterAnchors?.Count != 1) {
                Log.Error($"Unexpected number of anchors for {this.Url}");
                return null;
            }
            
            HtmlNode addCharacterAnchor = addCharacterAnchors[0];
            HtmlNode table = addCharacterAnchor.ParentNode.ParentNode.NextSibling; // there isn't a clean way to get this non-unique table
            if (table.Name == "table") return table;
            
            Log.Error($"DOM traversal failed for character table in {this.Url}");
            return null;
        }

        private string IsInLanguage(string language) {
            return (this.SelectByTypeContainsText("small", language, this.FirstCharacterTable) != null).ToString();
        }
        
        protected override DataModel Scrape() {
            return new CharactersModel {
                IsInJapanese  = { Value = this.IsInLanguage("Japanese")},
                IsInEnglish   = { Value = this.IsInLanguage("English")},
                IsInSpanish   = { Value = this.IsInLanguage("Spanish")},
                IsInFrench    = { Value = this.IsInLanguage("French")},
                IsInKorean    = { Value = this.IsInLanguage("Korean")},
                IsInItalian   = { Value = this.IsInLanguage("Italian")},
                IsInBrazilian = { Value = this.IsInLanguage("Brazilian")},
                IsInGerman    = { Value = this.IsInLanguage("German")},
                IsInHungarian = { Value = this.IsInLanguage("Hungarian")}
            };
        }
    }
}