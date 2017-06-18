using System.Collections.Generic;
using System.Linq;

namespace AnimeExporter.Models {
    public class Animes {

        private readonly List<Anime> _animes = new List<Anime>();
        
        public Animes() {
            // Add the schema as its own "anime" so that we get nice titling in our Google Sheet
            _animes.Add(Anime.Schema());
        }

        public void Add(Anime anime) {
            _animes.Add(anime);
        }

        public List<IList<object>> ToTable() {
            return _animes.Select(anime => anime.Attributes).Cast<IList<object>>().ToList();
        }
        
    }
}