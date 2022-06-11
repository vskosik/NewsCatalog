using NewsCatalog.BLL.Moduls;
using NewsCatalog.UI.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatalog.UI.Infrastructure
{
    public class ViewModelLocator
    {
        private IKernel container;
        public ViewModelLocator()
        {
            container = new StandardKernel(new NewsModul());
        }

        public MainViewModel MainViewModel => container.Get<MainViewModel>();
        public LoginViewModel LoginViewModel => container.Get<LoginViewModel>();
    }
}