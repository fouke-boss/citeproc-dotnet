using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CiteProc
{
    /// <summary>
    /// Base class for all xml elements.
    /// </summary>
    public abstract class Element
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        protected Element()
        {
            // init
            if (File.Current != null)
            {
                this.Tag = string.Format("({0}, {1})", File.Current.LineNumber, File.Current.LinePosition);
            }
        }

        /// <summary>
        /// Gets or sets additional data for the element. When the elemnt was loaded using an XmlReader,
        /// the tag is filled with the (line, position) of the element in the xml.
        /// </summary>
        [XmlIgnore]
        public string Tag
        {
            get;
            private set;
        }

        //public Element[] Elements
        //{
        //    get
        //    {
        //        return this.GetElements()
        //            .Where(x => x != null)
        //            .ToArray();
        //    }
        //}
        ///// <summary>
        ///// Returns all child elements.
        ///// </summary>
        ///// <returns></returns>
        //protected abstract IEnumerable<Element> GetElements();
    }
}
