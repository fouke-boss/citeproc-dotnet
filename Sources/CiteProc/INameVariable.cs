using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc
{
    public interface INameVariable
    {
        /// <summary>
        /// Surname minus any particles and suffixes.
        /// </summary>
        string FamilyName
        {
            get;
        }
        /// <summary>
        /// Given names, either full (“John Edward”) or initialized (“J. E.”)
        /// </summary>
        string GivenNames
        {
            get;
        }

        /// <summary>
        /// Name suffix, e.g. “Jr.” in “John Smith Jr.” and “III” in “Bill Gates III”.
        /// </summary>
        string Suffix
        {
            get;
        }
        bool PrecedeSuffixByComma
        {
            get;
        }

        /// <summary>
        /// Name particles that are not dropped when only the surname is shown (“de” in the Dutch surname “de Koning”)
        /// but which may be treated separately from the family name, e.g. for sorting.
        /// </summary>
        string NonDroppingParticles
        {
            get;
        }
        /// <summary>
        /// Name particles that are dropped when only the surname is shown (“van” in “Ludwig van Beethoven”, which becomes “Beethoven”).
        /// </summary>
        string DroppingParticles
        {
            get;
        }
    }
}