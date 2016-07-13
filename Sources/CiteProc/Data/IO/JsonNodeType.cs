using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Data.IO
{
    internal enum JsonNodeType
    {
        None,
        StartOfArray,
        EndOfArray,
        StartOfObject,
        EndOfObject,
        PropertyName,
        String,
        Boolean,
        Integer,
        Null,
    }
}
