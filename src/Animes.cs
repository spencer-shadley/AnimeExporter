using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AnimeExporter {
    
    public class Animes : IEnumerable<Anime> {

        private readonly Dictionary<string, Anime> _animes = new Dictionary<string, Anime>();
//        private readonly List<Anime> _animes = new List<Anime>();
        
        public static HashSet<string> Genres = new HashSet<string>();
        
        /// <summary>
        /// Schema for buckets of data (genres, prodcuers, etc.)
        /// </summary>
        private static IList<object> CollectionSchema => new List<object> {
            "Genres"
        };

        public void Add(Anime anime) {
//            _animes.Add(anime);
        }

        public void Add(Animes animes) {
            foreach (Anime anime in animes) {
                this.Add(anime);
            }
        }

        // TODO: public static implicit operator Attribute(string value)
        
        /// <summary>
        /// A table of metadata about each bucket (genres, producers, etc.) of data
        /// </summary>
        public List<IList<object>> ToCollectionsTable() {
            var ret = new List<IList<object>> { CollectionSchema };
            ret.AddRange(
                Genres.Select(genre => new List<object> { genre }));
            return ret;
        }

        /// <summary>
        /// A table of data representing all <see cref="_animes"/>
        /// </summary>
        public List<IList<object>> ToDataTable() {
            return null;
//            return _animes.Select(anime => anime.Attributes).Cast<IList<object>>().ToList();
        }

        public IEnumerator<Anime> GetEnumerator() {
            return null;
//            return _animes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}