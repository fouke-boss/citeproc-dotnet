using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CiteProc.Compilation;

namespace CiteProc.v10
{
    /// <summary>
    /// Base class for Citation and Bibliography elements.
    /// </summary>
    public abstract class EntryElement : Element, INameOptions, INamesOptions
    {
        /// <summary>
        /// The optional cs:sort element specifies how references within the bibliography should be sorted.
        /// </summary>
        [XmlElement("sort")]
        public SortElement Sort
        {
            get;
            set;
        }
        /// <summary>
        /// The required cs:layout child element describes how each entry should be formatted.
        /// </summary>
        [XmlElement("layout")]
        public LayoutElement Layout
        {
            get;
            set;
        }

        /// <summary>
        /// Compiles the entry descriptor element.
        /// </summary>
        /// <param name="code"></param>
        internal abstract void Compile(Scope code);

        #region Inheritable name options
        /// <summary>
        /// Specifies the delimiter between the second to last and last name of the names in a name variable.
        /// </summary>
        [XmlAttribute("and")]
        public And And
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'and' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool AndSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the text string used to separate names in a name variable. Default is ”, ” (e.g. “Doe, Smith”).
        /// </summary>
        [XmlAttribute("name-delimiter")]
        public string NameDelimiter
        {
            get;
            set;
        }
        /// <summary>
        /// Specifies the text string used to separate names in a name variable. Default is ”, ” (e.g. “Doe, Smith”).
        /// </summary>
        string INameOptions.Delimiter
        {
            get
            {
                return this.NameDelimiter;
            }
        }


        /// <summary>
        /// Specifies the delimiter for names of the different name variables (e.g. the semicolon in “Doe, Smith (editors); Johnson (translator)”).
        /// </summary>
        [XmlAttribute("names-delimiter")]
        public string NamesDelimiter
        {
            get;
            set;
        }
        /// <summary>
        /// Specifies the delimiter for names of the different name variables (e.g. the semicolon in “Doe, Smith (editors); Johnson (translator)”).
        /// </summary>
        string INamesOptions.Delimiter
        {
            get
            {
                return this.NamesDelimiter;
            }
        }

        /// <summary>
        /// Determines when the name delimiter or a space is used between a truncated name list and the “et-al”
        /// (or “and others”) term in case of et-al abbreviation.
        /// </summary>
        [XmlAttribute("delimiter-precedes-et-al")]
        public DelimiterBehavior DelimiterPrecedesEtAl
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'delimiter-preceded-et-al' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool DelimiterPrecedesEtAlSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Determines when the name delimiter is used to separate the second to last and the last name in name lists (if
        /// and is not set, the name delimiter is always used, regardless of the value of delimiter-precedes-last).
        /// </summary>
        [XmlAttribute("delimiter-precedes-last")]
        public DelimiterBehavior DelimiterPrecedesLast
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'delimiter-precedes-last' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool DelimiterPrecedesLastSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// If the number of names in a name variable matches or exceeds the number set on et-al-min, the rendered name list is truncated.
        /// </summary>
        [XmlAttribute("et-al-min")]
        public int EtAlMin
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'et-al-min' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool EtAlMinSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// If the number of names in a name variable is truncated, it is truncated after reaching the number of names set on et-al-use-first.
        /// </summary>
        [XmlAttribute("et-al-use-first")]
        public int EtAlUseFirst
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'et-al-first' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool EtAlUseFirstSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// If the number of names in a name variable is truncated, it is truncated after reaching the number of names set on et-al-use-first.
        /// </summary>
        [XmlAttribute("et-al-subsequent-min")]
        public int EtAlSubsequentMin
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'et-al-subsequent-min' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool EtAlSubsequentMinSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// If the number of names in a name variable is truncated, it is truncated after reaching the number of names set on et-al-use-first.
        /// </summary>
        [XmlAttribute("et-al-subsequent-use-first")]
        public int EtAlSubsequentUseFirst
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'et-al-subsequent-use-first' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool EtAlSubsequentUseFirstSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// When set to “true” (the default is “false”), name lists truncated by et-al abbreviation are followed by the name
        /// delimiter, the ellipsis character, and the last name of the original name list. This is only possible when the
        /// original name list has at least two more names than the truncated name list.
        /// </summary>
        [XmlAttribute("et-al-use-last")]
        public bool EtAlUseLast
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'et-al-use-last' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool EtAlUseLastSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies whether all the name-parts of personal names should be displayed (value “long”, the default), or only the
        /// family name and the non-dropping-particle (value “short”). A third value, “count”, returns the total number of names
        /// that would otherwise be rendered by the use of the cs:names element (taking into account the effects of et-al abbreviation
        /// and editor/translator collapsing), which allows for advanced sorting.
        /// </summary>
        [XmlAttribute("name-form")]
        public NameFormat NameFormat
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'name-form' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool NameFormatSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies whether all the name-parts of personal names should be displayed (value “long”, the default), or only the
        /// family name and the non-dropping-particle (value “short”). A third value, “count”, returns the total number of names
        /// that would otherwise be rendered by the use of the cs:names element (taking into account the effects of et-al abbreviation
        /// and editor/translator collapsing), which allows for advanced sorting.
        /// </summary>
        NameFormat INameOptions.Format
        {
            get
            {
                return this.NameFormat;
            }
        }
        /// <summary>
        /// Indicates whether the 'name-form' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool INameOptions.FormatSpecified
        {
            get
            {
                return this.NameFormatSpecified;
            }
        }

        /// <summary>
        /// When set to “false” (the default is “true”), given names are no longer initialized when “initialize-with” is set. However,
        /// the value of “initialize-with” is still added after initials present in the full name (e.g. with initialize set to “false”,
        /// and initialize-with set to ”.”, “James T Kirk” becomes “James T. Kirk”).
        /// </summary>
        [XmlAttribute("initialize")]
        public bool Initialize
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'initialize' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool InitializeSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// When set, given names are converted to initials. The attribute value is added after each initial (”.” results in “J.J. Doe”). 
        /// </summary>
        [XmlAttribute("initialize-with")]
        public string InitializeWith
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies that names should be displayed with the given name following the family name (e.g. “John Doe” becomes “Doe, John”).
        /// </summary>
        [XmlAttribute("name-as-sort-order")]
        public NameSortOptions NameAsSortOrder
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'name-as-sort-order' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool NameAsSortOrderSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the delimiter for name-parts that have switched positions as a result of name-as-sort-order. The default value
        /// is ”, ” (“Doe, John”). As is the case for name-as-sort-order, this attribute only affects names written
        /// in the latin or Cyrillic alphabets.
        /// </summary>
        [XmlAttribute("sort-separator")]
        public string SortSeparator
        {
            get;
            set;
        }
        #endregion
    }
}
