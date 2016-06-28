using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Describes a sort order.
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// Ascending.
        /// </summary>
        [XmlEnum("ascending")]
        Ascending,
        /// <summary>
        /// Descending.
        /// </summary>
        [XmlEnum("descending")]
        Descending
    }
}
