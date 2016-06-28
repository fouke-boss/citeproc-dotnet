using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Specifies when and how names within bibliographies are substituted as a result of subsequent-author-substitute. 
    /// </summary>
    public enum SubsequentAuthorSubstituteRules
    {
        /// <summary>
        /// “complete-all” - (default), when all rendered names of the name variable match those in the preceding
        /// bibliographic entry, the value of subsequent-author-substitute replaces the entire name list (including
        /// punctuation and terms like “et al” and “and”), except for the affixes set on the cs:names element.
        /// </summary>
        [XmlEnum("complete-all")]
        CompleteAll,
        /// <summary>
        /// “complete-each” - requires a complete match like “complete-all”, but now the value of
        /// subsequent-author-substitute substitutes for each rendered name.
        /// </summary>
        [XmlEnum("complete-each")]
        CompleteEach,
        /// <summary>
        /// “partial-each” - when one or more rendered names in the name variable match those in the preceding bibliographic
        /// entry, the value of subsequent-author-substitute substitutes for each matching name. Matching starts with the
        /// first name, and continues up to the first mismatch.
        /// </summary>
        [XmlEnum("complete-all")]
        PartialEach,
        /// <summary>
        /// “partial-first” - as “partial-each”, but substitution is limited to the first name of the name variable.
        /// </summary>
        [XmlEnum("complete-all")]
        PartialFirst,
    }
}