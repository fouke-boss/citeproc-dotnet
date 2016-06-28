using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Specifies cite grouping and collapsing.
    /// </summary>
    public enum Collapse
    {
        /// <summary>
        /// “citation-number” - collapses ranges of cite numbers (rendered through the “citation-number” variable) in
        /// citations for “numeric” styles (e.g. from “[1, 2, 3, 5]” to “[1–3, 5]”). Only increasing ranges collapse,
        /// e.g. “[3, 2, 1]” will not collapse.
        /// </summary>
        [XmlEnum("citation-number")]
        CitationNumber,
        /// <summary>
        /// “year” - collapses cite groups by suppressing the output of the cs:names element for subsequent cites in
        /// the group, e.g. “(Doe 2000, Doe 2001)” becomes “(Doe 2000, 2001)”.
        /// </summary>
        [XmlEnum("year")]
        Year,
        /// <summary>
        /// “year-suffix” - collapses as “year”, but also suppresses repeating years within the cite group, e.g. 
        /// “(Doe 2000a, b)” instead of “(Doe 2000a, 2000b)”.
        /// </summary>
        [XmlEnum("year-suffix")]
        YearSuffix,
        /// <summary>
        /// “year-suffix-ranged” - collapses as “year-suffix”, but also collapses ranges of year-suffixes, e.g. 
        /// “(Doe 2000a–c,e)” instead of “(Doe 2000a, b, c, e)”.
        /// </summary>
        [XmlEnum("year-suffix-ranged")]
        YearSuffixRanged
    }
}
