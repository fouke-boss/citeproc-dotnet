using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// The cs:info element may be used to give metadata on the locale file.
    /// </summary>
    public class LocaleInfoElement : Element
    {
        /// <summary>
        /// Used to acknowledge locale translators.
        /// </summary>
        [XmlElement("translator")]
        public PersonalDetailsElement[] Translators
        {
            get;
            set;
        }

        /// <summary>
        /// The contents of cs:rights specifies the license under which the locale file is released.
        /// </summary>
        [XmlElement("rights")]
        public RightsElement Rights
        {
            get;
            set;
        }

        /// <summary>
        /// The contents of cs:updated must be a timestamp that shows when the locale file was last updated.
        /// </summary>
        [XmlElement("updated")]
        public DateTime Updated
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'updated' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool UpdatedSpecified
        {
            get;
            set;
        }
    }
}