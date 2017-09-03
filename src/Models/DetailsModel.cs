using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AnimeExporter.Models {
    
    public class Anime {

        // TODO: add summary stats (https://myanimelist.net/anime/1794/Dirty_Pair_no_Ooshoubu__Nolandia_no_Nazo/stats)
            // Watching
            // Completed
            // On-Hold
            // Dropped
            // Plan to Watch
            // Total
            // number of votes for each score
            // percent of votes for each score
        
        // TODO: Add  Characters & Staff (https://myanimelist.net/anime/1794/Dirty_Pair_no_Ooshoubu__Nolandia_no_Nazo/characters)
            // Characters
            // Voice actors
            // Staff
            // Dub Info (check if the voice actors have "English")
        
        // TODO: add streaming availability (https://myanimelist.net/anime/6045/Kimi_ni_Todoke/video?provider_id=2&subdub_type=sub)
            // Provided by (Hulu, Crunchyroll, etc.)
            // link to PV
        
        // TODO: add users (https://myanimelist.net/users.php?q=&loc=&agelow=1&agehigh=100&g=)
        
        // TODO: External Links
        
        public Attribute Title                = "Title";
        public Attribute Url                  = "URL";
        public Attribute EnglishTitle         = "English Title";
        public Attribute JapaneseTitle        = "Japanese Title";
        public Attribute Synonyms             = "Synonyms";
        public Attribute Score                = "Score";
        public Attribute NumRatings           = "Number of ratings";
        public Attribute Rank                 = "Rank";
        public Attribute Popularity           = "Popularity";
        public Attribute NumMembers           = "Number of Members";
        public Attribute NumFavorites         = "Number of Favorites";
        public Attribute MediaType            = "Type of Media";
        public Attribute NumEpisodes          = "Number of Episodes";
        public Attribute Status               = "Status";
        public Attribute DateStarted          = "Date Started Airing";
        public Attribute DateFinished         = "Date Finished Airing";
        public Attribute Producers            = "Producers";
        public Attribute Licensors            = "Licensors";
        public Attribute Studios              = "Studios";
        public Attribute Genres               = "Genres";
        public Attribute Duration             = "Duration";
        public Attribute Rating               = "Rating";
        public Attribute Source               = "Source";
        public Attribute Synopsis             = "Synopsis";
        public Attribute Background           = "Background";
        public Attribute Image                = "Image";
        public Attribute Adaptation           = "Adaptation";
        public Attribute AlternativeSetting   = "Alternative Setting";
        public Attribute Prequel              = "Prequel";
        public Attribute Sequel               = "Sequel";
        public Attribute Summary              = "Summary";
        public Attribute ParentStory          = "Parent story";
        public Attribute FullStory            = "Full story";
        public Attribute SideStory            = "Side story";
        public Attribute SpinOff              = "Spin-off";
        public Attribute Character            = "Character";
        public Attribute Other                = "Other";
        public Attribute AlternativeVersion   = "Alternative Version";
        
        /// <summary>
        /// Gets all <see cref="Attribute"/>
        /// </summary>
        /// <remarks>Uses reflection to gather every field of type <see cref="Attribute"/></remarks>
        public List<Attribute> AllAttributes {
            get {
                Type type = this.GetType();
                FieldInfo[] fields = type.GetFields();
                IEnumerable<object> values = fields.Select(field => field.GetValue(this));
                List<object> list = values.ToList();
                return list.Cast<Attribute>().ToList();
            }
        }
        
        /// <summary>
        /// Gets the schema based on each attribute in the anime
        /// </summary>
        /// <remarks>
        /// Currently <see cref="Attribute"/> defaults value to the schema value
        /// so only a default anime needs to be constructed.
        /// </remarks>
        public static Anime Schema() {
            return new Anime();
        }

        /// <summary>
        /// Converts to a row of data which is expected to be used in a table for publishing later
        /// </summary>
        public List<object> ToRow() {
            return this.AllAttributes.Select(attribute => attribute.Value).Cast<object>().ToList();
        }

        public override string ToString() {
            return this.AllAttributes.Aggregate(string.Empty, (attributes, attribute) => attributes + attribute);
        }
    }
}
