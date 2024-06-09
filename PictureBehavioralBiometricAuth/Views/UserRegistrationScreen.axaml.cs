using Avalonia.Controls;
using PictureBehavioralBiometricAuth.ViewModels;
using System;

namespace PictureBehavioralBiometricAuth.Views {
    public partial class UserRegistrationScreen : UserControl {

        public UserRegistrationScreen() {
            InitializeComponent();
            DataContextChanged += UserRegistrationScreen_DataContextChanged;
        }

        private void UserRegistrationScreen_DataContextChanged(object? sender, EventArgs e) {
            if(DataContext is UserRegistrationScreenViewModel ctx) {
                Form.InitImage(ctx?.GetAuthImage() ?? throw new NullReferenceException("Data context"));
                ctx.Form = Form;
            }
        }
    }
}
