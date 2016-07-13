using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc
{
    public interface ITextVariable : IVariable
    {
        string Value
        {
            get;
        }
    }
}
