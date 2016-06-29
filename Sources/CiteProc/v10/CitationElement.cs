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
    /// The cs:citation element describes the formatting of citations, which consist of one or more
    /// references (“cites”) to bibliographic sources. Citations appear in the form of either in-text
    /// citations (in the author (e.g. “[Doe]”), author-date (“[Doe 1999]”), label (“[doe99]”)
    /// or number (“[1]”) format) or notes.
    /// </summary>
    public class CitationElement : EntryElement
    {
        /// <summary>
        /// If set to “true” (“false” is the default), names that would otherwise be hidden as a result of et-al abbreviation
        /// are added one by one to all members of a set of ambiguous cites, until no more cites in the set can be disambiguated by adding names.
        /// </summary>
        [XmlAttribute("disambiguate-add-names")]
        public bool DisambiguateAddNames
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'disambiguate-add-names' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool DisambiguateAddNamesSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// If set to “true” (“false” is the default), ambiguous names (names that are identical in their “short” or
        /// initialized “long” form, but differ when initials are added or the full given name is shown) are expanded.
        /// Name expansion can be configured with givenname-disambiguation-rule.
        /// </summary>
        [XmlAttribute("disambiguate-add-givenname")]
        public bool DisambiguateAddGivenName
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'disambiguate-add-givenname' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool DisambiguateAddGivenNameSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies a) whether the purpose of name expansion is limited to disambiguating cites, or has the additional
        /// goal of disambiguating names (only in the latter case are ambiguous names in unambigous cites expanded, e.g.
        /// from “(Doe 1950; Doe 2000)” to “(Jane Doe 1950; John Doe 2000)”), b) whether name expansion targets all, or
        /// just the first name of each cite, and c) the method by which each name is expanded.
        /// </summary>
        [XmlAttribute("givenname-disambiguation-rule")]
        public GivenNameDisambiguationRules GivenNameDisambiguationRule
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'givenname-disambiguation-rule' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool GivenNameDisambiguationRuleSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// If set to “true” (“false” is the default), an alphabetic year-suffix is added to ambiguous cites (e.g.
        /// “Doe 2007, Doe 2007” becomes “Doe 2007a, Doe 2007b”) and to their corresponding bibliographic entries.
        /// The assignment of the year-suffixes follows the order of the bibliographies entries, and additional letters
        /// are used once “z” is reached (“z”, “aa”, “ab”, ..., “az”, “ba”, etc.). By default the year-suffix is appended
        /// to the cite, and to the first year rendered through cs:date in the bibliographic entry, but its location
        /// can be controlled by explicitly rendering the “year-suffix” variable using cs:text. If “year-suffix” is
        /// rendered through cs:text in the scope of cs:citation, it is suppressed for cs:bibliography, unless it is
        /// also rendered through cs:text in the scope of cs:bibliography, and vice versa.
        /// </summary>
        [XmlAttribute("disambiguate-add-year-suffix")]
        public bool DisambiguateAddYearSuffix
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'disambiguate-add-year-suffix' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool DisambiguateAddYearSuffixSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Activates cite grouping and specifies the delimiter for cites within a cite group. Defaults to ”, ”. E.g. with
        /// delimiter on cs:layout in cs:citation set to ”; ”, collapse set to “year”, and cite-group-delimiter set to ”,”,
        /// citations look like “(Doe 1999,2001; Jones 2000)”.
        /// </summary>
        [XmlAttribute("cite-group-delimiter")]
        public string CiteGroupDelimiter
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'cite-group-delimiter' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool CiteGroupDelimiterSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Activates cite grouping and collapsing.
        /// </summary>
        [XmlAttribute("collapse")]
        public Collapse Collapse
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'collapse' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool CollapseSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the delimiter for year-suffixes. Defaults to the delimiter set on cs:layout in cs:citation.
        /// E.g. with collapse set to “year-suffix”, delimiter on cs:layout in cs:citation set to ”; ”, and
        /// year-suffix-delimiter set to ”,”, citations look like “(Doe 1999a,b; Jones 2000)”.
        /// </summary>
        [XmlAttribute("year-suffix-delimiter")]
        public string YearSuffixDelimiter
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'year-suffix-delimiter' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool YearSuffixDelimiterSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the cite delimiter to be used after a collapsed cite group. Defaults to the delimiter set on cs:layout
        /// in cs:citation. E.g. with collapse set to “year”, delimiter on cs:layout in cs:citation set to ”, ”, and
        /// after-collapse-delimiter set to ”; ”, citations look like “(Doe 1999, 2001; Jones 2000, Brown 2001)”.
        /// </summary>
        [XmlAttribute("after-collapse-delimiter")]
        public string AfterCollapseDelimiter
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'after-collapse-delimiter' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool AfterCollapseDelimiterSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// A cite tests true for the “near-note” position (see Choose) when a preceding note exists that a) refers to the
        /// same item and b) does not precede the current note by more footnotes or endnotes than the value of
        /// near-note-distance (default value is “5”).
        /// </summary>
        [XmlAttribute("near-note-distance")]
        public int NearNoteDistance
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'near-note-distance' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool NearNoteDistanceSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Compiles the Citation element.
        /// </summary>
        /// <param name="code"></param>
        internal override void Compile(Scope code)
        {
            // disambiguation is not yet supported
            if (this.DisambiguateAddGivenNameSpecified || this.DisambiguateAddNamesSpecified || this.DisambiguateAddYearSuffixSpecified || this.GivenNameDisambiguationRuleSpecified)
            {
                throw new FeatureNotSupportedException("disambiguation");
            }

            // cite grouping is not yet supported
            if (this.CiteGroupDelimiterSpecified)
            {
                throw new FeatureNotSupportedException("cite grouping");
            }

            // cite collapsing is not yet supported
            if (this.CollapseSpecified || this.YearSuffixDelimiterSpecified || this.AfterCollapseDelimiterSpecified)
            {
                throw new FeatureNotSupportedException("cite collapsing");
            }

            // near-note-distance is not yet supported
            if (this.NearNoteDistanceSpecified)
            {
                throw new FeatureNotSupportedException("near-note-distance attribute");
            }

            // invoke
            using (var method = code.AppendMethodInvoke("this.RenderCitation", this))
            {
                // parameters
                method.AddContextAndParameters();

                // layout
                using (var lambda = method.AddLambdaExpression(false))
                {
                    (this.Layout ?? new LayoutElement()).Compile(lambda);
                }

                // sort
                using (var lambda = method.AddLambdaExpression(false))
                {
                    (this.Sort ?? new SortElement()).Compile(lambda);
                }
            }
        }
    }
}
