using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Test.CodeTesting
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TestCaseAttribute : System.Attribute
    {
        public TestCaseAttribute()
        {
            this.Name = null;
        }
        public TestCaseAttribute(string name)
        {
            this.Name = name;
        }

        public string Name
        {
            get;
            private set;
        }
    }
}
