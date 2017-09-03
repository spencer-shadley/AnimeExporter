using System.Collections.Generic;

namespace AnimeExporter.Models {
    public class AnimeModel {
        public List<AttributeModel> Attributes { get; }
        
        public AnimeModel(params DataModel[] dataModels) {
            foreach (DataModel model in dataModels) {
                this.Attributes.AddRange(model.Attributes);
            }
        }
    }
}
