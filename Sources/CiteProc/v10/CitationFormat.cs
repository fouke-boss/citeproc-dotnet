using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Describes how in-text citations are rendered.
    /// </summary>
    public enum CitationFormat
    {
        /// <summary>
        /// “author-date” - e.g. ”... (Doe, 1999)”
        /// </summary>
        [XmlEnum("author-date")]
        AuthorDate,
        /// <summary>
        /// “author” - e.g. ”... (Doe)”
        /// </summary>
        [XmlEnum("author")]
        Author,
        /// <summary>
        /// “numeric” - e.g. ”... [1]”
        /// </summary>
        [XmlEnum("numeric")]
        Numeric,
        /// <summary>
        /// “label” - e.g. ”... [doe99]”
        /// </summary>
        [XmlEnum("label")]
        Label,
        /// <summary>
        /// “note” - the citation appears as a footnote or endnote
        /// </summary>
        [XmlEnum("note")]
        Note
    }
}
