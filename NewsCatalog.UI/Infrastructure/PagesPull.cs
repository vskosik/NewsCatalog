using NewsCatalog.UI.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NewsCatalog.UI.Infrastructure
{
    public static class PagesPull
    {
        private static readonly Dictionary<string, UserControl> _pages;

        public static Dictionary<string, UserControl> Pages => _pages;
        public static UserControl CurrentPage { get; set; }

        static PagesPull()
        {
            _pages = new Dictionary<string, UserControl>
            {
                { "MainView", new MainView() },
                { "AdminView", new AdminView() },
                { "ArticleView", new ArticleView() }
            };
        }
    }
}
