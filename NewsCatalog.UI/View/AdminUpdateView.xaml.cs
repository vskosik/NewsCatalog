using System.Windows;
using System.Windows.Controls;

namespace NewsCatalog.UI.View
{
    /// <summary>
    /// Interaction logic for AdminUpdateView.xaml
    /// </summary>
    public partial class AdminUpdateView : UserControl
    {
        public AdminUpdateView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((dynamic)DataContext).Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
