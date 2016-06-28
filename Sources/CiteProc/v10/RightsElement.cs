using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// The contents of cs:rights specifies the license under which the style file is released. The element 
    /// may carry a license attribute to specify the URI of the license.
    /// </summary>
    public class RightsElement : Element
    {
        /// <summary>
        /// Sppecifies the URI of the license.
        /// </summary>
        [XmlAttribute("license")]
        public string License
        {
            get;
            set;
        }
        /// <summary>
        /// The contents of cs:rights specifies the license under which the style file is released.
        /// </summary>
        [XmlText]
        public string Contents
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
