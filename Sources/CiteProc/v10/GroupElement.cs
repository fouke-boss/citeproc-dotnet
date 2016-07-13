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
    /// The cs:group rendering element must contain one or more rendering elements (with the exception of cs:layout). cs:group 
    /// may carry the delimiter attribute to separate its child elements, as well as affixes and display attributes (applied to
    /// the output of the group as a whole) and formatting attributes (transmitted to the enclosed elements). cs:group implicitly
    /// acts as a conditional: cs:group and its child elements are suppressed if a) at least one rendering element in cs:group
    /// calls a variable (either directly or via a macro), and b) all variables that are called are empty.
    /// </summary>
    public class GroupElement : FormattingElement
    {
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
        /// The delimiter attribute delimits non-empty pieces of output.
        /// </summary>
        [XmlAttribute("delimiter")]
        public string Delimiter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rendering elements to be grouped.
        /// </summary>
        [XmlElement("choose", Type = typeof(ChooseElement))]
        [XmlElement("date", Type = typeof(DateElement))]
        [XmlElement("group", Type = typeof(GroupElement))]
        [XmlElement("number", Type = typeof(NumberElement))]
        [XmlElement("names", Type = typeof(NamesElement))]
        [XmlElement("label", Type = typeof(LabelElement))]
        [XmlElement("text", Type = typeof(TextElement))]
        public RenderingElement[] Children
        {
            get;
            set;
        }

        /// <summary>
        /// Compiles this group element.
        /// </summary>
        /// <param name="code"></param>
        internal override void Compile(Scope code)
        {
            // validate
            if (this.DisplaySpecified)
            {
                throw new FeatureNotSupportedException("display attribute");
            }

            // invoke
            using (var method = code.AppendMethodInvoke("this.RenderGroup", this))
            {
                // parameters
                method.AddElement(this);
                method.AddLiteral(this.Delimiter);
                method.AddLiteral(this.Prefix);
                method.AddLiteral(this.Suffix);
                method.AddContextAndParameters();

                // children
                using (var lambda = method.AddLambdaExpression(false))
                {
                    lambda.AppendArray("Result", this.Children.Cast<RenderingElement>(), (child, scope) => child.Compile(scope), scope => scope.Append("null"));
                }
            }
        }

        /// <summary>
        /// Returns all child elements.
        /// </summary>
        /// <returns></returns>
        internal override IEnumerable<RenderingElement> GetChildren()
        {
            return base.GetChildren().Concat(this.Children.SelectMany(x => x.GetChildren()));
        }
    }
}
