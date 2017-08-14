using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AnimeExporter {
    
    public class Anime {

        public Attribute Title                = "Title";
        public Attribute EnglishTitle         = "English Title";
        public Attribute JapaneseTitle        = "Japanese Title";
        public Attribute Synonyms             = "Synonyms";
        public Attribute Url                  = "URL";
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
        public Attribute Sequel               = "Sequel";
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

    /// <summary>
    /// Represents a piece of info about an anime using a Name/Value pair
    /// </summary>
    public class Attribute {
        public string Name { get; }
        public string Value { get; set; }
        
        /// <summary>
        /// If the value hasn't updated from it's default value it is assumed to be a failure
        /// </summary>
        /// <remarks>This will not work if <see cref="Value"/> is expected to be <see cref="Name"/></remarks>
        public bool IsFailure => this.Name.Equals(this.Value);

        public Attribute(string name) {
            this.Name = name;
            this.Value = name;
        }

        public static implicit operator Attribute(string value) {
            return new Attribute(value);
        }

        public override string ToString() {
            return this.NameToString() + this.ValueToString() + Environment.NewLine;
        }

        public string NameToString() {
            return $"{this.Name}: ";
        }

        public string ValueToString() {
            if (this.Value == null) {
                return "none";
            }
            
            string cleanedValue = this.Value.Trim().Replace(Environment.NewLine, string.Empty); 
            string truncatedValue = cleanedValue.Length < 100 ?
                cleanedValue : cleanedValue.Substring(0, Math.Min(100, this.Value.Length)) + "...[truncated text]";
            return truncatedValue;
        }
    }
}
