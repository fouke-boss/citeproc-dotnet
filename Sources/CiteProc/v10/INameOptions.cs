using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.v10
{
    /// <summary>
    /// Represents an element with name option attributes.
    /// </summary>
    public interface INameOptions : IEtAlOptions
    {
        /// <summary>
        /// Specifies the delimiter between the second to last and last name of the names in a name variable.
        /// </summary>
        And And
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'and' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool AndSpecified
        {
            get;
        }

        /// <summary>
        /// Specifies the text string used to separate names in a name variable. Default is ”, ” (e.g. “Doe, Smith”).
        /// </summary>
        string Delimiter
        {
            get;
        }

        /// <summary>
        /// Determines when the name delimiter or a space is used between a truncated name list and the “et-al”
        /// (or “and others”) term in case of et-al abbreviation.
        /// </summary>
        DelimiterBehavior DelimiterPrecedesEtAl
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'delimiter-preceded-et-al' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool DelimiterPrecedesEtAlSpecified
        {
            get;
        }

        /// <summary>
        /// Determines when the name delimiter is used to separate the second to last and the last name in name lists (if
        /// and is not set, the name delimiter is always used, regardless of the value of delimiter-precedes-last).
        /// </summary>
        DelimiterBehavior DelimiterPrecedesLast
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'delimiter-precedes-last' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool DelimiterPrecedesLastSpecified
        {
            get;
        }

        /// <summary>
        /// Specifies whether all the name-parts of personal names should be displayed (value “long”, the default), or only the
        /// family name and the non-dropping-particle (value “short”). A third value, “count”, returns the total number of names
        /// that would otherwise be rendered by the use of the cs:names element (taking into account the effects of et-al abbreviation
        /// and editor/translator collapsing), which allows for advanced sorting.
        /// </summary>
        NameFormat Format
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'form' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool FormatSpecified
        {
            get;
        }

        /// <summary>
        /// When set to “false” (the default is “true”), given names are no longer initialized when “initialize-with” is set. However,
        /// the value of “initialize-with” is still added after initials present in the full name (e.g. with initialize set to “false”,
        /// and initialize-with set to ”.”, “James T Kirk” becomes “James T. Kirk”).
        /// </summary>
        bool Initialize
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'initialize' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool InitializeSpecified
        {
            get;
        }

        /// <summary>
        /// When set, given names are converted to initials. The attribute value is added after each initial (”.” results in “J.J. Doe”). 
        /// </summary>
        string InitializeWith
        {
            get;
        }

        /// <summary>
        /// Specifies that names should be displayed with the given name following the family name (e.g. “John Doe” becomes “Doe, John”).
        /// </summary>
        NameSortOptions NameAsSortOrder
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'name-as-sort-order' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool NameAsSortOrderSpecified
        {
            get;
        }

        /// <summary>
        /// Sets the delimiter for name-parts that have switched positions as a result of name-as-sort-order. The default value
        /// is ”, ” (“Doe, John”). As is the case for name-as-sort-order, this attribute only affects names written
        /// in the latin or Cyrillic alphabets.
        /// </summary>
        string SortSeparator
        {
            get;
        }
    }
}