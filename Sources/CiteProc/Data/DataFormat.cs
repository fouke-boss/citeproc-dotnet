using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Data
{
    public abstract class DataFormat
    {
        public static readonly DataFormat Json = new JsonDataFormat();

        protected internal abstract IEnumerable<DataProvider> Parse(TextReader reader);
    }
}
