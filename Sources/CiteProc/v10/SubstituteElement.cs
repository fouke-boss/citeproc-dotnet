using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// The optional cs:substitute element adds substitution in case the name variables specified in the parent cs:names
    /// element are empty. The substitutions are specified as child elements of cs:substitute, and must consist of one
    /// or more rendering elements (with the exception of cs:layout).
    /// </summary>
    public class SubstituteElement : Element
    {
        /// <summary>
        /// Gets or sets the rendering elements to substite, if the variable in the parent names element is empty.
        /// </summary>
        [XmlElement("choose", Type = typeof(ChooseElement))]
        [XmlElement("date", Type = typeof(DateElement))]
        [XmlElement("group", Type = typeof(GroupElement))]
        [XmlElement("number", Type = typeof(NumberElement))]
        [XmlElement("names", Type = typeof(NamesElement))]
        [XmlElement("label", Type = typeof(LabelElement))]
        [XmlElement("text", Type = typeof(TextElement))]
        public RenderingElement[] Children
        {
            get;
            set;
        }
    }
}
