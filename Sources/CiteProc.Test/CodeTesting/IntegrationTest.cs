using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Test.CodeTesting
{
    public class IntegrationTest : Test
    {
        public IntegrationTest()
            : base("Integration Test")
        {
        }

        [TestCase]
        public void CompileOfDependentStyle()
        {
            // load files
            var file1 = this.LoadEmbeddedResource<v10.StyleFile>("annals-of-mathematics-and-artificial-intelligence.csl");
            var file2 = this.LoadEmbeddedResource<v10.StyleFile>("springer-mathphys-brackets.csl");

            // compile
            var processor = Processor.Compile(file1, file2);

            // check name
            this.Assert(processor.Title == file1.Info.Title.Text);
        }
    }
}
