using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeExporter.Models {
    public class Anime {
        public List<object> Data;
        
        // title
        // url
        // rating
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
            return Data.Cast<string>().Aggregate(
                string.Empty, (current, data) =>
                    current + (data + Environment.NewLine));
        }
    }
}