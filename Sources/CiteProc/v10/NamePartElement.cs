using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CiteProc.Compilation;

namespace CiteProc.v10
{
    public class NamePartElement : FormattingElement
    {
        [XmlAttribute("name")]
        public NamePartName Name
        {
            get;
            set;
        }

        /// <summary>
        /// When specified, converts the rendered text to the given case.
        /// </summary>
        [XmlAttribute("text-case")]
        public TextCase TextCase
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'text-case' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool TextCaseSpecified
        {
            get;
            set;
        }

        internal void Compile(MethodInvoke method)
        {
            // name
            method.AddElement(this);
            method.AddCode(method.ParameterName);
            method.AddLiteral(this.TextCaseSpecified ? (object)this.TextCase : null);
            method.AddLiteral(this.Prefix);
            method.AddLiteral(this.Suffix);
            method.AddDefaultParameters();
        }
    }
}
