using System;

namespace AnimeExporter.Models {
    /// <summary>
    /// Represents a piece of info about an anime using a Name/Value pair
    /// </summary>
    public class Attribute {
        public const string Empty = "none";
        
        public string Name { get; }
        public string Value { get; set; }
        
        /// <summary>
        /// If the value hasn't updated from it's default value it is assumed to be a failure
        /// </summary>
        /// <remarks>This will not work if <see cref="Value"/> is expected to be <see cref="Name"/></remarks>
        public bool IsFailure => this.Name.Equals(this.Value);

        public Attribute(string name) {
            this.Name = name;
            this.Value = name;
        }

        public static implicit operator Attribute(string value) {
            return new Attribute(value);
        }
        
        public override string ToString() {
            return this.NameToString() + this.ValueToString() + Environment.NewLine;
        }

        public string NameToString() {
            return $"{this.Name}: ";
        }

        public string ValueToString() {
            if (this.Value == null) {
                return Empty;
            }
            
            string cleanedValue = this.Value.Trim().Replace(Environment.NewLine, string.Empty); 
            string truncatedValue = cleanedValue.Length < 100 ?
                cleanedValue : cleanedValue.Substring(0, Math.Min(100, this.Value.Length)) + "...[truncated text]";
            return truncatedValue;
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(Attribute)) {
                return false;
            }
            var that = (Attribute) obj;
            return this.Name.Equals(that.Name) && this.Value.Equals(that.Value);
        }

        public override int GetHashCode() {
            unchecked {
                return ((this.Name != null ? this.Name.GetHashCode() : 0) * 397) ^
                       (this.Value != null ? this.Value.GetHashCode() : 0);
            }
        }
    }
}