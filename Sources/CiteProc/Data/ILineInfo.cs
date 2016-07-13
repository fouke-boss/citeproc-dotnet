using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Data
{
    internal interface ILineInfo
    {
        int LineNumber
        {
            get;
        }
        int LinePosition
        {
            get;
        }
    }
}
