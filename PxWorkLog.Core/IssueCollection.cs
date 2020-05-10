using PxWorkLog.Core.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PxWorkLog.Core
{
    public class IssueCollection : ObservableCollection<Issue>
    {
        public ProjectedObservableValue<TimeSpan> TotalActiveTime { get; }

        internal IssueCollection()
        {
            TotalActiveTime = new ProjectedObservableValue<TimeSpan>(() => this
                .Select(issue => issue.TotalLoggedTime)
                .Aggregate(TimeSpan.Zero, (x, y) => x + y));
            CollectionChanged += TotalActiveTime.Refresh;
            var aggregator = new CollectionEventAggregator();
            aggregator.Initialize(this,
                issue => issue.LoggedQuarters.CollectionChanged += aggregator.RaiseItemChanged,
                issue => issue.LoggedQuarters.CollectionChanged -= aggregator.RaiseItemChanged);
            aggregator.ItemChanged += TotalActiveTime.Refresh;

            Add(new Issue("Coordination"));
            Add(new Issue("Ceremonies"));
        }

        public Issue FindLoggedIn(LoggedQuarter quarter)
        {
            return this.SingleOrDefault(issue => issue.LoggedQuarters.Contains(quarter));
        }
    }
}
