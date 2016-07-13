using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Data.IO
{
    internal class CharReader : IDisposable, ILineInfo
    {
        private TextReader _Reader;
        private char[] _Buffer = new char[0];
        private int _Index = 0;
        private bool _EndOfReader = false;

        public CharReader(TextReader reader)
        {
            // init
            this._Reader = reader;
            this.BufferSize = 8192;
            this.LineNumber = 1;
            this.LinePosition = 0;
        }
        public void Dispose()
        {
            // dispose
            this._Reader = null;
        }

        public int BufferSize
        {
            get;
            private set;
        }

        private void Ensure(int count)
        {
            // init
            var remaining = (this._Buffer.Length - this._Index);

            // read?
            if (remaining < count && !this._EndOfReader)
            {
                // init
                var buffer = new char[Math.Max(count - remaining, this.BufferSize)];

                // read
                var count2 = this._Reader.ReadBlock(buffer, 0, buffer.Length);
                this._EndOfReader = (count2 < buffer.Length);

                // done
                this._Buffer = this._Buffer.Skip(this._Index).Concat(buffer.Take(count2)).ToArray();
                this._Index = 0;
            }
        }

        public char Current
        {
            get
            {
                // init
                this.Ensure(1);

                // done
                return this._Buffer[this._Index];
            }
        }
        public string Peek(int length)
        {
            // init
            this.Ensure(length);

            // done
            return new string(this._Buffer.Skip(this._Index).Take(length).ToArray());
        }

        public char Pop()
        {
            // init
            this.Ensure(1);

            // done
            var result = this._Buffer[this._Index];

            // line number and position
            if (result == '\n')
            {
                this.LineNumber++;
                this.LinePosition = 1;
            }
            else if (result == '\t')
            {
                this.LinePosition = (int)(4d * Math.Floor((double)(this.LinePosition + 3) / 4d)) + 1;
            }
            else
            {
                this.LinePosition++;
            }

            // next
            this._Index++;

            // done
            return result;
        }
        public string Pop(Predicate<char> predicate)
        {
            // init
            var result = new StringBuilder(65536);

            // loop
            while (!this.Eof && predicate(this.Current))
            {
                // add
                result.Append(this.Pop());
            }

            // done
            return result.ToString();
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

        public bool Eof
        {
            get
            {
                return (this._EndOfReader && this._Index >= this._Buffer.Length);
            }
        }
        public override string ToString()
        {
            return string.Format("'{0}' at ({1}, {2})", this.Current, this.LineNumber, this.LinePosition);
        }
    }
}
