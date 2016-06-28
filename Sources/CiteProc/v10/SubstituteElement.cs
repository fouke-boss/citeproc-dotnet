using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    public class SubstituteElement : Element
    {
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
