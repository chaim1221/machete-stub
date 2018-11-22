using System;

namespace Machete.Service
{
    public static class DbFunctions
    {
        public static DateTime AddMonths(DateTime date, int interval)
        {
            return date.AddMonths(interval);
        }
    }

    public static class SqlFunctions {
        public static string StringConvert(decimal wsiDwccardnum)
        {
            return wsiDwccardnum.ToString();
        }
    }
}
