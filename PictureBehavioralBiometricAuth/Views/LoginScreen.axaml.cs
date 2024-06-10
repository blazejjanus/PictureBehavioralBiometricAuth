using Avalonia.Controls;
using PictureBehavioralBiometricAuth.ViewModels;
using System;

namespace PictureBehavioralBiometricAuth.Views {
    public partial class LoginScreen : UserControl {
        public LoginScreen() {
            InitializeComponent();
            DataContextChanged += UserRegistrationScreen_DataContextChanged;
        }

        private void UserRegistrationScreen_DataContextChanged(object? sender, EventArgs e) {
            if (DataContext is LoginScreenViewModel ctx) {
                Form.InitImage(ctx?.GetAuthImage() ?? throw new NullReferenceException("Data context"));
                ctx.Form = Form;
            }
        }
    }
}
