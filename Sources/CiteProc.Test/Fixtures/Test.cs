using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Test.Fixtures
{
    public class Test : ITest
    {
        public Test(string name, string @namespace)
        {
            // init
            this.Name = name;
            var isDebug = (string.Compare(name, "debugging", true) == 0);

            // load cases
            var assembly = typeof(Test).Assembly;
            var ns = string.Format("{0}.{1}.", typeof(Test).Namespace, @namespace.Trim('.'));
            this.Cases = assembly
                .GetManifestResourceNames()
                .Where(x => x.StartsWith(ns))
                .Select(x => new TestCase(x.Substring(ns.Length, x.Length - ns.Length - 4), x, isDebug))
                .OrderBy(x => x.Name)
                .ToArray();
        }

        public string Name
        {
            get;
            private set;
        }
        public ITestCase[] Cases
        {
            get;
            private set;
        }
    }
}