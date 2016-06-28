using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Base class for elements with affixes and formatting attributes.
    /// </summary>
    public abstract class FormattingElement : RenderingElement, IFormatting
    {
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
    }
}
