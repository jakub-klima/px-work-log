using PxWorkLog.Core;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace PxWorkLog.Wpf
{
    public class IssueColors
    {
        // For other palettes, see: https://stackoverflow.com/questions/14204827/ms-chart-for-net-predefined-palettes-color-list
        private static readonly IReadOnlyList<Brush> Pallete = new[]
        {
            GetBrush(0x41, 0x8C, 0xF0),
            GetBrush(0xFC, 0xB4, 0x41),
            GetBrush(0xDF, 0x3A, 0x02),
            GetBrush(0x05, 0x64, 0x92),
            GetBrush(0xBF, 0xBF, 0xBF),
            GetBrush(0x1A, 0x3B, 0x69),
            GetBrush(0xFF, 0xE3, 0x82),
            GetBrush(0x12, 0x9C, 0xDD),
            GetBrush(0xCA, 0x6B, 0x4B),
            GetBrush(0x00, 0x5C, 0xDB),
            GetBrush(0xF3, 0xD2, 0x88),
            GetBrush(0x50, 0x63, 0x81),
            GetBrush(0xF1, 0xB9, 0xA8),
            GetBrush(0xE0, 0x83, 0x0A),
            GetBrush(0x78, 0x93, 0xBE)
        };

        private static Brush GetBrush(byte red, byte green, byte blue)
        {
            return new SolidColorBrush(Color.FromRgb(red, green, blue));
        }

        private readonly Dictionary<Issue, Brush> colors = new Dictionary<Issue, Brush>();

        public Brush GetColor(Issue issue)
        {
            if (issue == null)
            {
                return Brushes.White;
            }

            if (!colors.ContainsKey(issue))
            {
                Brush newColor = Pallete.FirstOrDefault(color => !colors.Values.Contains(color)) ?? Brushes.Black;
                colors.Add(issue, newColor);
            }

            return colors[issue];
        }

        public void RemoveColor(Issue issue)
        {
            colors.Remove(issue);
        }
    }
}
