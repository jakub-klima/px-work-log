using PxWorkLog.Core;
using PxWorkLog.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PxWorkLog.Wpf
{
    public partial class QuarterBoxViewModel
    {
        public Thickness Margin { get; }
        public ProjectedObservableValue<Brush> Background { get; }
        public ICommand LogCommand { get; }
        public ICommand RemoveLogCommand { get; }

        public QuarterBoxViewModel(LoggedQuarter quarter, IssueCollection issues, IssueLogger logger, IEnumerable<IssueRowViewModel> issueRows, IssueColors issueColors)
        {
            Margin = new Thickness(1, 1, quarter.Quarter == 3 ? 1 : 0, 1);

            Background = new ProjectedObservableValue<Brush>(() => issueColors.GetColor(issues.FindLoggedIn(quarter)));
            issues.CollectionChanged += Background.Refresh;
            var eventAggregator = new CollectionEventAggregator();
            eventAggregator.Initialize(issues,
                issue => issue.LoggedQuarters.CollectionChanged += eventAggregator.RaiseItemChanged,
                issue => issue.LoggedQuarters.CollectionChanged -= eventAggregator.RaiseItemChanged);
            eventAggregator.ItemChanged += Background.Refresh;

            LogCommand = new DelegateCommand(() =>
            {
                IssueRowViewModel checkedIssueRow = issueRows.SingleOrDefault(i => i.IsColorChecked.Value == true);
                if (checkedIssueRow != null)
                {
                    logger.Log(quarter, checkedIssueRow.Issue);
                }
            });
            RemoveLogCommand = new DelegateCommand(() => logger.RemoveLog(quarter));
        }
    }
}
