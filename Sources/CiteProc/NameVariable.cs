using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc
{
    public struct NameVariable : INameVariable
    {
        private string _FamilyName;
        private string _GivenNames;
        private string _Suffix;
        private bool _PrecedeSuffixByComma;
        private string _DroppingParticle;
        private string _NonDroppingParticle;

        public NameVariable(string familyName, string givenNames)
        {
            this._FamilyName = familyName;
            this._GivenNames = givenNames;
            this._Suffix = null;
            this._PrecedeSuffixByComma = false;
            this._DroppingParticle = null;
            this._NonDroppingParticle = null;
        }
        public NameVariable(string familyName, string givenNames, string suffix, bool precedeSuffixByComma, string droppingParticle, string nonDroppingParticle)
        {
            this._FamilyName = familyName;
            this._GivenNames = givenNames;
            this._Suffix = suffix;
            this._PrecedeSuffixByComma = precedeSuffixByComma;
            this._DroppingParticle = droppingParticle;
            this._NonDroppingParticle = nonDroppingParticle;
        }

        public string FamilyName
        {
            get
            {
                return this._FamilyName;
            }
        }
        public string GivenNames
        {
            get
            {
                return this._GivenNames;
            }
        }

        public string Suffix
        {
            get
            {
                return this._Suffix;
            }
        }
        public bool PrecedeSuffixByComma
        {
            get
            {
                return this._PrecedeSuffixByComma;
            }
        }

        public string DroppingParticles
        {
            get
            {
                return this._DroppingParticle;
            }
        }
        public string NonDroppingParticles
        {
            get
            {
                return this._NonDroppingParticle;
            }
        }

        public override string ToString()
        {
            // init
            var parts = new string[] { this._GivenNames, this._DroppingParticle, this._NonDroppingParticle, this._FamilyName, this._Suffix };

            // done
            return string.Join(" ", parts.Where(x => !string.IsNullOrEmpty(x)));
        }
    }
}
