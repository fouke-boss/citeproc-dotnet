using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Specifies the vertical alignment (baseline, subscript or superscript).
    /// </summary>
    public enum VerticalAlign
    {
        /// <summary>
        /// “baseline” (default).
        /// </summary>
        [XmlEnum("baseline")]
        Baseline,
        /// <summary>
        /// “sup” (superscript).
        /// </summary>
        [XmlEnum("sup")]
        Superscript,
        /// <summary>
        /// “sup” (subscript).
        /// </summary>
        [XmlEnum("sub")]
        Subscript
    }
}
