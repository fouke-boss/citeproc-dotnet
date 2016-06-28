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
    /// The cs:label rendering element outputs the term matching the variable selected with the required variable 
    /// attribute, which must be set to “locator, “page”, or one of the number variables. The term is only rendered
    /// if the selected variable is non-empty. 
    /// </summary>
    public class LabelElement : FormattingElement, IStripPeriods
    {
        /// <summary>
        /// The term/variable to be rendered.
        /// </summary>
        [XmlAttribute("variable")]
        public string Variable
        {
            get;
            set;
        }

        /// <summary>
        /// Selects the format of the term.
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
        /// Sets pluralization of the term: contextual (default), always or never.
        /// </summary>
        [XmlAttribute("plural")]
        public LabelPluralization Plurilization
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'plural' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool PlurilizationSpecified
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
        /// Compiles the Label element.
        /// </summary>
        /// <param name="code"></param>
        internal override void Compile(Scope code)
        {
            // validate:form
            if (this.FormatSpecified && (this.Format == TermFormat.Verb || this.Format == TermFormat.VerbShort))
            {
                throw new CompilerException(this, "Format '{0}' is not allowed on a cs:label element.", this.Format);
            }

            // find matching term
            var term = Utility.GetTerm(this.Variable);
            if (term == null)
            {
                throw new CompilerException(this, "No label is defined for variable '{0}'.", this.Variable);
            }

            // invoke
            using (var method = code.AppendMethodInvoke("this.RenderLabel", this))
            {
                // parameters
                method.AddElement(this);
                method.AddLiteral(this.Variable);
                method.AddLiteral(term);
                method.AddLiteral(this.FormatSpecified ? this.Format : TermFormat.Long);
                method.AddLiteral(this.PlurilizationSpecified ? this.Plurilization : LabelPluralization.Contextual);
                method.AddLiteral(this.Prefix);
                method.AddLiteral(this.Suffix);
                method.AddContextAndParameters();
            }
        }
    }
}