using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeExporter.Models {
    public class Anime {

        public readonly List<object> Attributes = new List<object>();

        private static readonly object[] SchemaAttributes = {
            "Title", "URL", "Score", "Number of ratings", "Rank", "Popularity",
            "Number of Members", "Number of Favorites", "Type of Media", "Number of Episodes",
            "Status", "Date Started Airing", "Date Finished Airing", "Producers", "Licensors",
            "Studios", "Genres", "Duration", "Rating", "Source", "Synopsis", "Background", "Image"
        
            // TODO: the below fields
            // adaptation
            // sequel
            // related
            // English title
            // synonyms
            // Japanese title
        };

        public Anime(params object[] attributes) {
            foreach (object attribute in attributes) {
                Attributes.Add(attribute);
            }
        }

        public static Anime Schema() {
            return new Anime(SchemaAttributes);
        }

        public static Anime Fail() {
            return new Anime("Failed to export this anime");
        }
        
        public override string ToString() {
            return string.Join(Environment.NewLine,
                Attributes.Where(attribute => attribute.ToString().Length < 100)); // avoid things like synposis
        }
    }
}