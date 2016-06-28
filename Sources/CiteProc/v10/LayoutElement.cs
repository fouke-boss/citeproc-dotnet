using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CiteProc.Compilation;

namespace CiteProc.v10
{
    public class LayoutElement : FormattingElement
    {
        /// <summary>
        /// The delimiter attribute delimits non-empty pieces of output.
        /// </summary>
        [XmlAttribute("delimiter")]
        public string Delimiter
        {
            get;
            set;
        }

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
        /// Compiles this Layout element.
        /// </summary>
        /// <param name="code"></param>
        internal override void Compile(Scope code)
        {
            // invoke
            using (var method = code.AppendMethodInvoke("this.RenderLayout", this))
            {
                // parameters
                method.AddElement(this);
                method.AddLiteral(this.Prefix);
                method.AddLiteral(this.Suffix);
                method.AddContextAndParameters();

                // children
                using (var lambda = method.AddLambdaExpression(true, "new Result[]"))
                {
                    lambda.AppendArray(this.Children.Cast<RenderingElement>(), child => child.Compile(lambda), () => lambda.Append("null"));
                }
            }
        }
    }
}