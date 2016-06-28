using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Represents the ways an external document relates to a style.
    /// </summary>
    public enum LinkRelation
    {
        /// <summary>
        /// URI of the style itself.
        /// </summary>
        [XmlEnum("self")]
        Self,
        /// <summary>
        /// URI of the style from which the current style is derived.
        /// </summary>
        [XmlEnum("template")]
        Template,
        /// <summary>
        /// URI of style documentation.
        /// </summary>
        [XmlEnum("documentation")]
        Documentation,
        /// <summary>
        /// The independent parent file of the current dependent file.
        /// </summary>
        [XmlEnum("independent-parent")]
        IndepdendentParent
    }
}
