using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc.Data
{
    public class PersonalName : Name, IPersonalName
    {
        public PersonalName(string familyName, string givenNames)
            : this(familyName, givenNames, null, false, null, null)
        {
        }
        public PersonalName(string familyName, string givenNames, string suffix, bool precedeSuffixByComma, string droppingParticles, string nonDroppingParticles)
        {
            this.FamilyName = familyName;
            this.GivenNames = givenNames;
            this.Suffix = suffix;
            this.PrecedeSuffixByComma = precedeSuffixByComma;
            this.DroppingParticles = droppingParticles;
            this.NonDroppingParticles = nonDroppingParticles;
        }

        public string FamilyName
        {
            get;
            private set;
        }
        public string GivenNames
        {
            get;
            private set;
        }

        public string Suffix
        {
            get;
            private set;
        }
        public bool PrecedeSuffixByComma
        {
            get;
            private set;
        }

        public string DroppingParticles
        {
            get;
            private set;
        }
        public string NonDroppingParticles
        {
            get;
            private set;
        }

        public override bool IsEmpty
        {
            get
            {
                return (string.IsNullOrWhiteSpace(this.FamilyName) && string.IsNullOrWhiteSpace(this.GivenNames) && string.IsNullOrWhiteSpace(this.NonDroppingParticles) && string.IsNullOrWhiteSpace(this.DroppingParticles) && string.IsNullOrWhiteSpace(this.Suffix));
            }
        }

        public override string ToString()
        {
            // init
            var parts = new string[] { this.GivenNames, this.DroppingParticles, this.NonDroppingParticles, this.FamilyName, this.Suffix };

            // done
            return string.Join(" ", parts.Where(x => !string.IsNullOrEmpty(x)));
        }
    }
}
