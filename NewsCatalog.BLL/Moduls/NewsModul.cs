using NewsCatalog.BLL.ApiManager;
using NewsCatalog.BLL.Services;
using NewsCatalog.DAL.Context;
using NewsCatalog.DAL.Infrastructure;
using NewsCatalog.DAL.Repositories;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatalog.BLL.Moduls
{
    public class NewsModul : NinjectModule
    {
        public override void Load()
        {
            Bind<BookmarkRepository>().To<BookmarkRepository>();
            Bind<AdminRepository>().To<AdminRepository>();
            Bind<IBookmarkService>().To<BookmarkService>();
            Bind<IAdminService>().To<AdminService>();
            Bind<DbContext>().To<NewsContext>();
            Bind<NetworkManager>().To<NetworkManager>();
            Bind<NewsApiManager>().To<NewsApiManager>();
        }
    }
}
