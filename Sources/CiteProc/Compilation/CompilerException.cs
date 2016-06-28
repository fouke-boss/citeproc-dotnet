using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
    public class CompilerException : ApplicationException
    {
        internal CompilerException(string text, params object[] args)
            : base(string.Format(text, args))
        {
        }
        internal CompilerException(Exception innerException, string text, params object[] args)
            : base(string.Format(text, args), innerException)
        {
        }
        internal CompilerException(Element element, string text, params object[] args)
            : base(string.Format("{0}: {1}", element.Tag, string.Format(text, args)))
        {
            // init
            this.Tag = element.Tag;
        }

        public string Tag
        {
            get;
            private set;
        }
    }
}
