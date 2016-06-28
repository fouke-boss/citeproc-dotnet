using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// Represents the personal details of a author, contributor or translater of a style.
    /// </summary>
    public class PersonalDetailsElement : Element
    {
        /// <summary>
        /// The name of the author, contributor or translator.
        /// </summary>
        [XmlElement("name")]
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Website of the author, contributor or translator.
        /// </summary>
        [XmlElement("uri")]
        public string Uri
        {
            get;
            set;
        }
        /// <summary>
        /// Email address of the author, contributor or translator.
        /// </summary>
        [XmlElement("email")]
        public string Email
        {
            get;
            set;
        }
    }
}
