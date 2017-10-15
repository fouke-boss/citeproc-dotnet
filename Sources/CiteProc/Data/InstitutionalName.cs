using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Data
{
    public class InstitutionalName : Name, IInstitutionalName
    {
        public InstitutionalName(string name)
        {
            this.Name = name;
        }

        public string Name
        {
            get;
            private set;
        }

        public override bool IsEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.Name);
            }
        }

        public override string ToString()
        {
            // done
            return this.Name;
        }
    }
}
