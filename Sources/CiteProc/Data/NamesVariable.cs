using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Data
{
    public class NamesVariable : INamesVariable
    {
        private List<Name> _Items;

        public NamesVariable(IEnumerable<Name> names)
        {
            // init
            this._Items = names.ToList();
        }

        public int Count
        {
            get
            {
                return this._Items.Count;
            }
        }
        public Name this[int index]
        {
            get
            {
                return this._Items[index];
            }
        }

        public IEnumerator<Name> GetEnumerator()
        {
            return this._Items.GetEnumerator();
        }
        IEnumerator<IName> IEnumerable<IName>.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(", ", this.Select(x => x.ToString()));
        }
    }
}
