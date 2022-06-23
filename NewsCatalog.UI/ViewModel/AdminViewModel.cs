using NewsCatalog.BLL.DTO;
using NewsCatalog.BLL.Services;
using NewsCatalog.UI.Infrastructure;
using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace NewsCatalog.UI.ViewModel
{
    public class AdminViewModel : BaseNotifyPropertyChanged
    {
        public static event PropertyChangedEventHandler StaticPropertyChanged;

        private static void OnStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        private static AdminDTO _selectedAdmin;
        private IAdminService _adminService;
        private string _status;
        private string _password;

        public static AdminDTO SelectedAdmin
        {
            get => _selectedAdmin;
            set
            {
                _selectedAdmin = value;
                OnStaticPropertyChanged("SelectedAdmin");
            }
        }

        public string Username { get; set; }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOnPropertyChanged();
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                NotifyOnPropertyChanged();
            }
        }

        public AdminViewModel(IAdminService adminService)
        {
            _adminService = adminService;

            Status = "Enter data:";

            InitCommands();
        }

        public AdminViewModel() { }

        private void InitCommands()
        {
            ConfirmCreateCommand = new RelayCommand((param) =>
            {
                if (Username != null && Password != null)
                {
                    if (_adminService.GetAll().Any(x => x.Username == Username))
                    {
                        Status = "Username already exist!";
                        return;
                    }

                    using (var md5 = MD5.Create())
                    {
                        var passwordBytes = Encoding.ASCII.GetBytes(Password);
                        var hashBytes = md5.ComputeHash(passwordBytes);

                        var passwordHash = Convert.ToBase64String(hashBytes);

                        SelectedAdmin = new AdminDTO
                        {
                            Username = Username,
                            PasswordHash = passwordHash
                        };

                        _adminService.Create(SelectedAdmin);
                    }

                    Username = null;
                    Password = null;

                    Status = "Enter data:";
                    Switcher.Switch(PagesPull.CurrentPage);
                }
            });

            ConfirmUpdateCommand = new RelayCommand((param) =>
            {
                if (Password != null)
                {
                    using (var md5 = MD5.Create())
                    {
                        var passwordBytes = Encoding.ASCII.GetBytes(Password);
                        var hashBytes = md5.ComputeHash(passwordBytes);

                        var passwordHash = Convert.ToBase64String(hashBytes);

                        SelectedAdmin.PasswordHash = passwordHash;

                        _adminService.Update(SelectedAdmin);
                    }

                    ((PasswordBox)param).Password = null;

                    Switcher.Switch(PagesPull.CurrentPage);
                }
                else
                {
                    Status = "Enter password!";
                }
            });

            ExitCommand = new RelayCommand((param) =>
            {
                Switcher.Switch(PagesPull.CurrentPage);
            });
        }

        public ICommand ConfirmCreateCommand { get; private set; }
        public ICommand ConfirmUpdateCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
    }
}
