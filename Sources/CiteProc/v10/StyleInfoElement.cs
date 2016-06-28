using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc.v10
{
    /// <summary>
    /// The cs:info element contains the style’s metadata. Its structure is based on the Atom Syndication Format.
    /// </summary>
    public class StyleInfoElement : Element
    {
        /// <summary>
        /// The element should contain a URI to establish the identity of the style. A stable, unique
        /// and dereferenceable URI is desired for publicly available styles.
        /// </summary>
        [XmlElement("id")]
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// The contents of cs:title should be the name of the style as shown to users.
        /// </summary>
        [XmlElement("title")]
        public InfoTextElement Title
        {
            get;
            set;
        }
        /// <summary>
        /// The contents of cs:title-short should be a shortened style name (e.g. “APA”).
        /// </summary>
        [XmlElement("title-short")]
        public InfoTextElement TitleShort
        {
            get;
            set;
        }
        /// <summary>
        /// The contents of cs:summary gives a (short) description of the style.
        /// </summary>
        [XmlElement("summary")]
        public InfoTextElement Summary
        {
            get;
            set;
        }

        /// <summary>
        /// Acknowledges the author(s) of the style.
        /// </summary>
        [XmlElement("author")]
        public PersonalDetailsElement[] Authors
        {
            get;
            set;
        }
        /// <summary>
        /// Acknowledges contributor(s) to the style.
        /// </summary>
            [XmlElement("contributor")]
        public PersonalDetailsElement[] Contributors
        {
            get;
            set;
        }

        /// <summary>
        /// Styles may be assigned one or more categories. A single categorie may be used to describe how in-text citations are
        /// rendered using the citation-format attribute. Multiple categories may be used with the field attribute, set to one of
        /// the discipline categories, to indicate the field(s) for which the style is relevant.
        /// </summary>
        [XmlElement("category")]
        public CategoryElement[] Categories
        {
            get;
            set;
        }
        /// <summary>
        /// May be used multiple times. cs:link must carry two attributes: href, set to a URI (usually a URL), and
        /// rel, whose value indicates how the URI relates to the style.
        /// </summary>
        [XmlElement("link")]
        public LinkElement[] Links
        {
            get;
            set;
        }
        /// <summary>
        /// The contents of cs:rights specifies the license under which the style file is released. The element 
        /// may carry a license attribute to specify the URI of the license.
        /// </summary>
        [XmlElement("rights")]
        public RightsElement Rights
        {
            get;
            set;
        }

        /// <summary>
        /// The cs:issn element may be used multiple times to indicate the ISSN identifier(s) of the journal for which the style was written.
        /// </summary>
        [XmlElement("issn")]
        public string[] ISSN
        {
            get;
            set;
        }
        /// <summary>
        /// The cs:eissn element may be used once for the eISSN identifier.
        /// </summary>
        [XmlElement("eissn")]
        public string eISSN
        {
            get;
            set;
        }
        /// <summary>
        /// The cs:issnl elements may be used once for the ISSN-L identifier.
        /// </summary>
        [XmlElement("issnl")]
        public string ISSNL
        {
            get;
            set;
        }

        /// <summary>
        /// The contents of cs:published must be a timestamp, indicating when the style was initially created or made available.
        /// </summary>
        [XmlElement("published")]
        public DateTime Published
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'published' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool PublishedSpecified
        {
            get;
            set;
        }
        /// <summary>
        /// The contents of cs:updated must be a timestamp that shows when the style was last updated.
        /// </summary>
        [XmlElement("updated")]
        public DateTime Updated
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether the 'updates' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public bool UpdatedSpecified
        {
            get;
            set;
        }
    }
}
