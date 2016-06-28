using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// The display attribute (similar the “display” property in CSS) may be used to structure individual bibliographic entries
    /// into one or more text blocks. If used, all rendering elements should be under the control of a display attribute. 
    /// </summary>
    public enum Display
    {
        /// <summary>
        /// Block stretching from margin to margin.
        /// </summary>
        [XmlEnum("block")]
        Block,
        /// <summary>
        /// Block starting at the left margin. If followed by a “right-inline” block, the “left-margin” blocks of all bibliographic
        /// entries are set to a fixed width to accommodate the longest content string found among these “left-margin” blocks. In
        /// the absence of a “right-inline” block the “left-margin” block extends to the right margin.
        /// </summary>
        [XmlEnum("left-margin")]
        LeftMargin,
        /// <summary>
        /// block starting to the right of a preceding “left-margin” block (behaves as “block” in the absence of
        /// such a “left-margin” block). Extends to the right margin.
        /// </summary>
        [XmlEnum("right-inline")]
        RightInline,
        /// <summary>
        /// Block indented to the right by a standard amount. Extends to the right margin.
        /// </summary>
        [XmlEnum("indent")]
        Indent
    }
}
