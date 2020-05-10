using PxWorkLog.Core;
using System.Windows;
using System.Windows.Threading;

namespace PxWorkLog.Wpf
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var model = new WorkLogCore();
            var viewModel = new MainWindowViewModel(model);
            var view = new MainWindow(viewModel);
            view.Show();
        }
    }
}
