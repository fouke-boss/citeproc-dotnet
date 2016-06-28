using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Per-locale, global options which affect both citations and the bibliography.
    /// </summary>
    public class StyleOptionsElement
    {
        /// <summary>
        /// By setting limit-day-ordinals-to-day-1 to “true” (“false” is the default), the “ordinal” form is 
        /// limited to the first day of each month (other days will use the “numeric” form). This is desirable 
        /// for some languages, such as French: “1er janvier”, but “2 janvier”, “3 janvier”, etc.
        /// </summary>
        [XmlAttribute("limit-day-ordinals-to-day-1")]
        public bool LimitDayOrdinalsToDay1
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'limit-day-ordinals-to-day-1' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool LimitDayOrdinalsToDay1Specified
        {
            get;
            set;
        }

        /// <summary>
        /// For cs:text elements rendered with the quotes attribute set to “true” (see Formatting), and for which 
        /// the output is followed by a comma or period, punctuation-in-quote specifies whether this punctuation is
        /// placed outside (value “false”, default) or inside (value “true”) the closing quotation mark.
        /// </summary>
        [XmlAttribute("punctuation-in-quote")]
        public bool PunctuationInQuote
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'punctuation-in-quote' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool PunctuationInQuoteSpecified
        {
            get;
            set;
        }
    }
}
