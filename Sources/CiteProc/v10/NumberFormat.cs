using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Represents the available number formats.
    /// </summary>
    public enum NumberFormat
    {
        /// <summary>
        /// “numeric” - (default), e.g. “1”, “2”, “3”.
        /// </summary>
        [XmlEnum("numeric")]
        Numeric,
        /// <summary>
        /// “ordinal” - e.g. “1st”, “2nd”, “3rd”..
        /// </summary>
        [XmlEnum("ordinal")]
        Ordinal,
        /// <summary>
        /// long-ordinal” - e.g. “first”, “second”, “third”.
        /// </summary>
        [XmlEnum("long-ordinal")]
        LongOrdinal,
        /// <summary>
        /// “roman” - e.g. “i”, “ii”, “iii”.
        /// </summary>
        [XmlEnum("roman")]
        Roman
    }
}
