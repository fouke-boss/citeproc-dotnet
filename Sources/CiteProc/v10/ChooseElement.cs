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
    /// The cs:choose rendering element allows for conditional rendering of rendering elements.
    /// </summary>
    public class ChooseElement : RenderingElement
    {
        /// <summary>
        /// Represents the first, 'if', conditional branch.
        /// </summary>
        [XmlElement("if")]
        public IfElement If
        {
            get;
            set;
        }
        /// <summary>
        /// Represents the subsequent, 'else if' conditional branches.
        /// </summary>
        [XmlElement("else-if")]
        public IfElement[] ElseIf
        {
            get;
            set;
        }
        /// <summary>
        /// Represents the last, 'else' conditional branch.
        /// </summary>
        [XmlElement("else")]
        public ElseElement Else
        {
            get;
            set;
        }

        /// <summary>
        /// Compiles the Choose element.
        /// </summary>
        /// <param name="code"></param>
        internal override void Compile(Scope code)
        {
            // invoke
            using (var method = code.AppendMethodInvoke("this.RenderChoose", this))
            {
                // parameters
                method.AddElement(this);
                method.AddContextAndParameters();

                // lambda expression
                using (var lambda = method.AddLambdaExpression(true))
                {
                    // if
                    lambda.Append("if");
                    this.If.Compile(lambda);

                    // else if
                    if (this.ElseIf != null)
                    {
                        foreach (var elseif in this.ElseIf)
                        {
                            lambda.AppendIndent();
                            lambda.Append("else if");
                            elseif.Compile(lambda);
                        }
                    }

                    // else
                    lambda.AppendIndent();
                    lambda.Append("else");
                    (this.Else ?? new ElseElement()).Compile(lambda);
                }
            }
        }
    }
}
