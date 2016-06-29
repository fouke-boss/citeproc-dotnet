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
    /// The cs:bibliography element describes the formatting of bibliographies, which list one or more bibliographic sources.
    /// </summary>
    public class BibliographyElement : EntryElement
    {
        /// <summary>
        /// If set to “true” (“false” is the default), bibliographic entries are rendered with hanging-indents.
        /// </summary>
        [XmlAttribute("hanging-indent")]
        public bool HangingIndent
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'hanging-indent' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool HangingIndentSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// If set, subsequent lines of bibliographic entries are aligned along the second field.
        /// </summary>
        [XmlAttribute("second-field-align")]
        public SecondFieldAlign SecondFieldAlign
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'hanging-indent' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool SecondFieldAlignSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies vertical line distance. Defaults to “1” (single-spacing), and can be set to any positive integer
        /// to specify a multiple of the standard unit of line height (e.g. “2” for double-spacing).
        /// </summary>
        [XmlAttribute("line-spacing")]
        public int LineSpacing
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'line-spacing' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool LineSpacingSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies vertical distance between bibliographic entries. By default (with a value of “1”), entries are separated
        /// by a single additional line-height (as set by the line-spacing attribute). Can be set to any non-negative integer
        /// to specify a multiple of this amount.
        /// </summary>
        [XmlAttribute("entry-spacing")]
        public int EntrySpacing
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'entry-spacing' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool EntrySpacingSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// If set, the value of this attribute replaces names in a bibliographic entry that also occur in the preceding entry.
        /// The exact method of substitution depends on the value of the subsequent-author-substitute-rule attribute. Substitution
        /// is limited to the names of the first cs:names element rendered.
        /// </summary>
        [XmlAttribute("subsequent-author-substitute")]
        public string SubsequentAuthorSubstitute
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'subsequent-author-substitute' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool SubsequentAuthorSubstituteSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies when and how names are substituted as a result of the subsequent-author-substitute attribute.
        /// </summary>
        [XmlAttribute("subsequent-author-substitute-rule")]
        public SubsequentAuthorSubstituteRules SubsequentAuthorSubstituteRule
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'subsequent-author-substitute-rule' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool SubsequentAuthorSubstituteRuleSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Compiles the bibliography element.
        /// </summary>
        /// <param name="code"></param>
        internal override void Compile(Scope code)
        {
            // options are not yet supported
            if (this.HangingIndentSpecified)
            {
                //throw new FeatureNotSupportedException("hanging-indent attribute");
            }
            if (this.SecondFieldAlignSpecified)
            {
                //throw new FeatureNotSupportedException("second-field-align attribute");
            }
            if (this.LineSpacingSpecified)
            {
                //throw new FeatureNotSupportedException("line-spacing attribute");
            }
            if (this.EntrySpacingSpecified)
            {
                //throw new FeatureNotSupportedException("entry-spacing attribute");
            }

            // subsequent author substitution not yet supported
            if (this.SubsequentAuthorSubstituteSpecified || this.SubsequentAuthorSubstituteRuleSpecified)
            {
                throw new FeatureNotSupportedException("subsequent-author-substitution");
            }

            // invoke
            using (var method = code.AppendMethodInvoke("this.RenderBibliography", this))
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
