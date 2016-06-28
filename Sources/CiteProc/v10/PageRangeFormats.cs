using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Represents the available page range abbreviation rules.
    /// </summary>
    public enum PageRangeFormats
    {
        /// <summary>
        /// Page ranges are abbreviated according to the Chicago Manual of Style-rules.
        /// </summary>
        [XmlEnum("chicago")]
        Chicago,
        /// <summary>
        /// Abbreviated page ranges are expanded to their non-abbreviated form: 42–45, 321–328, 2787–2816.
        /// </summary>
        [XmlEnum("expanded")]
        Expanded,
        /// <summary>
        /// All digits repeated in the second number are left out: 42–5, 321–8, 2787–816.
        /// </summary>
        [XmlEnum("minimal")]
        Minimal,
        /// <summary>
        /// As “minimal”, but at least two digits are kept in the second number when it has two or more digits long.
        /// </summary>
        [XmlEnum("minimal-two")]
        MinimalTwo
    }
}
