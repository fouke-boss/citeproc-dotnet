using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Links this style to other styles and documents.
    /// </summary>
    public class LinkElement : Element
    {
        /// <summary>
        /// The uri of the linked document.
        /// </summary>
        [XmlAttribute("href")]
        public string Href
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates how the URI relates to the style.
        /// </summary>
        [XmlAttribute("rel")]
        public LinkRelation Relation
        {
            get;
            set;
        }
        /// <summary>
        /// Specifies the language of the element’s content; the value must be an xsd:language locale code.
        /// </summary>
        [XmlAttribute("xml:lang")]
        public string Culture
        {
            get;
            set;
        }
    }
}
