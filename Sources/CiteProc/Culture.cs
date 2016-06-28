using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc
{
    /// <summary>
    /// A culture represents the combination of a language and a dialect.
    /// </summary>
    public struct Culture : IEquatable<Culture>
    {
        private string _Language;
        private string _Dialect;

        /// <summary>
        /// The invariant culture.
        /// </summary>
        public static readonly Culture Invariant = new Culture();
        /// <summary>
        /// The Klingon culture.
        /// </summary>
        public static readonly Culture Klingon = new Culture("tlh");

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="xmlLang"></param>
        public Culture(string xmlLang)
        {
            // spilt
            var parts = (xmlLang ?? "").Split(new char[]{'-'}, StringSplitOptions.RemoveEmptyEntries);
            this._Language = (parts.Length == 0 || string.IsNullOrEmpty(parts[0]) || parts[0].Length != 2 ? null : parts[0].ToLower());
            this._Dialect = (parts.Length > 1 && !string.IsNullOrEmpty(parts[1]) && parts[1].Length == 2 && !string.IsNullOrEmpty(this._Language) ? parts[1].ToUpper() : null);

            // klingon?
            if (!string.IsNullOrEmpty(xmlLang) && this.IsInvariant)
            {
                this._Language = "tlh";
            }
        }

        /// <summary>
        /// The language of the culture, in lowercase.
        /// </summary>
        public string Language
        {
            get
            {
                return this._Language;
            }
        }
        /// <summary>
        /// The dialect of the culture, in uppercase.
        /// </summary>
        public string Dialect
        {
            get
            {
                return this._Dialect;
            }
        }

        /// <summary>
        /// Indicates whether both language and dialect are set. 
        /// </summary>
        public bool IsDialect
        {
            get
            {
                return !string.IsNullOrEmpty(this._Language) && !string.IsNullOrEmpty(this._Dialect);
            }
        }
        /// <summary>
        /// Indicates whether language is set but dialect is not set. 
        /// </summary>
        public bool IsLanguage
        {
            get
            {
                return !string.IsNullOrEmpty(this._Language) && string.IsNullOrEmpty(this._Dialect);
            }
        }
        /// <summary>
        /// Indicates whether both language and dialect are not set. 
        /// </summary>
        public bool IsInvariant
        {
            get
            {
                return string.IsNullOrEmpty(this._Language) && string.IsNullOrEmpty(this._Dialect);
            }
        }
        /// <summary>
        /// Indicates whether the culture is the Klingon culture.
        /// </summary>
        public bool IsKlingon
        {
            get
            {
                return (this._Language == "tlh");
            }
        }

        /// <summary>
        /// Implicit conversion operator to string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Culture(string value)
        {
            return new Culture(value);
        }
        /// <summary>
        /// Implicit conversion operator from string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator string(Culture value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Equal operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Culture a, Culture b)
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
        /// <summary>
        /// Inequal operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Culture a, Culture b)
        {
            return !(a == b);
        }
        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return (obj is Culture && this.Equals((Culture)obj));
        }
        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Culture other)
        {
            return (other._Language == this._Language && other._Dialect == this._Dialect);
        }
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns the full name of this culture.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.IsInvariant)
            {
                return "";
            }
            else if (this.IsLanguage)
            {
                return this._Language;
            }
            else
            {
                return string.Format("{0}-{1}", this._Language, this._Dialect);
            }
        }
    }
}
