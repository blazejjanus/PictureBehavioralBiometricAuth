using PictureBehavioralBiometricAuth.Components.Forms;
using PictureBehavioralBiometricAuth.Db.Models;
using PictureBehavioralBiometricAuth.Resources;
using PictureBehavioralBiometricAuth.Services;
using Prism.Commands;
using System;

namespace PictureBehavioralBiometricAuth.ViewModels {
    public class LoginScreenViewModel : ViewModelBase {
        private readonly UserManagementService _userManagementService;
        private readonly AuthenticationService _authenticationService;
        private LoginForm? _form;

        public LoginScreenViewModel(ApplicationContext context, UserManagementService userManagementService, AuthenticationService authenticationService) : base(context) {
            _userManagementService = userManagementService;
            _authenticationService = authenticationService;
        }

        public LoginForm? Form {
            get => _form;
            set {
                if (value == null) return;
                _form = value;
                _form.ErrorHandler += OnError;
                _form.DisplayGrid = _context.Settings.DebugMode;
                _form.DisplayRegions = _context.Settings.DebugMode;
            }
        }

        private AuthImageModel? _authImage;
        private bool _isError = false;
        private string _errorMessage = string.Empty;
        private string _username = string.Empty;

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

        public DelegateCommand LoginCommand => new DelegateCommand(UserLoginAction);

        private void UserLoginAction() {
            try {
                if (Form == null) throw new Exception("Cannot find form reference, try relaunching application or wait a bit longer.");
                var user = _userManagementService.GetUser(Username);
                if(user == null) throw new Exception("User does not exist!");
                var points = Form.GetAuthPoints();
                if(!_authenticationService.LoginUser(Username, user.AuthImage.Name, points, out int detectedSimilarity)) {
                    throw new Exception($"Points similarity: {detectedSimilarity}%, {_context.Settings.LoginPassThreshold}% is required to login.");
                }
                IsError = false;
                ClearForm();
                _context.DialogService.ShowDialog(Common.DialogTitleSuccess, Common.UserLoginSuccessfull + $"\nPoints similarity: {detectedSimilarity}%");
            } catch (Exception exc) {
                ErrorMessage = exc.Message;
                _context.DialogService.ShowDialog(Common.DialogTitleError, Common.UserLoginFailed + exc.Message);
                ClearForm();
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

        private void OnError(object? sender, string message) {
            if (string.IsNullOrEmpty(message)) {
                IsError = false;
                ErrorMessage = string.Empty;
            } else {
                DisplayError(message);
            }
        }

        private void DisplayError(string message) {
            ErrorMessage = message;
            IsError = true;
        }
    }
}
