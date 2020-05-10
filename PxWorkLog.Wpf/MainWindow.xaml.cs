using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PxWorkLog.Wpf
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel viewModel;

        public MainWindow(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
            DataContext = this.viewModel;
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            viewModel.TakeBackupScreenshot(this);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Manually activates the binding to copy text to view model; otherwise it is activated too late on lost focus.
                BindingOperations
                    .GetBindingExpression((TextBox)sender, TextBox.TextProperty)
                    .UpdateSource();

                viewModel.AddIssueCommand.Execute(null);
            }
        }
    }
}
