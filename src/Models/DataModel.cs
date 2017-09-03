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

        public override string ToString() {
            return new AnimeModel(string.Empty, this).ToString();
        }
    }
}
