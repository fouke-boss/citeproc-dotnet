using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Specifies which parts of the date will be rendered. 
    /// </summary>
    public enum DatePrecision
    {
        /// <summary>
        /// “year-month-day” - (default), renders the year, month and day.
        /// </summary>
        [XmlEnum("year-month-day")]
        YearMonthDay,
        /// <summary>
        /// “year-month” - renders the year and month.
        /// </summary>
        [XmlEnum("year-month")]
        YearMonth,
        /// <summary>
        /// “year” - renders the year.
        /// </summary>
        [XmlEnum("year")]
        Year
    }
}
