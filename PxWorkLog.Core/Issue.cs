using System;
using System.Collections.ObjectModel;

namespace PxWorkLog.Core
{
    public class Issue
    {
        public string Name { get; set; }
        public ObservableCollection<LoggedQuarter> LoggedQuarters { get; } = new ObservableCollection<LoggedQuarter>();
        public TimeSpan TotalLoggedTime => LoggedQuarter.Duration * LoggedQuarters.Count;

        public Issue(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
