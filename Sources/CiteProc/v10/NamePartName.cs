using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Specifies the part of a name.
    /// </summary>
    public enum NamePartName
    {
        /// <summary>
        /// The family name part, including any non-dropping particles.
        /// </summary>
        [XmlEnum("family")]
        Family,
        /// <summary>
        /// The given name part, including any dropping particles.
        /// </summary>
        [XmlEnum("given")]
        Given
    }
}
