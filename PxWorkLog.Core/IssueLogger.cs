using PxWorkLog.Core.Utils;
using System.Timers;

namespace PxWorkLog.Core
{
    public class IssueLogger
    {
        private readonly Timer timer = new Timer(1000);
        private readonly IssueCollection issues;
        public ObservableValue<Issue> RunningIssue { get; } = new ObservableValue<Issue>();

        internal IssueLogger(IssueCollection issues)
        {
            this.issues = issues;
            timer.Elapsed += (sender, e) => Tick();
            timer.Start();
        }

        public void StartStop(Issue issue)
        {
            if (issue == RunningIssue.Value)
            {
                Stop();
            }
            else
            {
                Start(issue);
            }
        }

        private void Stop()
        {
            RunningIssue.Value = null;
        }

        private void Start(Issue issue)
        {
            RunningIssue.Value = issue;
            Tick();
        }

        public void Tick()
        {
            if (RunningIssue.Value != null)
            {
                Log(LoggedQuarter.FromNow(), RunningIssue.Value);
            }
        }

        public void Log(LoggedQuarter quarter, Issue issue)
        {
            RemoveLog(quarter);
            issue.LoggedQuarters.Add(quarter);
        }

        public void RemoveLog(LoggedQuarter quarter)
        {
            Issue issue = issues.FindLoggedIn(quarter);
            issue?.LoggedQuarters.Remove(quarter);
        }
    }
}
