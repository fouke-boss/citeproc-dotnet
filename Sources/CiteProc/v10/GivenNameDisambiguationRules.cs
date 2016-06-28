using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Specifies a) whether the purpose of name expansion is limited to disambiguating cites, or has
    /// the additional goal of disambiguating names 
    /// </summary>
    public enum GivenNameDisambiguationRules
    {
        /// <summary>
        /// Name expansion has the dual purpose of disambiguating cites and names. All rendered ambiguous
        /// names, in both ambiguous and umambiguous cites, are subject to disambiguation. Each name is
        /// progressively transformed until it is disambiguated. Names that can not be disambiguated
        /// remain in their original form.
        /// </summary>
        [XmlEnum("all-names")]
        AllNames,
        /// <summary>
        /// As “all-names”, but name expansion is limited to showing initials (see step 1(a) above). No
        /// disambiguation attempt is made when initialize-with is not set or when initialize is set to “false”.
        /// </summary>
        [XmlEnum("all-names-with-initials")]
        AllNamesWithInitials,
        /// <summary>
        /// As “all-names”, but disambiguation is limited to the first name of each cite.
        /// </summary>
        [XmlEnum("primary-name")]
        PrimaryName,
        /// <summary>
        /// As “all-names-with-initials”, but disambiguation is limited to the first name of each cite.
        /// </summary>
        [XmlEnum("primary-name-with-initials")]
        PrimaryNameWithInitials,
        /// <summary>
        /// Default. As “all-names”, but the goal of name expansion is limited to disambiguating cites. Only ambiguous
        /// names in ambiguous cites are affected, and disambiguation stops after the first name that eliminates cite ambiguity.
        /// </summary>
        [XmlEnum("by-cite")]
        ByCite
    }
}