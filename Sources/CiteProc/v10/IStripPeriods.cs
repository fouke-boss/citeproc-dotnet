using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.v10
{
    /// <summary>
    /// Represents an element with a strip-periods attribute.
    /// </summary>
    public interface IStripPeriods
    {
        /// <summary>
        /// When set to “true” (“false” is the default), any periods in the rendered text are removed.
        /// </summary>
        bool StripPeriods
        {
            get;
        }
        /// <summary>
        /// Indicates whether the 'strip-periods' attribute is specified. Required by System.Xml.XmlSerializer.
        /// </summary>
        bool StripPeriodsSpecified
        {
            get;
        }
    }
}
