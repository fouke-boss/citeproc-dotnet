using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CiteProc.Compilation;

namespace CiteProc.v10
{
    public class NameElement : FormattingElement, INameOptions
    {
        [XmlElement("name-part")]
        public NamePartElement[] NameParts
        {
            get;
            set;
        }

        [XmlAttribute("and")]
        public And And
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool AndSpecified
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

        [XmlAttribute("delimiter-precedes-et-al")]
        public DelimiterBehavior DelimiterPrecedesEtAl
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool DelimiterPrecedesEtAlSpecified
        {
            get;
            set;
        }

        [XmlAttribute("delimiter-precedes-last")]
        public DelimiterBehavior DelimiterPrecedesLast
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool DelimiterPrecedesLastSpecified
        {
            get;
            set;
        }

        [XmlAttribute("et-al-min")]
        public int EtAlMin
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool EtAlMinSpecified
        {
            get;
            set;
        }

        [XmlAttribute("et-al-use-first")]
        public int EtAlUseFirst
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool EtAlUseFirstSpecified
        {
            get;
            set;
        }

        [XmlAttribute("et-al-subsequent-min")]
        public int EtAlSubsequentMin
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool EtAlSubsequentMinSpecified
        {
            get;
            set;
        }

        [XmlAttribute("et-al-subsequent-use-first")]
        public int EtAlSubsequentUseFirst
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool EtAlSubsequentUseFirstSpecified
        {
            get;
            set;
        }

        [XmlAttribute("et-al-use-last")]
        public bool EtAlUseLast
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool EtAlUseLastSpecified
        {
            get;
            set;
        }

        [XmlAttribute("form")]
        public NameFormat Format
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool FormatSpecified
        {
            get;
            set;
        }

        [XmlAttribute("initialize")]
        public bool Initialize
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool InitializeSpecified
        {
            get;
            set;
        }

        [XmlAttribute("initialize-with")]
        public string InitializeWith
        {
            get;
            set;
        }

        [XmlAttribute("name-as-sort-order")]
        public NameSortOptions NameAsSortOrder
        {
            get;
            set;
        }
        [XmlIgnore]
        public bool NameAsSortOrderSpecified
        {
            get;
            set;
        }

        [XmlAttribute("sort-separator")]
        public string SortSeparator
        {
            get;
            set;
        }

        internal void Compile(MethodInvoke method)
        {
            // init
            method.AddElement(this);
            method.AddCode(method.ParameterName);
            method.AddLiteral(this.Prefix);
            method.AddLiteral(this.Suffix);

            // name part parameters
            using (var lambda = method.AddLambdaExpression(true, "new NamePartParameters[]"))
            {
                // fix
                var family = (this.NameParts == null || !this.NameParts.Any(x => x.Name == NamePartName.Family) ? new NamePartElement() : this.NameParts.Single(x => x.Name == NamePartName.Family));
                var given = (this.NameParts == null || !this.NameParts.Any(x => x.Name == NamePartName.Given) ? new NamePartElement() : this.NameParts.Single(x => x.Name == NamePartName.Given));

                // array
                lambda.AppendArray(new NamePartElement[] { family, given }, part =>
                {
                    // init
                    using (var m = lambda.AppendMethodInvoke("new NamePartParameters", part))
                    {
                        part.Compile(m);
                    }
                });
            }

            // default parameters
            method.AddDefaultParameters();
        }
    }
}