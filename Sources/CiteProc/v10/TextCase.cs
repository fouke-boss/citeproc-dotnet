using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Represents the available text cases.
    /// </summary>
    public enum TextCase
    {
        /// <summary>
        /// Renders text in lowercase.
        /// </summary>
        [XmlEnum("lowercase")]
        Lowercase,
        /// <summary>
        /// Renders text in uppercase.
        /// </summary>
        [XmlEnum("uppercase")]
        Uppercase,
        /// <summary>
        /// Capitalizes the first character of the first word, if the word is lowercase.
        /// </summary>
        [XmlEnum("capitalize-first")]
        CapitalizeFirst,
        /// <summary>
        /// Capitalizes the first character of every lowercase word.
        /// </summary>
        [XmlEnum("capitalize-all")]
        CapitalizeAll,
        /// <summary>
        /// Renders text in sentence case.
        /// </summary>
        [XmlEnum("sentence")]
        Sentence,
        /// <summary>
        /// Renders text in title case.
        /// </summary>
        [XmlEnum("title")]
        Title
    }
}
