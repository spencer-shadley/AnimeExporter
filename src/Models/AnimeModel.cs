using System.Collections.Generic;
using System.Linq;

namespace AnimeExporter.Models {
    public class AnimeModel {
        
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public AttributeModel Url { get; }
        public List<AttributeModel> Attributes { get; }
        
        public AnimeModel(string url, params DataModel[] dataModels) {
            this.Url = url;
            this.Attributes = new List<AttributeModel>();
            
            foreach (DataModel model in dataModels) {
                if (model == null) {
                    Log.Error($"[AnimeModel()] dataModel was null");
                    continue;
                }
                this.Attributes.AddRange(model.Attributes);
            }
        }

        /// <summary>
        /// Converts to a row of data which is expected to be used in a table for publishing later
        /// </summary>
        public List<object> ToRow() {
            return this.Attributes.Select(attribute => attribute.Value).Cast<object>().ToList();
        }
        
        /// <summary>
        /// Gets the schema based on each attribute in the anime
        /// </summary>
        /// <remarks>
        /// Currently <see cref="AttributeModel"/> defaults value to the schema value
        /// so only a default anime needs to be constructed.
        /// </remarks>
        public static AnimeModel Schema() {
            return new AnimeModel("Url", new DetailsModel(), new StatsModel(), new VideoModel(), new CharactersModel());
        }

        public override string ToString() {
            return this.Attributes.Aggregate(string.Empty, (attributes, attribute) => attributes + attribute);
        }
    }
}
