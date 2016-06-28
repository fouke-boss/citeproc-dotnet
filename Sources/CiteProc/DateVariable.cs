using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteProc
{
    public struct DateVariable : IDateVariable
    {
        private int _YearFrom;
        private int _YearTo;
        private Season? _SeasonFrom;
        private Season? _SeasonTo;
        private int? _MonthFrom;
        private int? _MonthTo;
        private int? _DayFrom;
        private int? _DayTo;
        private bool _IsCirca;

        public DateVariable(DateTime date)
            : this(date, date, false)
        {
        }
        public DateVariable(DateTime from, DateTime to)
            : this(from, to, false)
        {
        }
        public DateVariable(DateTime from, DateTime to, bool isCirca)
        {
            // done
            this._YearFrom = from.Year;
            this._YearTo = to.Year;
            this._SeasonFrom = GetSeason(from);
            this._SeasonTo = GetSeason(to);
            this._MonthFrom = from.Month;
            this._MonthTo = to.Month;
            this._DayFrom = from.Day;
            this._DayTo = to.Day;
            this._IsCirca = isCirca;
        }
        public DateVariable(int year, Season? season, int? month, int? day, bool isCirca)
            : this(year, season, month, day, year, season, month, day, isCirca)
        {
        }
        public DateVariable(int yearFrom, Season? seasonFrom, int? monthFrom, int? dayFrom, int yearTo, Season? seasonTo, int? monthTo, int? dayTo, bool isCirca)
        {
            // done
            this._YearFrom = yearFrom;
            this._YearTo = yearTo;
            this._SeasonFrom = seasonFrom;
            this._SeasonTo = seasonTo;
            this._MonthFrom = monthFrom;
            this._MonthTo = monthTo;
            this._DayFrom = dayFrom;
            this._DayTo = dayTo;
            this._IsCirca = isCirca;
        }
        private static Season GetSeason(DateTime date)
        {
            if (date < new DateTime(date.Year, 3, 21))
            {
                return Season.Winter;
            }
            else if (date < new DateTime(date.Year, 6, 21))
            {
                return Season.Spring;
            }
            else if (date < new DateTime(date.Year, 9, 21))
            {
                return Season.Summer;
            }
            else if (date < new DateTime(date.Year, 12, 21))
            {
                return Season.Autumn;
            }
            else
            {
                return Season.Winter;
            }
        }

        public int YearFrom
        {
            get
            {
                return this._YearFrom;
            }
        }
        public int YearTo
        {
            get
            {
                return this._YearTo;
            }
        }

        public Season? SeasonFrom
        {
            get
            {
                return this._SeasonFrom;
            }
        }
        public Season? SeasonTo
        {
            get
            {
                return this._SeasonTo;
            }
        }

        public int? MonthFrom
        {
            get
            {
                return this._MonthFrom;
            }
        }
        public int? MonthTo
        {
            get
            {
                return this._MonthTo;
            }
        }

        public int? DayFrom
        {
            get
            {
                return this._DayFrom;
            }
        }
        public int? DayTo
        {
            get
            {
                return this._DayTo;
            }
        }

        public bool IsApproximate
        {
            get
            {
                return this._IsCirca;
            }
        }
    }
}