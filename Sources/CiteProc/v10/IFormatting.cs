using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.v10
{
    /// <summary>
    /// Represents an element with formatting attributes.
    /// </summary>
    public interface IFormatting
    {
        /// <summary>
        /// Gets or sets the font style (normal, italic or oblique).
        /// </summary>
        FontStyle FontStyle
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'font-style' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool FontStyleSpecified
        {
            get;
        }

        /// <summary>
        /// Gets or sets the font variant (normal or small-caps).
        /// </summary>
        bool FontVariantSpecified
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'font-variant' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        FontVariant FontVariant
        {
            get;
        }

        /// <summary>
        /// Gets or sets the font weight (normal, bold or light).
        /// </summary>
        FontWeight FontWeight
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'font-weight' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool FontWeightSpecified
        {
            get;
        }

        /// <summary>
        /// Gets or sets the text decoration (none or underline).
        /// </summary>
        bool TextDecorationSpecified
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'text-decoration' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        TextDecoration TextDecoration
        {
            get;
        }

        /// <summary>
        /// Gets or sets the vertical alignment (baseline, subscript or superscript).
        /// </summary>
        bool VerticalAlignSpecified
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'vertical-align' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        VerticalAlign VerticalAlign
        {
            get;
        }
    }
}