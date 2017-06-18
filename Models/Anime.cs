using System;
using System.Collections.Generic;

namespace AnimeExporter.Models {
    public class Anime {

        public readonly List<object> Attributes = new List<object>();

        public Anime(params object[] attributes) {
            foreach (object attribute in attributes) {
                Attributes.Add(attribute);
            }
        }
        
        public override string ToString() {
            return string.Join(Environment.NewLine, Attributes);
        }
    }
}