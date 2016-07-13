using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using CiteProc.Compilation;

namespace CiteProc.v10
{
    /// <summary>
    /// Represents a single Citation Style Language style file.
    /// </summary>
    [XmlRoot("style", Namespace = File.NAMESPACE_URI)]
    public class StyleFile : File, IGlobalOptions, INamesOptions, INameOptions
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StyleFile()
            : base(new Version(1, 0, 1))
        {
        }

        /// <summary>
        /// Indicates whether this style is an dependent style.
        /// </summary>
        public bool IsDependent
        {
            get
            {
                return ((this.Macros == null || this.Macros.Length == 0) && this.Citation == null && this.Bibliography == null);
            }
        }

        #region Attributes
        /// <summary>
        /// Determines whether the style uses in-text citations (value “in-text”) or notes (“note”).
        /// </summary>
        [XmlAttribute("class")]
        public CitationClass @Class
        {
            get;
            set;
        }
        /// <summary>
        /// Sets a default locale for style localization. Value must be a locale code.
        /// </summary>
        [XmlAttribute("default-locale")]
        public string DefaultLocale
        {
            get;
            set;
        }
        #endregion

        #region Elements
        /// <summary>
        /// Contains the metadata describing the style (style name, ID, authors, etc.).
        /// </summary>
        [XmlElement("info")]
        public StyleInfoElement Info
        {
            get;
            set;
        }

        /// <summary>
        /// Used to specify (overriding) localization data.
        /// </summary>
        [XmlElement("locale")]
        public LocaleElement[] Locales
        {
            get;
            set;
        }

        /// <summary>
        /// Macros allow formatting instructions to be reused, keeping styles compact and maintainable.
        /// </summary>
        [XmlElement("macro")]
        public MacroElement[] Macros
        {
            get;
            set;
        }
        /// <summary>
        /// Describes the formatting of in-text citations or notes.
        /// </summary>
        [XmlElement("citation")]
        public CitationElement Citation
        {
            get;
            set;
        }
        /// <summary>
        /// Describes the formatting of the bibliography.
        /// </summary>
        [XmlElement("bibliography")]
        public BibliographyElement Bibliography
        {
            get;
            set;
        }
        #endregion

        public new StyleFile Clone()
        {
            return (StyleFile)base.Clone();
        }

        #region Compilation
        /// <summary>
        /// Compiles the style into a processor instance.
        /// </summary>
        /// <returns></returns>
        public Processor Compile()
        {
            return Processor.Compile(this);
        }

        /// <summary>
        /// Compiles this style element.
        /// </summary>
        /// <param name="code"></param>
        internal void Compile(Class code)
        {
            // special elements
            code.Root.SetSpecialElement(this.Bibliography);
            code.Root.SetSpecialElement(this.GetFirstNamesElementInBibliography());

            // default locale
            if (!string.IsNullOrEmpty(this.DefaultLocale))
            {
                code.AppendProperty("protected override string DefaultLocale", this.DefaultLocale);
            }

            // global options
            if (this.InitializeWithHyphenSpecified)
            {
                code.AppendProperty("protected override bool InitializeWithHyphen", this.InitializeWithHyphen);
            }
            if (this.PageRangeFormatSpecified)
            {
                code.AppendProperty("protected override PageRangeFormats PageRangeFormat", this.PageRangeFormat);
            }
            if (this.DemoteNonDroppingParticleSpecified)
            {
                code.AppendProperty("protected override DemotingBehavior DemoteNonDroppingParticle", this.DemoteNonDroppingParticle);
            }

            // macros
            if (this.Macros != null)
            {
                // macro names unique
                var error = this.Macros
                    .GroupBy(x => x.Name)
                    .FirstOrDefault(x => x.Count() > 1);
                if (error != null)
                {
                    throw new CompilerException(error.Last(), "A macro with name '{0}' already exists.", error.Key);
                }

                // register macros
                code.Root.RegisterMacros(this.Macros.Select(x => x.Name));

                // render macros
                foreach (var macro in this.Macros)
                {
                    using (var block = code.AppendRenderingMethod(code.Root.GetMacro(macro.Name), "Result", "private"))
                    {
                        macro.Compile(block);
                    }
                }
            }

            // first name
            var tmp = this.GetFirstNamesElementInBibliography();

            // GenerateBibliographyLayout
            using (var block = code.AppendRenderingMethod("GenerateBibliographyLayout", "ComposedRun", "protected override"))
            {
                Compile(block, this.Bibliography ?? new BibliographyElement(), (scope, child) => child.CompileLayout(scope));
            }

            // GenerateBibliographySort
            using (var block = code.AppendRenderingMethod("GenerateBibliographySort", "string[]", "protected override"))
            {
                Compile(block, this.Bibliography ?? new BibliographyElement(), (scope, child) => child.CompileSort(scope));
            }

            // GetBibliographySortComparer
            if (this.Bibliography != null && this.Bibliography.Sort != null)
            {
                using (var block = code.AppendMethod("protected override SortComparer GetBibliographySortComparer()"))
                {
                    block.AppendIndent();
                    using (var method = block.AppendMethodInvoke("return new SortComparer", null))
                    {
                        method.AddSortComparerParameters(this.Bibliography.Sort);
                    }
                    block.Append(";");
                    block.AppendLineBreak();
                }
            }

            // GenerateCitation
            using (var block = code.AppendMethod("public override ComposedRun GenerateCitation(IDataProvider[] items, string locale, bool forceLocale)"))
            {
                block.AppendIndent();
                using (var method = block.AppendMethodInvoke("return this.GenerateCitation", null))
                {
                    // parameters
                    method.AddCode("items");
                    method.AddCode("locale");
                    method.AddCode("forceLocale");
                    method.AddLiteral(this.Citation == null || this.Citation.Layout == null ? null : this.Citation.Layout.Delimiter);
                    method.AddSortComparer(this.Citation);
                }
                block.Append(";");
                block.AppendLineBreak();
            }

            // GenerateCitationEntry
            using (var block = code.AppendRenderingMethod("GenerateCitationEntry", "Entry", "protected override"))
            {
                this.Compile(block, this.Citation ?? new CitationElement(), (scope, child) => child.Compile(scope));
            }

            // disambiguate
            using (var block = code.AppendMethod("protected override DisambiguationContext Disambiguate(IEnumerable<Item> items)"))
            {
                // init
                var citation = this.Citation ?? new CitationElement();

                // compile
                block.AppendIndent();
                using (var method = block.AppendMethodInvoke("return this.Disambiguate", null))
                {
                    // parameters
                    method.AddCode("items.ToArray()");
                    method.AddLiteral(citation.DisambiguateAddNamesSpecified ? citation.DisambiguateAddNames : false);
                    method.AddLiteral(citation.DisambiguateAddGivenNameSpecified ? true : false);
                    method.AddLiteral(citation.DisambiguateAddYearSuffixSpecified ? citation.DisambiguateAddYearSuffix : false);
                }
                block.Append(";");
                block.AppendLineBreak();
            }
        }
        private void Compile<T>(Scope code, T child, Action<Scope, T> action)
            where T : EntryElement
        {
            // invoke
            code.AppendIndent();
            using (var method = code.AppendMethodInvoke("return this.RenderStyle", this))
            {
                // parameters
                method.AddContextAndParameters();

                // child?
                using (var lambda = method.AddLambdaExpression(false))
                {
                    action(lambda, child);
                }
            }

            // done
            code.Append(";");
            code.AppendLineBreak();
        }

        private NamesElement GetFirstNamesElementInBibliography()
        {
            // init
            NamesElement result = null;

            // find layout in bibliography
            if (this.Bibliography != null && this.Bibliography.Layout != null)
            {
                // init
                result = this.Bibliography.Layout.GetChildren()
                    .OfType<NamesElement>()
                    .FirstOrDefault();
                if (result != null)
                {
                }
            }

            // done
            return result;
        }

        #endregion

        #region Global options
        /// <summary>
        /// Specifies whether compound given names (e.g. “Jean-Luc”) should be initialized with a hyphen 
        /// (“J.-L.”, value “true”, default) or without (“J.L.”, value “false”).
        /// </summary>
        [XmlAttribute("initialize-with-hyphen")]
        public bool InitializeWithHyphen
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'initialize-with-hyphen' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool InitializeWithHyphenSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Activates expansion or collapsing of page ranges. If the attribute is not set, page ranges are rendered without reformatting.
        /// </summary>
        [XmlAttribute("page-range-format")]
        public PageRangeFormats PageRangeFormat
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'page-range-format' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool PageRangeFormatSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the display and sorting behavior of the non-dropping-particle in inverted names (e.g. “Koning, W. de”).
        /// </summary>
        [XmlAttribute("demote-non-dropping-particle")]
        public DemotingBehavior DemoteNonDroppingParticle
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'demote-non-dropping-particle' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool DemoteNonDroppingParticleSpecified
        {
            get;
            set;
        }
        #endregion

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