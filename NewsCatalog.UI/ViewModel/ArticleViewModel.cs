using NewsCatalog.BLL.DTO;
using NewsCatalog.UI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatalog.UI.ViewModel
{
    public class ArticleViewModel
    {
        public BookmarkDTO SelectedArticle
        {
            get => SingletonArticle.SelectedArticle;
            set => SingletonArticle.SelectedArticle = value;
        }
    }
}
