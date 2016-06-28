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
    /// The cs:names rendering element outputs the contents of one or more name variables (selected with the
    /// required variable attribute), each of which can contain multiple names (e.g. the “author” variable
    /// contains all the author names of the cited item). If multiple variables are selected (separated by
    /// single spaces, see example below), each variable is independently rendered in the order specified,
    /// with one exception: when the selection consists of “editor” and “translator”, and when the contents
    /// of these two name variables is identical, then the contents of only one name variable is rendered.
    /// </summary>
    public class NamesElement : FormattingElement, INamesOptions
    {
        /// <summary>
        /// Selects the names variables to be used.
        /// </summary>
        [XmlAttribute("variable")]
        public string Variable
        {
            get;
            set;
        }
        /// <summary>
        /// Separates the names of the different name variables (e.g. the semicolon in “Doe, Smith (editors); Johnson (translator)”).
        /// </summary>
        [XmlAttribute("delimiter")]
        public string Delimiter
        {
            get;
            set;
        }

        /// <summary>
        /// The cs:name element can be used to describe the formatting of individual names, and the separation of names within a name variable.
        /// </summary>
        [XmlElement("name")]
        public NameElement Name
        {
            get;
            set;
        }
        /// <summary>
        /// Et-al abbreviation can be further customized with the optional cs:et-al element.
        /// </summary>
        [XmlElement("et-al")]
        public EtAlElement EtAl
        {
            get;
            set;
        }
        /// <summary>
        /// The optional cs:substitute element adds substitution in case the name variables specified in the parent cs:names
        /// element are empty. The substitutions are specified as child elements of cs:substitute, and must consist of one
        /// or more rendering elements (with the exception of cs:layout).
        /// </summary>
        [XmlElement("substitute")]
        public SubstituteElement Substitute
        {
            get;
            set;
        }
        [XmlElement("label")]
        public LabelElement Label
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

        internal override void Compile(Scope code)
        {
            // validate
            if (this.Substitute != null)
            {
                throw new FeatureNotSupportedException("substitute element");
            }

            // validate
            if (this.DisplaySpecified)
            {
                throw new FeatureNotSupportedException("display attribute");
            }

            // invoke
            using (var method = code.AppendMethodInvoke("this.RenderNames", this))
            {
                // variables
                var variables = this.Variable.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();

                // terms
                var terms = variables
                    .Select(x => Utility.GetTerm(x))
                    .Select(x => (x == null ? "(TermName?)null" : Compiler.GetLiteral(x.Value)))
                    .ToArray();

                // parameters
                method.AddElement(this);
                method.AddCode(string.Format("new string[]{{{{{0}}}}}", string.Join(",", variables.Select(x => Compiler.GetLiteral(x)))));
                method.AddCode(string.Format("new TermName?[]{{{{{0}}}}}", string.Join(",", terms)));
                method.AddLiteral(this.Prefix);
                method.AddLiteral(this.Suffix);
                method.AddContextAndParameters();

                // name parameters
                using (var lambda = method.AddLambdaExpression(false))
                {
                    // init
                    using (var m = lambda.AppendMethodInvoke("new NameParameters", this.Name))
                    {
                        (this.Name ?? new NameElement()).Compile(m);
                    }
                }

                // et al parameters
                using (var lambda = method.AddLambdaExpression(false))
                {
                    // init
                    var etAl = (this.EtAl ?? new EtAlElement());

                    // method
                    using (var m = lambda.AppendMethodInvoke("new EtAlParameters", etAl))
                    {
                        etAl.Compile(m);
                    }
                }

                // label parameters
                using (var lambda = method.AddLambdaExpression(false))
                {
                    if (this.Label == null)
                    {
                        lambda.Append("null");
                    }
                    else
                    {
                        // method
                        using (var m = lambda.AppendMethodInvoke("new LabelParameters", this.Label))
                        {
                            m.AddElement(this.Label);
                            m.AddCode(m.ParameterName);
                            m.AddLiteral(this.Label.FormatSpecified ? this.Label.Format : TermFormat.Long);
                            m.AddLiteral(this.Label.PlurilizationSpecified ? this.Label.Plurilization : LabelPluralization.Contextual);
                            m.AddLiteral(this.Label.Prefix);
                            m.AddLiteral(this.Label.Suffix);
                            m.AddLiteral(this.Label.TextCaseSpecified ? (object)this.Label.TextCase : null);
                            m.AddDefaultParameters();
                        }
                    }
                }
            }
        }
    }
}
