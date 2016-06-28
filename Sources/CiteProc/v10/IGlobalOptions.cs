using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.v10
{
    /// <summary>
    /// Global options, which affect both citations and the bibliography, are set on the cs:style element.
    /// </summary>
    public interface IGlobalOptions
    {
        /// <summary>
        /// Specifies whether compound given names (e.g. “Jean-Luc”) should be initialized with a hyphen 
        /// (“J.-L.”, value “true”, default) or without (“J.L.”, value “false”).
        /// </summary>
        bool InitializeWithHyphen
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'initialize-with-hyphen' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool InitializeWithHyphenSpecified
        {
            get;
        }

        /// <summary>
        /// Activates expansion or collapsing of page ranges. If the attribute is not set, page ranges are rendered without reformatting.
        /// </summary>
        PageRangeFormats PageRangeFormat
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'page-range-format' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool PageRangeFormatSpecified
        {
            get;
        }

        /// <summary>
        /// Sets the display and sorting behavior of the non-dropping-particle in inverted names (e.g. “Koning, W. de”).
        /// </summary>
        DemotingBehavior DemoteNonDroppingParticle
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'demote-non-dropping-particle' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool DemoteNonDroppingParticleSpecified
        {
            get;
        }
    }
}
