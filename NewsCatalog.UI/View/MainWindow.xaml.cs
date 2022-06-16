using NewsCatalog.UI.Infrastructure;
using System.Windows;
using System.Windows.Controls;

namespace NewsCatalog.UI.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Switcher.PageSwitcher = this;
            PagesPull.CurrentPage = PagesPull.Pages["MainView"];
            Switcher.Switch(PagesPull.Pages["MainView"]);
        }

        public void Navigate(UserControl nextPage)
        {
            this.Content = nextPage;
        }
    }
}
