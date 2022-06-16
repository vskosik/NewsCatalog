using NewsCatalog.UI.View;
using System.Windows.Controls;

namespace NewsCatalog.UI.Infrastructure
{
    public static class Switcher
    {
        public static MainWindow PageSwitcher { get; set; }

        public static void Switch(UserControl newPage)
        {
            PageSwitcher.Navigate(newPage);
        }
    }
}
