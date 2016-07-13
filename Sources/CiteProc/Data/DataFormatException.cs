using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Data
{
    public class DataFormatException : ApplicationException, ILineInfo
    {
        internal DataFormatException(ILineInfo info, string text, params object[] args)
            : base(string.Format("({1}, {2}): {0}", string.Format(text, args), info.LineNumber, info.LinePosition))
        {
            // init
            this.LineNumber = info.LineNumber;
            this.LinePosition = info.LinePosition;
        }

        public int LineNumber
        {
            get;
            private set;
        }
        public int LinePosition
        {
            get;
            private set;
        }
    }
}
