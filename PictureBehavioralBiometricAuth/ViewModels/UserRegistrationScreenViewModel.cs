using Avalonia.Controls;
using PictureBehavioralBiometricAuth.Components.Forms;
using PictureBehavioralBiometricAuth.Db.Models;
using PictureBehavioralBiometricAuth.Resources;
using PictureBehavioralBiometricAuth.Services;
using Prism.Commands;

namespace PictureBehavioralBiometricAuth.ViewModels {
    public class UserRegistrationScreenViewModel : ViewModelBase {
        private readonly UserManagementService _userManagementService;
        private readonly AuthenticationService _authenticationService;
        private string _username = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isError = false;

        public UserRegistrationScreenViewModel(ApplicationContext context, UserManagementService userManagementService, AuthenticationService authenticationService) : base(context) {
            _userManagementService = userManagementService;
            _authenticationService = authenticationService;
        }

        public LoginForm? Form { get; set; }
        private AuthImageModel? _authImage;

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
                if (Form == null) throw new System.Exception("Cannot find form reference, try relaunching application or wait a bit longer.");
                _userManagementService.AddUser(new UserModel() { 
                    Username = this.Username,
                    RegistrationTime = System.DateTime.UtcNow,
                    AuthImage = GetAuthImage(),
                    Points = Form.GetAuthPoints(),
                });
                IsError = false;
                ClearForm();
                _context.DialogService.ShowDialog(Common.DialogTitleSuccess, Common.UserRegistrationSuccess);
            } catch(System.Exception exc) {
                ErrorMessage = exc.Message;
                _context.DialogService.ShowDialog(Common.DialogTitleError, string.Format(Common.UserRegistrationFailed, exc.Message));
                IsError = true;
            }
        }

        public AuthImageModel GetAuthImage() {
            if (_authImage != null) return _authImage;
            var regions = LoginForm.GetRegions();
            var image = _authenticationService.GetAuthImage("house");
            if (image != null) {
                return image;
            } else {
                image = new AuthImageModel {
                    Name = "house",
                    Width = 520,
                    Height = 440,
                    GridCellSize = LoginForm.GRID_CELL_SIZE,
                    Regions = regions,
                };
                _authenticationService.AddAuthImage(image);
                _authImage = image;
                return image;
            }
        }

        private async void ClearForm() {
            Username = string.Empty;
            Form?.ClearForm();
        }
    }
}
