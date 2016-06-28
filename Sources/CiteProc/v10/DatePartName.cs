using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Specifies the name of the date part - day, month, year.
    /// </summary>
    public enum DatePartName
    {
        /// <summary>
        /// Day part.
        /// </summary>
        [XmlEnum("day")]
        Day,
        /// <summary>
        /// Month part.
        /// </summary>
        [XmlEnum("month")]
        Month,
        /// <summary>
        /// Year part.
        /// </summary>
        [XmlEnum("year")]
        Year
    }
}
