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
    /// Represents a cs:else conditional branch inside a cs:choose element.
    /// </summary>
    public class ElseElement : Element
    {
        /// <summary>
        /// Gets or sets the rendered child elements if the condition is met.
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
        /// Compiles this Else element.
        /// </summary>
        /// <param name="code"></param>
        internal virtual void Compile(Scope code)
        {
            // init
            code.Append("{{return ");

            // array
            code.AppendArray("Result", this.Children, (child, scope) => child.Compile(scope));

            // done
            code.Append(";}}");


            //code.Append("{{return new Result[]");
            //using (var block = code.AppendBlock(false, ";}"))
            //{
            //    if (this.Children != null)
            //    {
            //        // compile children
            //        int index = 0;
            //        foreach (var child in this.Children.Cast<RenderingElement>())
            //        {
            //            // compile
            //            block.AppendIndent();
            //            child.Compile(block);

            //            // comma
            //            if (index < this.Children.Length - 1)
            //            {
            //                block.Append(",");
            //            }

            //            // done
            //            block.AppendLineBreak();
            //        }
            //    }
            //}
        }
    }
}