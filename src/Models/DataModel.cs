using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AnimeExporter.Models {
    public abstract class DataModel {
        
        /// <summary>
        /// Gets all <see cref="AttributeModel"/>
        /// </summary>
        /// <remarks>Uses reflection to gather every field of type <see cref="AttributeModel"/></remarks>
        public List<AttributeModel> Attributes {
            get {
                Type type = this.GetType();
                FieldInfo[] fields = type.GetFields();
                IEnumerable<object> values = fields.Select(field => field.GetValue(this));
                List<object> list = values.ToList();
                return list.Cast<AttributeModel>().ToList();
            }
        }

        /// <summary>
        /// Converts to a row of data which is expected to be used in a table for publishing later
        /// </summary>
        public List<object> ToRow() {
            return this.Attributes.Select(attribute => attribute.Value).Cast<object>().ToList();
        }

        public override string ToString() {
            return this.Attributes.Aggregate(string.Empty, (attributes, attribute) => attributes + attribute);
        }
        
        // TODO: where should this go?
        /// <summary>
        /// Gets the schema based on each attribute in the anime
        /// </summary>
        /// <remarks>
        /// Currently <see cref="AttributeModel"/> defaults value to the schema value
        /// so only a default anime needs to be constructed.
        /// </remarks>
        public static DetailsModel Schema() {
            return new DetailsModel();
        }
    }
}
