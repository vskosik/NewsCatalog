using NewsCatalog.BLL.DTO;
using NewsCatalog.UI.Infrastructure;
using System.ComponentModel;
using System.Windows.Input;

namespace NewsCatalog.UI.ViewModel
{
    public class ArticleViewModel
    {
        public static event PropertyChangedEventHandler StaticPropertyChanged;

        private static void OnStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        private static BookmarkDTO _selectedArticle;

        public static BookmarkDTO SelectedArticle 
        { 
            get => _selectedArticle; 
            set 
            { 
                _selectedArticle = value;
                OnStaticPropertyChanged("SelectedArticle");
            }
        }

        public ArticleViewModel()
        {
            ExitCommand = new RelayCommand((param) =>
            {
                Switcher.Switch(PagesPull.CurrentPage);
            });
        }

        public ICommand ExitCommand { get; private set; }
    }
}
