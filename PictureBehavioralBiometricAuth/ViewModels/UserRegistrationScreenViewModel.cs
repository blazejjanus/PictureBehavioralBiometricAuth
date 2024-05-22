using Prism.Commands;

namespace PictureBehavioralBiometricAuth.ViewModels {
    public class UserRegistrationScreenViewModel : ViewModelBase {
        private string _username = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isError = false;

        public UserRegistrationScreenViewModel(ApplicationContext context) : base(context) { }

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

        private async void UserRegisterAction() {
            //TODO: Add user service
        }
    }
}
