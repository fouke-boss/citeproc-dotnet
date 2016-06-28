using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Specifies the text decoration (none or underline).
    /// </summary>
    public enum TextDecoration
    {
        /// <summary>
        /// “none” (default).
        /// </summary>
        [XmlEnum("none")]
        None,
        /// <summary>
        /// “underline”.
        /// </summary>
        [XmlEnum("underline")]
        Underline
    }
}
