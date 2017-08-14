using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Google.Apis.Util;

namespace AnimeExporter {
    
    public class Animes : IEnumerable<Anime> {

        private readonly Dictionary<string, Anime> _animes = new Dictionary<string, Anime>();
        
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
        public void Add(Anime anime) {
            Debug.Assert(anime?.Url.Value != null);
            
            this._animes[anime.Url.Value] = anime;
        }

        /// <summary>
        /// Combines <see cref="animes"/> to the existing <see cref="Animes"/>
        /// </summary>
        /// <param name="animes">The animes to add</param>
        /// <remarks>In the case of an already existing anime the original ('this') wins</remarks>
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
            return this.Select(anime => anime.ToRow()).Cast<IList<object>>().ToList();
        }
        
        public IEnumerator<Anime> GetEnumerator()
        {
            return this._animes.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}