using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Compilation
{
    internal sealed class CodeWriter
    {
        private int _Indent = 0;
        private StringBuilder _Result = new StringBuilder(2 * 65536);

        public void Append(string code)
        {
            this._Result.Append(code);
        }
        public void AppendIndent()
        {
#if(DEBUG)
            this.Append("".PadLeft(4 * this._Indent));
#endif
        }

        public void IncreaseIndent()
        {
            this._Indent++;
        }
        public void DecreaseIndent()
        {
            this._Indent--;
        }

        public override string ToString()
        {
            return this._Result.ToString();
        }
    }
}
