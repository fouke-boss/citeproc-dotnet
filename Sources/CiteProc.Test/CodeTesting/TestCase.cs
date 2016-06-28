using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Test.CodeTesting
{
    public class TestCase : ITestCase
    {
        private Test _Owner;
        private MethodInfo _Method;

        public TestCase(Test owner, MethodInfo method)
        {
            // init
            this._Owner = owner;
            this._Method = method;

            // attribute
            var att = method.GetCustomAttributes(true)
                .OfType<TestCaseAttribute>()
                .Single();

            // done
            this.Name = (string.IsNullOrEmpty(att.Name) ? method.Name : att.Name);
        }

        public string Name
        {
            get;
            private set;
        }

        public bool Execute(System.IO.StreamWriter log)
        {
            // init
            var result = false;

            // try/catch
            try
            {
                // invoke
                this._Method.Invoke(this._Owner, null);

                // done
                result = true;
            }
            catch
            {
                result = false;
            }

            // dome
            return result;
        }
    }
}
