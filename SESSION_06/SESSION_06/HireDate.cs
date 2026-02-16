using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SESSION_06
{
    internal class HireDate
    {
        public int Day { get; }
        public int Month { get; }
        public int Year { get; }

        public HireDate(int day, int month, int year)
        {
            Day = day;
            Month = month;
            Year = year;
        }
        public string GetHireDate()
        {
            return $"{Day}/{Month}/{Year}";
        }
    }
}
