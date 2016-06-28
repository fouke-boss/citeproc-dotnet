using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// May be used once to describe how in-text citations are rendered, using the citation-format attribute. May
    /// be used multiple times with the field attribute, set to one of the discipline categories, to indicate the field(s)
    /// for which the style is relevant.
    /// </summary>
    public class CategoryElement : Element
    {
        /// <summary>
        /// Describes how in-text citations are rendered.
        /// </summary>
        [XmlAttribute("citation-format")]
        public CitationFormat CitationFormat
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the citation format is specified. Required bij the System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool CitationFormatSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates the field for which the style is relevant.
        /// </summary>
        [XmlAttribute("field")]
        public string Field
        {
            get;
            set;
        }
    }
}
