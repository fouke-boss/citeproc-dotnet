using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc
{
    public interface IDateVariable : IVariable
    {
        int YearFrom
        {
            get;
        }
        int YearTo
        {
            get;
        }

        Season? SeasonFrom
        {
            get;
        }
        Season? SeasonTo
        {
            get;
        }

        int? MonthFrom
        {
            get;
        }
        int? MonthTo
        {
            get;
        }

        int? DayFrom
        {
            get;
        }
        int? DayTo
        {
            get;
        }

        bool IsApproximate
        {
            get;
        }
    }
}