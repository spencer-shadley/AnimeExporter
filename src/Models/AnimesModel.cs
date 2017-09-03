using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AnimeExporter.Models {
    
    public class AnimesModel : IEnumerable<AnimeModel> {

        private readonly Dictionary<string, AnimeModel> _animes = new Dictionary<string, AnimeModel>();
        
        public static HashSet<string> Genres = new HashSet<string>();
        
        /// <summary>
        /// Schema for buckets of data (genres, prodcuers, etc.)
        /// </summary>
        private static IList<object> CollectionSchema => new List<object> {
            "Genres"
        };

        /// <summary>
        /// Adds an anime to the existing collection assuming it does not already exist
        /// </summary>
        /// <param name="anime">The anime to add</param>
        public void Add(AnimeModel anime) {
            Debug.Assert(anime?.Url.Value != null);
            
            this._animes[anime.Url.Value] = anime;
        }

        /// <summary>
        /// Combines <param name="animesModel"></param> to the existing <see cref="AnimesModel"/>
        /// </summary>
        /// <remarks>In the case of an already existing anime the original ('this') wins</remarks>
        public void Add(AnimesModel animesModel) {
            Debug.Assert(animesModel != null);
            
            foreach (AnimeModel anime in animesModel) {
                this.Add(anime);
            }
        }
        
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
            return this.Select(anime => anime.ToRow()).Cast<IList<object>>().ToList();
        }
        
        public IEnumerator<AnimeModel> GetEnumerator()
        {
            return this._animes.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}