using System;
using System.Collections.Generic;

namespace AnimeExporter.Models {
    public class Anime {

        public readonly List<object> Attributes = new List<object>();

        private static readonly object[] SchemaAttributes = {
            "Title", "URL", "Score", "Number of ratings", "Rank", "Popularity",
            "Number of Members", "Number of Favorites", "Type of Media", "Number of Episodes",
            "Status", "Date Started Airing", "Date Finished Airing"
        
            // TODO: the below fields
            // categories
            // synopsis
            // background
            // adaptation
            // summary
            // sequel
            // related
            // rating
            // type
            // number of episodes
            // status
            // aired
            // permiered
            // broadcast
            // producers
            // licensors
            // studios
            // source
            // duration
            // link to image
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
            return string.Join(Environment.NewLine, Attributes);
        }
    }
}