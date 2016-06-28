using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Test
{
    public interface ITest
    {
        string Name
        {
            get;
        }
        ITestCase[] Cases
        {
            get;
        }
    }
}
