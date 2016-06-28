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
    /// Base class for elements with affixes.
    /// </summary>
    public abstract class RenderingElement : Element
    {
        /// <summary>
        /// Gets or sets the value that is added before the output of the element carrying the attribute, but only if the element produces output.
        /// </summary>
        [XmlAttribute("prefix")]
        public string Prefix
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the value that is added after the output of the element carrying the attribute, but only if the element produces output.
        /// </summary>
        [XmlAttribute("suffix")]
        public string Suffix
        {
            get;
            set;
        }

        /// <summary>
        /// Compiles this rendering element.
        /// </summary>
        /// <param name="code"></param>
        internal virtual void Compile(Scope code)
        {
            throw new CompilerException(this, "Not supported.");
        }
    }
}
