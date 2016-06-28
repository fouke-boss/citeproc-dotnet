using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Test.CodeTesting
{
    public abstract class Test : ITest
    {
        protected Test(string name)
        {
            // init
            this.Name = name;

            // create a list of cases
            this.Cases = this.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.IsDefined(typeof(TestCaseAttribute)))
                .Select(x => new TestCase(this, x))
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

        protected void Assert(bool condition)
        {
            if (!condition)
            {
                throw new AssertionException();
            }
        }

        protected T LoadEmbeddedResource<T>(string name)
            where T : File
        {
            // open stream
            var tmp = typeof(Test).Assembly.GetManifestResourceNames().OrderBy(x => x).ToArray();
            using (var stream = typeof(Test).Assembly.GetManifestResourceStream(string.Format("CiteProc.Test.CodeTesting.Repository.{0}", name)))
            {
                // load
                return File.Load<T>(stream);
            }
        }
    }
}
