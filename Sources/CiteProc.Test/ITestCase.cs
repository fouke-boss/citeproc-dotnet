using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Test
{
    public interface ITestCase
    {
        string Name
        {
            get;
        }
        bool Execute(StreamWriter log);
    }
}
