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
    /// The cs:text rendering element outputs text. It must carry one of the following attributes to select what
    /// should be rendered: variable, macro, value or term.
    /// </summary>
    public class TextElement : FormattingElement, IStripPeriods
    {
        /// <summary>
        /// Renders the text contents of a variable. 
        /// </summary>
        [XmlAttribute("variable")]
        public string Variable
        {
            get;
            set;
        }
        /// <summary>
        /// Renders the text output of a macro. Attribute value must match the value of the name attribute of a cs:macro element.
        /// </summary>
        [XmlAttribute("macro")]
        public string Macro
        {
            get;
            set;
        }
        /// <summary>
        /// Renders the literal text in the value attribute.
        /// </summary>
        [XmlAttribute("value")]
        public string Value
        {
            get;
            set;
        }
        /// <summary>
        /// Renders a term. May be accompanied by the plural attribute to select the singular or plural variant of a term, and by
        /// the form attribute to select the “long” (default), “short”, “verb”, “verb-short” or “symbol” form variant.
        /// </summary>
        [XmlAttribute("term")]
        public TermName Term
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'term' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool TermSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// The plural attribute selects the singular (“false”, default) or plural (“true”) variant of a term.
        /// </summary>
        [XmlAttribute("plural")]
        public bool Plural
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'plural' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool PluralSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// The form attribute Select the “long” (default), “short”, “verb”, “verb-short” or “symbol” form variant of a term.
        /// </summary>
        [XmlAttribute("form")]
        public TermFormat Format
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'form' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool FormatSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// When set to “true” (“false” is default), the rendered text is wrapped in quotes (the quotation marks used are terms). The
        /// localized punctuation-in-quote option controls whether an adjoining comma or period appears outside (default) or
        /// inside the closing quotation mark.
        /// </summary>
        [XmlAttribute("quotes")]
        public bool Quotes
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'quotes' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool QuotesSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// When set to “true” (“false” is the default), any periods in the rendered text are removed.
        /// </summary>
        [XmlAttribute("strip-periods")]
        public bool StripPeriods
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'strip-periods' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool StripPeriodsSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// When specified, converts the rendered text to the given case.
        /// </summary>
        [XmlAttribute("text-case")]
        public TextCase TextCase
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'text-case' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool TextCaseSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// The display attribute (similar the “display” property in CSS) may be used to structure individual bibliographic entries
        /// into one or more text blocks. If used, all rendering elements should be under the control of a display attribute. 
        /// </summary>
        [XmlAttribute("display")]
        public Display Display
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'display' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool DisplaySpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Compiles this Text element.
        /// </summary>
        /// <param name="code"></param>
        internal override void Compile(Scope code)
        {
            // count the number of options specifies
            var count = new bool[] { !string.IsNullOrEmpty(this.Variable), (this.Value != null), (this.Macro != null), this.TermSpecified, }
                .Where(x => x)
                .Count();
            if (count != 1)
            {
                throw new CompilerException(this, "One and only one of the attributes 'variable', 'value', 'macro' and 'term' is allowed.");
            }

            // validate
            if (this.DisplaySpecified)
            {
                throw new FeatureNotSupportedException("display attribute");
            }

            // validate form
            if (this.Variable != null && this.FormatSpecified && this.Format != TermFormat.Long && this.Format != TermFormat.Short)
            {
                throw new CompilerException(this, "Format '{0}' is not allowed.", this.Format);
            }
            else if ((this.Value != null || this.Macro != null) && this.FormatSpecified)
            {
                throw new CompilerException(this, "Format cannot be specified.");
            }

            // add context
            using (var method = code.AppendMethodInvoke(null, this))
            {
                // by type
                if (!string.IsNullOrEmpty(this.Value))
                {
                    method.Name = "this.RenderTextByValue";
                    method.AddElement(this);
                    method.AddLiteral(this.Value);
                }
                else if (!string.IsNullOrEmpty(this.Variable))
                {
                    method.Name = "this.RenderTextByVariable";
                    method.AddElement(this);
                    method.AddLiteral(this.Variable.ToLower());
                    method.AddLiteral(Utility.GetXmlEnum<TermName>(this.Variable));
                    method.AddLiteral(this.FormatSpecified ? this.Format : TermFormat.Long);
                }
                else if (!string.IsNullOrEmpty(this.Macro))
                {
                    method.Name = "this.RenderTextByMacro";
                    method.AddElement(this);
                    method.AddCode("this.{0}", code.Root.GetMacro(this.Macro));
                }
                else if (this.TermSpecified)
                {
                    method.Name = "this.RenderTextByTerm";
                    method.AddElement(this);
                    method.AddLiteral(this.Term);
                    method.AddLiteral(this.FormatSpecified ? this.Format : TermFormat.Long);
                    method.AddLiteral(this.PluralSpecified ? this.Plural : false);
                }
                else
                {
                    throw new NotSupportedException();
                }

                // default text parameters
                method.AddLiteral(this.Prefix);
                method.AddLiteral(this.Suffix);
                method.AddLiteral(this.QuotesSpecified && this.Quotes ? true : false);
                method.AddLiteral(this.TextCaseSpecified ? (object)this.TextCase : null);

                // context parameters
                method.AddContextAndParameters();
            }
        }
    }
}
