using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Specifies the behavior of delimiters for the delimiter-precedes-et-al and delimiter-precedes-last attributes on cs:name.
    /// </summary>
    public enum DelimiterBehavior
    {
        /// <summary>
        /// “contextual” - (default), name delimiter is only used for name lists truncated to two or more (delimiter-precedes-et-al)
        /// or three or more (delimiter-precedes-last) names.
        /// </summary>
        [XmlEnum("contextual")]
        Contextual,
        /// <summary>
        /// “after-inverted-name” - name delimiter is only used if the preceding name is inverted as a result of the name-as-sort-order attribute.
        /// </summary>
        [XmlEnum("after-inverted-name")]
        AfterInvertedName,
        /// <summary>
        /// “always” - name delimiter is always used.
        /// </summary>
        [XmlEnum("always")]
        Always,
        /// <summary>
        /// “never” - name delimiter is never used.
        /// </summary>
        [XmlEnum("never")]
        Never
    }
}
