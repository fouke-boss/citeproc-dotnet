using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
    [Obsolete("Remove as soon as all features are supported.")]
    public class FeatureNotSupportedException : NotSupportedException
    {
        public FeatureNotSupportedException(string feature)
            : base(string.Format("Feature '{0}' is not (yet) supported", feature))
        {
            this.Feature = feature;
        }

        public string Feature
        {
            get;
            private set;
        }
    }
}
