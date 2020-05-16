using PxWorkLog.Core.Utils;
using System;
using System.Collections.ObjectModel;

namespace PxWorkLog.Core
{
    public class Issue
    {
        public string Name { get; set; }
        public ObservableCollection<LoggedQuarter> LoggedQuarters { get; } = new ObservableCollection<LoggedQuarter>();
        public ProjectedObservableValue<TimeSpan> TotalLoggedTime { get; }

        public Issue(string name)
        {
            Name = name;

            TotalLoggedTime = new ProjectedObservableValue<TimeSpan>(() => LoggedQuarter.Duration * LoggedQuarters.Count);
            LoggedQuarters.CollectionChanged += TotalLoggedTime.Refresh;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
