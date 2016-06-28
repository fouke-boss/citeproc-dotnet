using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Specifies the order of the name parts (e.g. “John Doe” vs. “Doe, John”). 
    /// </summary>
    public enum NameSortOptions
    {
        /// <summary>
        /// None of the names will be inverted.
        /// </summary>
        [XmlIgnore]
        None,
        /// <summary>
        /// “first”: only the first name in a list will be inverted.
        /// </summary>
        [XmlEnum("first")]
        First,
        /// <summary>
        /// “all”: a;; names in a list will be inverted.
        /// </summary>
        [XmlEnum("all")]
        All
    }
}