using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AnimeExporter {
    
    public class Anime {

        public Attribute Title        = new Attribute("Title");
        public Attribute Url          = new Attribute("URL");
        public Attribute Score        = new Attribute("Score");
        public Attribute NumRatings   = new Attribute("Number of ratings");
        public Attribute Rank         = new Attribute("Rank");
        public Attribute Popularity   = new Attribute("Popularity");
        public Attribute NumMembers   = new Attribute("Number of Members");
        public Attribute NumFavorites = new Attribute("Number of Favorites");
        public Attribute MediaType    = new Attribute("Type of Media");
        public Attribute NumEpisodes  = new Attribute("Number of Episodes");
        public Attribute Status       = new Attribute("Status");
        public Attribute DateStarted  = new Attribute("Date Started Airing");
        public Attribute DateFinished = new Attribute("Date Finished Airing");
        public Attribute Producers    = new Attribute("Producers");
        public Attribute Licensors    = new Attribute("Licensors");
        public Attribute Studios      = new Attribute("Studios");
        public Attribute Genres       = new Attribute("Genres");
        public Attribute Duration     = new Attribute("Duration");
        public Attribute Rating       = new Attribute("Rating");
        public Attribute Source       = new Attribute("Source");
        public Attribute Synopsis     = new Attribute("Synopsis");
        public Attribute Background   = new Attribute("Background");
        public Attribute Image        = new Attribute("Image");
        
        // TODO: the below Attributes
        // adaptation
        // sequel
        // related
        // English title
        // synonyms
        // Japanese title

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
            return AllAttributes.Select(attribute => attribute.Value).Cast<object>().ToList();
        }

        public override string ToString() {
            return AllAttributes.Aggregate(string.Empty, (current, attribute) => current + attribute);
        }
    }

    /// <summary>
    /// Represents a piece of info about an anime using a Name/Value pair
    /// </summary>
    public struct Attribute {
        public string Name { get; }
        public string Value { get; set; }

        /// <summary>
        /// If the value hasn't updated from it's default value it is assumed to be a failure
        /// </summary>
        /// <remarks>This will not work if <see cref="Value"/> is expected to be <see cref="Name"/></remarks>
        public bool IsFailure => Name.Equals(Value);

        public Attribute(string name) {
            Name = name;
            Value = name;
        }

        public override string ToString() {
            string cleanedValue = Value.Trim().Replace(Environment.NewLine, string.Empty); 
            string truncatedValue = cleanedValue.Length < 100 ?
                cleanedValue : cleanedValue.Substring(0, Math.Min(100, Value.Length)) + "...[truncated text]";
            return $"{Name}: {truncatedValue}" +
                Environment.NewLine;
        }
    }
}
