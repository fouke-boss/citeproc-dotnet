using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.v10
{
    /// <summary>
    /// Represents an element with names option attributes.
    /// </summary>
    public interface INamesOptions
    {
        /// <summary>
        /// Specifies the delimiter for names of the different name variables (e.g. the semicolon in “Doe, Smith (editors); Johnson (translator)”).
        /// </summary>
        string Delimiter
        {
            get;
        }
    }
}
