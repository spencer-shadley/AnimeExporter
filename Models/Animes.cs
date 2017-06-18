using System.Collections.Generic;
using System.Linq;

namespace AnimeExporter.Models {
    public class Animes {

        private readonly List<Anime> _animes = new List<Anime>();

        public Animes() {
            // Add the schema as its own "anime" so that we get nice titling in our Google Sheet
            _animes.Add(new Anime("Title", "URL", "Score", "Number of ratings", "Rank", "Popularity",
                "Number of Members", "Number of Favorites", "Type of Media", "Number of Episodes",
                "Status", "Dates Aired"));
        
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
        }

        public void Add(Anime anime) {
            _animes.Add(anime);
        }

        public List<IList<object>> ToTable() {
            return _animes.Select(anime => anime.Attributes).Cast<IList<object>>().ToList();
        }
        
    }
}