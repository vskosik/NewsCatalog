using NewsCatalog.BLL.Services;
using NewsCatalog.UI.Infrastructure;
using NewsCatalog.UI.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NewsCatalog.UI.ViewModel
{
    public class LoginViewModel : BaseNotifyPropertyChanged
    {
        private IAdminService _adminService;
        private string _status;

        public string Password { private get; set; }
        public string Login { private get; set; }
        public string Status 
        { 
            get => _status;
            set
            {
                _status = value;
                NotifyOnPropertyChanged();
            }
        }

        public bool IsLogged
        {
            get => SingletonLoginResult.IsLogged;
            set => SingletonLoginResult.IsLogged = value;
        }

        public LoginViewModel(IAdminService adminService)
        {
            Status = "Enter your credentials:";
            _adminService = adminService;

            ApplyCommand = new RelayCommand((param) =>
            {
                MD5 md5 = MD5.Create();
                var passwordBytes = Encoding.ASCII.GetBytes(Password ?? string.Empty);
                var hashBytes = md5.ComputeHash(passwordBytes);

                var passwordHash = Convert.ToBase64String(hashBytes);

                foreach (var admin in _adminService.GetAll())
                {
                    if (admin.PasswordHash.Equals(passwordHash) && admin.Username.Equals(Login))
                    {
                        IsLogged = true;
                        Switcher.Switch(PagesPull.Pages["AdminView"]);
                        ((Window)param).Close();
                    }
                }

                Status = "Incorrect credentials!";
            });

            CloseCommand = new RelayCommand((param) =>
            {
                Switcher.Switch(PagesPull.Pages["MainView"]);
                ((Window)param).Close();
            });
        }

        public ICommand ApplyCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
    }
}
