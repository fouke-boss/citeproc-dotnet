using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Represents a text in a given language within the style info.
    /// </summary>
    public class InfoTextElement : Element
    {
        /// <summary>
        /// Specifies the language of the element’s content; the value must be an xsd:language locale code.
        /// </summary>
        [XmlAttribute("xml:lang")]
        public string XmlLang
        {
            get;
            set;
        }
        /// <summary>
        /// The text of the translation.
        /// </summary>
        [XmlText]
        public string Text
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.Text;
        }
    }
}