using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Specifies the format of a date part.
    /// </summary>
    public enum DatePartFormat
    {
        /// <summary>
        /// “numeric” - (default), e.g. “1” - allowed for day and month parts.
        /// </summary>
        [XmlEnum("numeric")]
        Numeric,
        /// <summary>
        /// “numeric-leading-zeros” - e.g. “01” - allowed for day and month parts.
        /// </summary>
        [XmlEnum("numeric-leading-zeros")]
        NumericLeadingZeros,
        /// <summary>
        /// “ordinal” - e.g. “1st” - allowed for day parts.
        /// </summary>
        [XmlEnum("ordinal")]
        Ordinal,
        /// <summary>
        /// “long” - (default), e.g. “January” or “2005” - allowed for month and year parts.
        /// </summary>
        [XmlEnum("long")]
        Long,
        /// <summary>
        /// “short” - e.g. “Jan.” (month) or “05” (year) - allowed for month and year parts.
        /// </summary>
        [XmlEnum("short")]
        Short
    }
}
