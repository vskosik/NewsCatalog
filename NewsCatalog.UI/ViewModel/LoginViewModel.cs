using NewsCatalog.BLL.Services;
using NewsCatalog.UI.Infrastructure;
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
    public class LoginViewModel
    {
        public string Password { private get; set; }
        public string Login { private get; set; }

        private IAdminService _adminService;

        public bool IsLogged
        {
            get => SingletonLoginResult.IsAllowed;
            set => SingletonLoginResult.IsAllowed = value;
        }

        public LoginViewModel(IAdminService adminService)
        {
            _adminService = adminService;

            ApplyCommand = new RelayCommand(async (param) =>
            {
                MD5 md5 = MD5.Create();
                var passwordBytes = Encoding.ASCII.GetBytes(Password);
                var hashBytes = md5.ComputeHash(passwordBytes);

                var passwordHash = Convert.ToBase64String(hashBytes);

                foreach (var admin in await _adminService.GetAllAsync())
                {
                    if (admin.PasswordHash.Equals(passwordHash) && admin.Username.Equals(Login))
                    {
                        IsLogged = true;
                        ((Window)param).Close();
                    }
                }
            });

            CloseCommand = new RelayCommand((param) =>
            {
                ((Window)param).Close();
            });
        }

        public ICommand ApplyCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
    }
}
