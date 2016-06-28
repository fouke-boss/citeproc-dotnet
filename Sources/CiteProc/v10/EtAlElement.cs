using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Et-al abbreviation, controlled via the et-al-... attributes, can be further customized with this optional cs:et-al element.
    /// </summary>
    public class EtAlElement : Element, IFormatting
    {
        /// <summary>
        /// The term attribute may be set to either “et-al” (the default) or to “and others” to use either term.
        /// </summary>
        [XmlAttribute("term")]
        public TermName Term
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'term' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool TermSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the font style (normal, italic or oblique).
        /// </summary>
        [XmlAttribute("font-style")]
        public FontStyle FontStyle
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'font-style' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool FontStyleSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the font variant (normal or small-caps).
        /// </summary>
        [XmlAttribute("font-variant")]
        public FontVariant FontVariant
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'font-variant' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool FontVariantSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the font weight (normal, bold or light).
        /// </summary>
        [XmlAttribute("font-weight")]
        public FontWeight FontWeight
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'font-weight' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool FontWeightSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text decoration (none or underline).
        /// </summary>
        [XmlAttribute("text-decoration")]
        public TextDecoration TextDecoration
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'text-decoration' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool TextDecorationSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the vertical alignment (baseline, subscript or superscript).
        /// </summary>
        [XmlAttribute("vertical-align")]
        public VerticalAlign VerticalAlign
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'vertical-align' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool VerticalAlignSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Compiles this EtAl element.
        /// </summary>
        /// <param name="m"></param>
        internal void Compile(Compilation.MethodInvoke m)
        {
            m.AddElement(this);
            m.AddCode(m.ParameterName);
            m.AddLiteral(this.TermSpecified ? this.Term : TermName.EtAl);
            m.AddDefaultParameters();
        }
    }
}