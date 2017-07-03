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
        
        /*private static readonly object[] SchemaAttributes = {
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
        };*/

        /*public Anime(params object[] attributes) {
            foreach (object attribute in attributes) {
                Attributes.Add(attribute);
            }
        }*/

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
        
        public static Anime Schema() {
            return new Anime(/*SchemaAttributes*/);
        }

        public static Anime Fail() {
            return new Anime(/*"Failed to export this anime"*/);
        }

        public override string ToString() {
            return AllAttributes.Aggregate(string.Empty, (current, attribute) => current + attribute);
        }
        
//        public override string ToString() {
//            return string.Join(Environment.NewLine,
//                Attributes.Where(attribute => attribute.ToString().Length < 100)); // avoid things like synposis
//        }
    }

    public struct Attribute {
        public string Name { get; }
        public string Value { get; set; }

        public Attribute(string name) {
            this.Name = name;
            this.Value = null;
        }

        public override string ToString() {
            return $"{this.Name}: " +
                   (Value.Length < 100 ? Value : $"Value too long to display ({Value.Length} characters)") +
                Environment.NewLine;
        }
    }
}
