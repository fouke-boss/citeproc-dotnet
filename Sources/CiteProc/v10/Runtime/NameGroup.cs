using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.v10.Runtime
{
    partial class Processor
    {
        protected internal class NameGroup
        {
            public NameGroup(string variable, TermName? term, IEnumerable<IName> names)
            {
                // init
                this.Variable = variable;
                this.Term = term;
                this.Names = (names == null ? null : names.ToArray());
            }

            public string Variable
            {
                get;
                private set;
            }
            public TermName? Term
            {
                get;
                private set;
            }
            public IName[] Names
            {
                get;
                private set;
            }

            public static bool AreNamesEqual(IName name1, IName name2)
            {
                // type?
                if (name1 is IPersonalName && name2 is IPersonalName)
                {
                    // cast
                    var pname1 = (IPersonalName)name1;
                    var pname2 = (IPersonalName)name2;

                    // done
                    return (string.Compare(pname1.FamilyName, pname2.FamilyName, true) == 0 &&
                        string.Compare(pname1.GivenNames, pname2.GivenNames, true) == 0 &&
                        string.Compare(pname1.DroppingParticles, pname2.DroppingParticles, true) == 0 &&
                        string.Compare(pname1.NonDroppingParticles, pname2.NonDroppingParticles, true) == 0 &&
                        string.Compare(pname1.Suffix, pname2.Suffix, true) == 0);
                }
                else if (name1 is IInstitutionalName && name2 is IInstitutionalName)
                {
                    // cast
                    var iname1 = (IInstitutionalName)name1;
                    var iname2 = (IInstitutionalName)name2;

                    // done
                    return (string.Compare(iname1.Name, iname2.Name, true) == 0);
                }
                else
                {
                    return false;
                }
            }
        }
    }
}