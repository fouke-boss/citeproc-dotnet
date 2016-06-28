using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Sets pluralization of the term in a label.
    /// </summary>
    public enum LabelPluralization
    {
        /// <summary>
        /// “contextual” - (default), the term plurality matches that of the variable content. Content is considered plural
        /// when it contains multiple numbers (e.g. “page 1”, “pages 1-3”, “volume 2”, “volumes 2 & 4”), or, in the case of
        /// the “number-of-pages” and “number-of-volumes” variables, when the number is higher than 1 (“1 volume” and “3 volumes”).
        /// </summary>
        [XmlEnum("contextual")]
        Contextual,
        /// <summary>
        /// “always” - always use the plural form, e.g. “pages 1” and “pages 1-3”
        /// </summary>
        [XmlEnum("always")]
        Always,
        /// <summary>
        /// “never” - always use the singular form, e.g. “page 1” and “page 1-3”.
        /// </summary>
        [XmlEnum("never")]
        Never
    }
}
