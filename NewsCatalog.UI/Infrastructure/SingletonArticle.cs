using NewsCatalog.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatalog.UI.Infrastructure
{
    public static class SingletonArticle
    {
        public static BookmarkDTO SelectedArticle { get; set; }
    }
}
