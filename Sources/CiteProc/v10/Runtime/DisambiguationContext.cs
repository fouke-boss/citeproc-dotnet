using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.v10.Runtime
{
    partial class Processor
    {
        protected internal class DisambiguationContext
        {
#warning Remove!
            public static readonly DisambiguationContext Default = new DisambiguationContext();

            public int MinAddNames
            {
                get;
                set;
            }
            public DisambiguateAddGivenNameLevel AddGivenNameLevel
            {
                get;
                set;
            }
        }
    }
}