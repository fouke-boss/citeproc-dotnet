using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc
{
    public struct NumberVariable : INumberVariable
    {
        private uint _Min;
        private uint _Max;
        private char _Separator;

        public NumberVariable(uint value)
        {
            this._Min = value;
            this._Max = value;
            this._Separator = ' ';
        }
        public NumberVariable(uint min, uint max, char separator)
        {
            this._Min = min;
            this._Max = max;
            this._Separator = separator;
        }

        public uint Min
        {
            get
            {
                return this._Min;
            }
        }
        public uint Max
        {
            get
            {
                return this._Max;
            }
        }
        public char Separator
        {
            get
            {
                return this._Separator;
            }
        }

#warning This hides a bug where in CSL JSON a raw date "2016" is parsed as a number instead of a date.
        public override string ToString()
        {
            return (this.Min == this.Max ? this.Min.ToString() : string.Format("{0}{1}{2}", this.Min, this.Separator, this.Max));
        }
    }
}