using PictureBehavioralBiometricAuth.Db.Models;
using PictureBehavioralBiometricAuth.Resources;
using PictureBehavioralBiometricAuth.Services;
using Prism.Commands;

namespace PictureBehavioralBiometricAuth.ViewModels {
    public class UserRegistrationScreenViewModel : ViewModelBase {
        private readonly UserManagementService _userManagementService;
        private string _username = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isError = false;

        public UserRegistrationScreenViewModel(ApplicationContext context, UserManagementService userManagementService) : base(context) {
            _userManagementService = userManagementService;
        }

        public string Username { 
            get => _username;
            set { 
                _username = value;
                RaisePropertyChanged();
            }
        }

        public string ErrorMessage {
            get => _errorMessage;
            set {
                _errorMessage = value;
                RaisePropertyChanged();
            }
        }

        public bool IsError {
            get => _isError;
            set {
                _isError = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand RegisterCommand => new DelegateCommand(UserRegisterAction);

        private void UserRegisterAction() {
            try {
                _userManagementService.AddUser(new UserModel() { Username = this.Username });
                IsError = false;
                ClearForm();
                _context.DialogService.ShowDialog(Common.DialogTitleSuccess, Common.UserRegistrationSuccess);
            } catch(System.Exception exc) {
                ErrorMessage = exc.Message;
                _context.DialogService.ShowDialog(Common.DialogTitleError, string.Format(Common.UserRegistrationFailed, exc.Message));
                IsError = true;
            }
        }

        private async void ClearForm() {
            Username = string.Empty;
        }
    }
}
