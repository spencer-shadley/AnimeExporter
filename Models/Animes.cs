﻿using System.Collections.Generic;
using System.Linq;

namespace AnimeExporter.Models {
    public class Animes {

        private readonly List<Anime> _animes = new List<Anime>();

        public Animes() {
            _animes.Add(new Anime("Title", "URL", "Score", "Number of ratings"));
        }

        public void Add(Anime anime) {
            _animes.Add(anime);
        }

        public List<IList<object>> ToTable() {
            return _animes.Select(anime => anime.Attributes).Cast<IList<object>>().ToList();
        }
        
    }
}