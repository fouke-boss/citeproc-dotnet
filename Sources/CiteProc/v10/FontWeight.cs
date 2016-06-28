using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Sepecifies the font weight (normal, bold or light).
    /// </summary>
    public enum FontWeight
    {
        /// <summary>
        /// “normal” (default).
        /// </summary>
        [XmlEnum("normal")]
        Normal,
        /// <summary>
        /// “bold”.
        /// </summary>
        [XmlEnum("bold")]
        Bold,
        /// <summary>
        /// “light”.
        /// </summary>
        [XmlEnum("light")]
        Light
    }
}
