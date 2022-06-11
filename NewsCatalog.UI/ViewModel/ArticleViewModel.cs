using NewsCatalog.BLL.DTO;
using NewsCatalog.UI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NewsCatalog.UI.ViewModel
{
    public class ArticleViewModel
    {
        public BookmarkDTO SelectedArticle => SingletonArticle.SelectedArticle;

        public ArticleViewModel()
        {
            ExitCommand = new RelayCommand((param) =>
            {
                ((Window)param).Close();
            });
        }

        public ICommand ExitCommand { get; private set; }
    }
}
