using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Data
{
    public struct TextVariable : ITextVariable, IEquatable<TextVariable>, IEquatable<ITextVariable>
    {
        private string _Value;

        public TextVariable(string value)
        {
            this._Value = value;
        }

        public string Value
        {
            get
            {
                return this._Value;
            }
        }

        public static implicit operator TextVariable(string value)
        {
            return new TextVariable(value);
        }
        public static implicit operator string(TextVariable value)
        {
            return value._Value;
        }

        public static bool operator ==(TextVariable a, TextVariable b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }
        public static bool operator !=(TextVariable a, TextVariable b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            return (obj is TextVariable && this.Equals((TextVariable)obj));
        }
        public bool Equals(TextVariable other)
        {
            return (this._Value == other._Value);
        }
        public bool Equals(ITextVariable other)
        {
            return (this._Value == other.Value);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool IsNullOrEmpty
        {
            get
            {
                return string.IsNullOrEmpty(this._Value);
            }
        }

        public override string ToString()
        {
            return this._Value;
        }
    }
}