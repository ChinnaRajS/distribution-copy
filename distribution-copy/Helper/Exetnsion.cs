using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace distribution_copy.Helper
{
    public static class Exetnsion
    {
        public static string DateDifference(this string firstDate, string secondDate)
        {
            if (!string.IsNullOrEmpty(firstDate) && !string.IsNullOrEmpty(secondDate))
            {
                return (Convert.ToDateTime(firstDate).Date - Convert.ToDateTime(secondDate).Date).Days.ToString();
            }
            return "";
        }

        public static string WorkingDaysDifference(this string firstDate, string secondDate)
        {
            if (!string.IsNullOrEmpty(firstDate) && !string.IsNullOrEmpty(secondDate))
            {
                return (Convert.ToDateTime(firstDate).Date - Convert.ToDateTime(secondDate).Date).Days.ToString();
            }
            return "";
        }

        public static string GetBusinessDays(this string firstDate, string secondDate)
        {
            DateTime startD = Convert.ToDateTime(firstDate);
            DateTime endD = Convert.ToDateTime(secondDate);
            double calcBusinessDays =
                1 + ((endD - startD).TotalDays * 5 -
                (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;

            if (endD.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
            if (startD.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

            return calcBusinessDays.ToString();
        }
    }
}