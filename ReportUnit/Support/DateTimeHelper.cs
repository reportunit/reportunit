namespace ReportUnit.Support
{
    using System;

    internal class DateTimeHelper
    {
        /// <summary>
        /// Work out the difference between two date time strings in milliseconds
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static double DifferenceInMilliseconds(string startTime, string endTime)
        {
            DateTime s, e;

            if (DateTime.TryParse(startTime, out s) && DateTime.TryParse(endTime, out e))
            {
                return (e - s).TotalMilliseconds;
            }

            return 0;
        }
    }
}
