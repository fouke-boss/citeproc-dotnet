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
    /// Specifies the sorting order of cites within citations, and bibliographic entries within the bibliography respectively.
    /// </summary>
    public class SortElement : Element
    {
        /// <summary>
        /// The list of sort keys.
        /// </summary>
        [XmlElement("key")]
        public KeyElement[] Keys
        {
            get;
            set;
        }

        /// <summary>
        /// Compiles this Sort element.
        /// </summary>
        /// <param name="code"></param>
        internal void Compile(LambdaExpression code)
        {
            // invoke
            using (var method = code.AppendMethodInvoke("this.RenderSort", this))
            {
                // parameters
                method.AddElement(this);
                method.AddContextAndParameters();

                // children
                using (var lambda = method.AddLambdaExpression(true, "new string[]"))
                {
                    lambda.AppendArray(this.Keys, key => key.Compile(lambda), () => lambda.Append("null"));
                }
            }
        }
    }
}