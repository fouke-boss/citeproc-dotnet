using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Represents the format of a term.
    /// </summary>
    public enum TermFormat
    {
        /// <summary>
        /// “long” - (default), e.g. “editor” and “editors” for the “editor” term.
        /// </summary>
        [XmlEnum("long")]
        Long,
        /// <summary>
        /// “short” - e.g. “ed.” and “eds.” for the term “editor”.
        /// </summary>
        [XmlEnum("short")]
        Short,
        /// <summary>
        /// “verb” - e.g. “edited by” for the term “editor”.
        /// </summary>
        [XmlEnum("verb")]
        Verb,
        /// <summary>
        /// “verb-short” - e.g. “ed.” for the term “editor”.
        /// </summary>
        [XmlEnum("verb-short")]
        VerbShort,
        /// <summary>
        /// “symbol” - e.g. “§” and “§§” for the term “section”.
        /// </summary>
        [XmlEnum("symbol")]
        Symbol
    }
}
