using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Specifies alignment of the second field.
    /// </summary>
    public enum SecondFieldAlign
    {
        /// <summary>
        /// With “flush”, the first field is flush with the margin.
        /// </summary>
        [XmlEnum("flush")]
        Flush,
        /// <summary>
        /// With “margin”, the first field is put in the margin, and subsequent lines are aligned with the margin.
        /// </summary>
        [XmlEnum("margin")]
        Margin
    }
}
