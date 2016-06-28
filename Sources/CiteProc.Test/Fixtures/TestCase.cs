using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CiteProc.Compilation;

namespace CiteProc.Test.Fixtures
{
    public class TestCase : ITestCase
    {
        internal TestCase(string name, string resourceName, bool isDebug)
        {
            // init
            this.Name = (name.Length > 60 ? name.Substring(0, 60) : name);
            this.ResourceName = resourceName;
            this.IsDebug = isDebug;
        }

        public string Name
        {
            get;
            private set;
        }
        public string ResourceName
        {
            get;
            private set;
        }
        public bool IsDebug
        {
            get;
            private set;
        }

        public bool Execute(System.IO.StreamWriter log)
        {
            // init
            var output = string.Empty;
            Fixture fixture = null;

            // execute
            try
            {
                // load fixture
                using (var stream = typeof(TestCase).Assembly.GetManifestResourceStream(this.ResourceName))
                {
                    fixture = Fixture.Load(stream);
                }

                // execute
                output = fixture.Execute();
            }
            catch (FeatureNotSupportedException)
            {
                // rethrow
                throw;
            }
            catch (Exception ex)
            {
                // catch
                output = string.Format("[{0}: {1}]", ex.GetType().Name, ex.Message);
            }

            // compare
            var result = (fixture != null && output == fixture.ExpectedResult);

            // punctuation (not yet supported)
            if (!result && fixture != null && this.UnPuncuate(output) == this.UnPuncuate(fixture.ExpectedResult))
            {
                throw new FeatureNotSupportedException("punctuation");
            }

            // break?
            if (!result)
            {
                // break
                if (this.IsDebug)
                {
                    System.Diagnostics.Debugger.Break();
                }

                // log
                log.WriteLine("".PadRight(this.Name.Length + 6, '-'));
                log.WriteLine(string.Format("-- {0} --", this.Name));
                log.WriteLine("".PadRight(this.Name.Length + 6, '-'));
                log.WriteLine(fixture == null ? null : fixture.ExpectedResult);
                log.WriteLine("".PadRight(this.Name.Length + 6, '-'));
                log.WriteLine(output);
                log.WriteLine("".PadRight(this.Name.Length + 6, '-'));
                log.WriteLine();
                log.WriteLine();
                log.WriteLine();
            }

            // done
            return result;
        }
        private string UnPuncuate(string text)
        {
            return text
                .Replace(" ", "")
                .Replace(".", "")
                .Replace(",", "")
                .Replace(":", "")
                .Replace(";", "")
                .Replace("!", "")
                .Replace("?", "");
        }
    }
}
