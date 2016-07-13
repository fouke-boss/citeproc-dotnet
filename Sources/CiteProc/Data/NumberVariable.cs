using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Data
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

        public static NumberVariable? TryParse(string value)
        {
            // number?
            var single = TryParseNumber(value);
            if (single.HasValue)
            {
                return new NumberVariable(single.Value);
            }

            // init
            NumberVariable? result = null;

            // separator?
            if (result == null)
            {
                result = TrySplitNumber(value, ",");
            }
            if (result == null)
            {
                result = TrySplitNumber(value, "&");
            }
            if (result == null)
            {
                result = TrySplitNumber(value, "-–");
            }

            // done
            return result;
        }
        private static uint? TryParseNumber(string text)
        {
            // init
            var result = (uint?)null;

            // try/parse
            uint dummy;
            if (uint.TryParse(text, out dummy))
            {
                result = dummy;
            }

            // done
            return result;
        }
        private static NumberVariable? TrySplitNumber(string text, string separators)
        {
            // init
            NumberVariable? result = null;

            // split
            var parts = text
                .Split(separators.ToArray(), StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            // 2 parts?
            if (parts.Length == 2)
            {
                // both numbers?
                var n1 = TryParseNumber(parts[0]);
                var n2 = TryParseNumber(string.Format("{0}{1}", parts[0].Substring(0, Math.Max(parts[0].Length - parts[1].Length, 0)), parts[1]));

                // done
                if (n1.HasValue && n2.HasValue)
                {
                    result = new NumberVariable(n1.Value, n2.Value, separators.First());
                }
            }

            // done
            return result;
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

        public override string ToString()
        {
            return (this.Min == this.Max ? this.Min.ToString() : string.Format("{0}{1}{2}", this.Min, this.Separator, this.Max));
        }
    }
}