using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Specifies the delimiter between the second to last and last name of the names in a name variable.
    /// </summary>
    public enum And
    {
        /// <summary>
        /// Selects the comma, e.g. “Doe, Johnson, Smith”.
        /// </summary>
        [XmlIgnore]
        Delimiter,
        /// <summary>
        /// Selects the “and” term, e.g. “Doe, Johnson and Smith”.
        /// </summary>
        [XmlEnum("text")]
        Text,
        /// <summary>
        /// Selects the ampersand, e.g. “Doe, Johnson &amp; Smith”.
        /// </summary>
        [XmlEnum("symbol")]
        Symbol
    }
}
