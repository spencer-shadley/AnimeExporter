using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AnimeExporter.Models {
    public class Anime {

        public readonly List<object> Attributes = new List<object>();

        public Anime(params object[] attributes) {
            foreach (object attribute in attributes) {
                Attributes.Add(attribute);
            }
        }
        
        // categories
        // rank
        // popularity
        // members
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
        
        public override string ToString() {
            return string.Join(Environment.NewLine, Attributes);
        }
    }
}