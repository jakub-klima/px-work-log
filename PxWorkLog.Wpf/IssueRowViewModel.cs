using PxWorkLog.Core;
using PxWorkLog.Core.Utils;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PxWorkLog.Wpf
{
    public class IssueRowViewModel
    {
        internal Issue Issue { get; }

        public string Name { get; set; }
        public IReadOnlyObservableValue<Brush> Background { get; }
        public IReadOnlyObservableValue<string> StartStopButtonText { get; }
        public Brush Color { get; }
        public ObservableValue<bool?> IsColorChecked { get; } = new ObservableValue<bool?>(false);
        public IReadOnlyObservableValue<string> TimeText { get; }

        public ICommand StartStopButtonCommand { get; }
        public ICommand RemoveButtonCommand { get; }
        public ICommand ColorClickedCommand { get; }

        public event EventHandler ColorClicked;

        public IssueRowViewModel(Issue issue, WorkLogCore workLog, CollectionEventAggregator colorClickedAggregator, IssueColors issueColors)
        {
            Issue = issue;
            Name = Issue.Name;
            TimeText = ProjectedObservableValue<string>.FromObservableValue(issue.TotalLoggedTime, time => time.ToString("h\\:mm"));
            Color = issueColors.GetColor(Issue);
            Background = ProjectedObservableValue<Brush>.FromObservableValue(workLog.Logger.RunningIssue, runningIssue => runningIssue == Issue ? Brushes.LightGreen : Brushes.White);
            StartStopButtonText = ProjectedObservableValue<string>.FromObservableValue(workLog.Logger.RunningIssue, runningIssue => runningIssue == Issue ? "Stop" : "Start");
            StartStopButtonCommand = new DelegateCommand(() => workLog.Logger.StartStop(Issue));
            RemoveButtonCommand = new DelegateCommand(() =>
            {
                if (MessageBox.Show($"Do you want to delete '{Name}' issue and its logged time?", "Remove issue", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    workLog.Issues.Remove(Issue);
                    issueColors.RemoveColor(Issue);
                }
            });
            ColorClickedCommand = new DelegateCommand(() => ColorClicked?.Invoke(this, EventArgs.Empty));
            colorClickedAggregator.ItemChanged += (sender, e) => IsColorChecked.Value = sender == this && IsColorChecked.Value == false;
        }
    }
}
