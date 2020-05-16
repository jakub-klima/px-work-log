using PxWorkLog.Core.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PxWorkLog.Core
{
    public class IssueCollection : ObservableCollection<Issue>
    {
        public ProjectedObservableValue<TimeSpan> TotalActiveTime { get; }
        public event EventHandler LoggedQuartersChanged;

        internal IssueCollection()
        {
            var aggregator = new CollectionEventAggregator();
            aggregator.Initialize(this,
                issue => issue.LoggedQuarters.CollectionChanged += aggregator.RaiseItemChanged,
                issue => issue.LoggedQuarters.CollectionChanged -= aggregator.RaiseItemChanged);
            aggregator.ItemChanged += (s, e) => LoggedQuartersChanged?.Invoke(this, EventArgs.Empty);
            CollectionChanged += (s, e) => LoggedQuartersChanged?.Invoke(this, EventArgs.Empty);

            TotalActiveTime = new ProjectedObservableValue<TimeSpan>(() => this.Aggregate(TimeSpan.Zero, (acc, issue) => acc + issue.TotalLoggedTime.Value));
            LoggedQuartersChanged += TotalActiveTime.Refresh;

            Add(new Issue("Coordination"));
            Add(new Issue("Ceremonies"));
        }

        public Issue FindLoggedIn(LoggedQuarter quarter)
        {
            return this.SingleOrDefault(issue => issue.LoggedQuarters.Contains(quarter));
        }
    }
}
