using PxWorkLog.Core;
using PxWorkLog.Core.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PxWorkLog.Wpf
{
    public class MainWindowViewModel
    {
        private readonly WorkLogCore model;

        public IReadOnlyCollection<QuarterBoxViewModel> QuarterBoxes { get; }
        public IReadOnlyCollection<HourLabelViewModel> HourLabels { get; }
        public ProjectedObservableCollection<IssueRowViewModel> IssueRows { get; set; }

        public IReadOnlyObservableValue<string> TotalTimeText { get; }
        public ICommand AddIssueCommand { get; }
        public ObservableValue<string> NewIssueName { get; } = new ObservableValue<string>("");

        public MainWindowViewModel(WorkLogCore model)
        {
            this.model = model;

            TotalTimeText = ProjectedObservableValue<string>.FromObservableValue(model.Issues.TotalActiveTime, time => $"Total {time:h\\:mm}");

            HourLabels = Enumerable
                .Range(7, 12 + 1)
                .Select(hour => new HourLabelViewModel(hour, isLast: hour == 19))
                .ToArray();

            var issueColors = new IssueColors();

            var colorClickedAggregator = new CollectionEventAggregator();
            IssueRows = ProjectedObservableCollection<IssueRowViewModel>.Create(this.model.Issues, issue => new IssueRowViewModel(issue, model, colorClickedAggregator, issueColors));
            colorClickedAggregator.Initialize(IssueRows,
                issue => issue.ColorClicked += colorClickedAggregator.RaiseItemChanged,
                issue => issue.ColorClicked -= colorClickedAggregator.RaiseItemChanged);

            QuarterBoxes = Enumerable
                .Range(4 * 7, 4 * 12)
                .Select(quarterIndex => new LoggedQuarter(quarterIndex / 4, quarterIndex % 4))
                .Select(quarter => new QuarterBoxViewModel(quarter, model.Issues, model.Logger, IssueRows, issueColors))
                .ToArray();

            AddIssueCommand = new DelegateCommand(() =>
            {
                this.model.Issues.Add(new Issue(NewIssueName.Value));
                NewIssueName.Value = "";
            });
        }

        public void TakeBackupScreenshot(Window window)
        {
            const int cropWidth = 355;
            var pngImage = new PngBitmapEncoder();
            var renderTargetBitmap = new RenderTargetBitmap(cropWidth, (int)window.Height, 96, 96, PixelFormats.Default);
            renderTargetBitmap.Render(window);
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            using (Stream fileStream = File.Create("backup.png"))
            {
                pngImage.Save(fileStream);
            }

            File.Move("backup.png", model.BackupPath.GetPath());
        }
    }
}
