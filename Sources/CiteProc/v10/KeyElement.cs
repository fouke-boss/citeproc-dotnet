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
    /// Represents a sort key.
    /// </summary>
    public class KeyElement : Element, IEtAlOptions
    {
        /// <summary>
        /// The variable that is used for sorting.
        /// </summary>
        [XmlAttribute("variable")]
        public string Variable
        {
            get;
            set;
        }
        /// <summary>
        /// The macro that is used for sorting.
        /// </summary>
        [XmlAttribute("macro")]
        public string Macro
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the direction of the sort.
        /// </summary>
        [XmlAttribute("sort")]
        public SortOrder SortOrder
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'sort' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool SortOrderSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// If the number of names in a name variable matches or exceeds the number set on et-al-min, the rendered name list is truncated.
        /// </summary>
        [XmlAttribute("names-min")]
        public int EtAlMin
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'et-al-min' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool EtAlMinSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// If the number of names in a name variable is truncated, it is truncated after reaching the number of names set on et-al-use-first.
        /// </summary>
        [XmlAttribute("names-use-first")]
        public int EtAlUseFirst
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'et-al-first' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool EtAlUseFirstSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// When set to “true” (the default is “false”), name lists truncated by et-al abbreviation are followed by the name
        /// delimiter, the ellipsis character, and the last name of the original name list. This is only possible when the
        /// original name list has at least two more names than the truncated name list.
        /// </summary>
        [XmlAttribute("names-use-last")]
        public bool EtAlUseLast
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'et-al-use-last' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool EtAlUseLastSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// If the number of names in a name variable is truncated, it is truncated after reaching the number of names set on et-al-use-first.
        /// </summary>
        int IEtAlOptions.EtAlSubsequentMin
        {
            get
            {
                return this.EtAlMin;
            }
        }
        /// <summary>
        /// Indicates whether the 'et-al-subsequent-min' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool IEtAlOptions.EtAlSubsequentMinSpecified
        {
            get
            {
                return this.EtAlMinSpecified;
            }
        }

        /// <summary>
        /// If the number of names in a name variable is truncated, it is truncated after reaching the number of names set on et-al-use-first.
        /// </summary>
        int IEtAlOptions.EtAlSubsequentUseFirst
        {
            get
            {
                return this.EtAlUseFirst;
            }
        }
        /// <summary>
        /// Indicates whether the 'et-al-subsequent-use-first' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool IEtAlOptions.EtAlSubsequentUseFirstSpecified
        {
            get
            {
                return this.EtAlUseFirstSpecified;
            }
        }

        /// <summary>
        /// Compiles this Key element.
        /// </summary>
        /// <param name="code"></param>
        internal void Compile(Scope code)
        {
            var count = new bool[] { !string.IsNullOrEmpty(this.Variable), !string.IsNullOrEmpty(this.Macro) }
                .Where(x => x)
                .Count();
            if (count != 1)
            {
                throw new CompilerException(this, "One and only one of the attributes 'variable' and 'macro' is allowed.");
            }

            // add context
            using (var method = code.AppendMethodInvoke(null, this))
            {
                // init
                method.AddElement(this);

                // by type
                if (!string.IsNullOrEmpty(this.Variable))
                {
                    method.Name = "this.RenderKeyByVariable";
                    method.AddLiteral(this.Variable);
                }
                else if (!string.IsNullOrEmpty(this.Macro))
                {
                    method.Name = "this.RenderKeyByMacro";
                    method.AddCode("this.{0}", code.Root.GetMacro(this.Macro));
                }
                else
                {
                    throw new NotSupportedException();
                }

                // context parameters
                method.AddContextAndParameters();
            }
        }
    }
}