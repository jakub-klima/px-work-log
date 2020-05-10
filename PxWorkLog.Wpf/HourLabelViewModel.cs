using System.Windows;

namespace PxWorkLog.Wpf
{
    public class HourLabelViewModel
    {
        public int Hour { get; }
        public Thickness Margin { get; }

        public HourLabelViewModel(int hour, bool isLast)
        {
            Hour = hour;
            Margin = new Thickness(0, 0, isLast ? 0 : 66, 0);
        }
    }
}
