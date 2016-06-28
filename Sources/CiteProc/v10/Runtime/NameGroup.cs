using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.v10.Runtime
{
    partial class Processor
    {
        protected class NameGroup
        {
            public NameGroup(string variable, TermName? term, object[] names)
            {
                // init
                this.Variable = variable;
                this.Term = term;
                this.Names = names;
            }

            public string Variable
            {
                get;
                private set;
            }
            public TermName? Term
            {
                get;
                private set;
            }
            public object[] Names
            {
                get;
                private set;
            }
        }
    }
}