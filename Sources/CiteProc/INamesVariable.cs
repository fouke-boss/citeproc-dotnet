using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc
{
    public interface INamesVariable : IVariable, IEnumerable<IName>
    {
    }
}
