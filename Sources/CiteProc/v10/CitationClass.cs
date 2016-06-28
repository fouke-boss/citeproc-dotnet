using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Determines whether the style uses in-text citations (value “in-text”) or notes (“note”).
    /// </summary>
    public enum CitationClass
    {
        /// <summary>
        /// In-text citations.
        /// </summary>
        [XmlEnum("in-text")]
        InText,
        /// <summary>
        /// Note citations.
        /// </summary>
        [XmlEnum("note")]
        Note
    }
}
