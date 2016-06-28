using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.v10
{
    /// <summary>
    /// Represents an element with et-al attributes.
    /// </summary>
    public interface IEtAlOptions
    {
        /// <summary>
        /// If the number of names in a name variable matches or exceeds the number set on et-al-min, the rendered name list is truncated.
        /// </summary>
        int EtAlMin
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'et-al-min' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool EtAlMinSpecified
        {
            get;
        }

        /// <summary>
        /// If the number of names in a name variable is truncated, it is truncated after reaching the number of names set on et-al-use-first.
        /// </summary>
        int EtAlUseFirst
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'et-al-first' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool EtAlUseFirstSpecified
        {
            get;
        }

        /// <summary>
        /// If the number of names in a name variable is truncated, it is truncated after reaching the number of names set on et-al-use-first.
        /// </summary>
        int EtAlSubsequentMin
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'et-al-subsequent-min' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool EtAlSubsequentMinSpecified
        {
            get;
        }

        /// <summary>
        /// If the number of names in a name variable is truncated, it is truncated after reaching the number of names set on et-al-use-first.
        /// </summary>
        int EtAlSubsequentUseFirst
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'et-al-subsequent-use-first' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool EtAlSubsequentUseFirstSpecified
        {
            get;
        }

        /// <summary>
        /// When set to “true” (the default is “false”), name lists truncated by et-al abbreviation are followed by the name
        /// delimiter, the ellipsis character, and the last name of the original name list. This is only possible when the
        /// original name list has at least two more names than the truncated name list.
        /// </summary>
        bool EtAlUseLast
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'et-al-use-last' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool EtAlUseLastSpecified
        {
            get;
        }
    }
}
