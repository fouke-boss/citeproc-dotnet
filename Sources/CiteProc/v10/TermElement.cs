using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// A term represents a localized text string.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Name}, {Format}")]
    public class TermElement : Element
    {
        /// <summary>
        /// The name of the term.
        /// </summary>
        [XmlAttribute("name")]
        public TermName Name
        {
            get;
            set;
        }
        /// <summary>
        /// Terms may be defined for specific forms by using cs:term with the optional form attribute.
        /// </summary>
        [XmlAttribute("form")]
        public TermFormat Format
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'form' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool FormatSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the way terms are matched to ordinals.
        /// </summary>
        [XmlAttribute("match")]
        public TermMatch Match
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'match' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool MatchSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the requested gender of a ordinal rendered for this term.
        /// </summary>
        [XmlAttribute("gender")]
        public Gender Gender
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'gender' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool GenderSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the gender of the ordinal term (e.g. 1re is feminine, while 1er is masculin).
        /// </summary>
        [XmlAttribute("gender-form")]
        public Gender GenderForm
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'gender-form' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool GenderFormSpecified
        {
            get;
            set;
        }

        /// <summary>
        /// The translation for the term.
        /// </summary>
        [XmlText]
        public string Value
        {
            get;
            set;
        }
        /// <summary>
        /// The translated term for a single item.
        /// </summary>
        [XmlElement("single")]
        public string Single
        {
            get;
            set;
        }
        /// <summary>
        /// The translated term for multiple items.
        /// </summary>
        [XmlElement("multiple")]
        public string Multiple
        {
            get;
            set;
        }
    }
}
