using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Control the testing logic of a cs:choose element.
    /// </summary>
    public enum Match
    {
        /// <summary>
        /// Default. Elements only tests “true” when all conditions test “true” for all given test values.
        /// </summary>
        [XmlEnum("all")]
        All,
        /// <summary>
        /// Element tests “true” when any condition tests “true” for any given test value.
        /// </summary>
        [XmlEnum("any")]
        Any,
        /// <summary>
        /// Element only tests “true” when none of the conditions test “true” for any given test value.
        /// </summary>
        [XmlEnum("none")]
        None
    }
}
