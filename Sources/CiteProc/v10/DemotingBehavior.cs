using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Describes the display and sorting behavior of the non-dropping-particle in inverted names (e.g. “de” in “Koning, W. de”). 
    /// </summary>
    public enum DemotingBehavior
    {
        /// <summary>
        /// “never”: the non-dropping-particle is treated as part of the family name, whereas the dropping-particle is appended
        /// (e.g. “de Koning, W.”, “La Fontaine, Jean de”). The non-dropping-particle is part of the primary sort key (sort order
        /// A, e.g. “de Koning, W.” appears under “D”).
        /// </summary>
        [XmlEnum("never")]
        Never,
        /// <summary>
        /// “sort-only”: same display behavior as “never”, but the non-dropping-particle is demoted to a secondary sort key 
        /// (sort order B, e.g. “de Koning, W.” appears under “K”).
        /// </summary>
        [XmlEnum("sort-only")]
        SortOnly,
        /// <summary>
        /// “display-and-sort” (default): the dropping and non-dropping-particle are appended (e.g. “Koning, W. de” and 
        /// “Fontaine, Jean de La”). For name sorting, all particles are part of the secondary sort key (sort order B, e.g.
        /// “Koning, W. de” appears under “K”).
        /// </summary>
        [XmlEnum("display-and-sort")]
        DisplayAndSort
    }
}