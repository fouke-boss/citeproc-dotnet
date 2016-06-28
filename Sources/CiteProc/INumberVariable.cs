using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc
{
    public interface INumberVariable
    {
        uint Min
        {
            get;
        }
        uint Max
        {
            get;
        }
        char Separator
        {
            get;
        }
    }
}
