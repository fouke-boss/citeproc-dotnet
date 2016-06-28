using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Specifies the font style (normal, italic or oblique).
    /// </summary>
    public enum FontStyle
    {
        /// <summary>
        /// “normal” (default).
        /// </summary>
        [XmlEnum("normal")]
        Normal,
        /// <summary>
        /// “italic”.
        /// </summary>
        [XmlEnum("italic")]
        Italic,
        /// <summary>
        /// “oblique” (i.e. slanted).
        /// </summary>
        [XmlEnum("oblique")]
        Oblique
    }
}
