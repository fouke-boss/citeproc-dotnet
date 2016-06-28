using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Specifies how ordinals are matched.
    /// </summary>
    public enum TermMatch
    {
        /// <summary>
        /// A term is used when the last digit in the term name matches the last digit of the rendered number.
        /// </summary>
        [XmlEnum("last-digit")]
        LastDigit,
        /// <summary>
        /// By setting the optional match attribute to “last-two-digits” (“last-digit” is the default), matches are 
        /// limited to numbers where the two last digits agree (“0”, “100”, “200”, etc.)
        /// </summary>
        [XmlEnum("last-two-digits")]
        LastTwoDigits,
        /// <summary>
        /// When match is set to “whole-number”, there is only a match if the number is the same as that of the term.
        /// </summary>
        [XmlEnum("whole-number")]
        WholeNumber
    }
}
