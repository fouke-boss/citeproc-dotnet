using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CiteProc.Formatting;

namespace CiteProc.v10.Runtime
{
    partial class Processor
    {
#warning Naar CiteProc.Formatting namespace? in dat geval aanvullen met IDataProvider?
        protected class Entry
        {
            public Entry(ComposedRun layout, string[] sort)
            {
                // init
                this.Layout = layout;
                this.Sort = sort;
            }

            public ComposedRun Layout
            {
                get;
                private set;
            }
            public string[] Sort
            {
                get;
                private set;
            }
        }
    }
}
