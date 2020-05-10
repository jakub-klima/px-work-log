using System;
using System.Diagnostics;

namespace PxWorkLog.Core
{
    public class LoggedQuarter
    {
        internal static readonly TimeSpan Duration = TimeSpan.FromMinutes(15);
        public int Hour { get; }
        public int Quarter { get; }

        internal static LoggedQuarter FromNow()
        {
            DateTime now = DateTime.Now;
            return new LoggedQuarter(now.Hour, quarter: now.Minute / 15);
        }

        public LoggedQuarter(int hour, int quarter)
        {
            Debug.Assert(0 <= hour && hour <= 23);
            Debug.Assert(0 <= quarter && quarter <= 3);

            Hour = hour;
            Quarter = quarter;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LoggedQuarter);
        }

        public bool Equals(LoggedQuarter other)
        {
            return
                other != null &&
                Hour == other.Hour &&
                Quarter == other.Quarter;
        }

        public override int GetHashCode()
        {
            return Hour ^ Quarter;
        }
    }
}
