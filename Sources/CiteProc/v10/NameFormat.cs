using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Describes the rendering format of a name.
    /// </summary>
    public enum NameFormat
    {
        /// <summary>
        /// “long”: all the name-parts of personal names will be rendered.
        /// </summary>
        [XmlEnum("long")]
        Long,
        /// <summary>
        /// “short”: only the family name and the non-dropping-particle will be rendered.
        /// </summary>
        [XmlEnum("short")]
        Short,
        /// <summary>
        /// “count”: returns the total number of names that would otherwise be rendered by the use of the cs:names element (taking
        /// into account the effects of et-al abbreviation and editor/translator collapsing). Allows for advanced sorting.
        /// </summary>
        [XmlEnum("count")]
        Count
    }
}