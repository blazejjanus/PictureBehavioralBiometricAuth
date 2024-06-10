namespace PictureBehavioralBiometricAuth.ViewModels {
    public partial class MainViewModel : ViewModelBase {
        public SettingsScreenViewModel SettingsScreenViewModel { get; set; }
        public UserRegistrationScreenViewModel UserRegistrationScreenViewModel { get; set; }
        public LoginScreenViewModel LoginScreenViewModel { get; set; }

        public MainViewModel(ApplicationContext context,
                            SettingsScreenViewModel settingsScreenViewModel, 
                            UserRegistrationScreenViewModel userRegistrationScreenViewModel,
                            LoginScreenViewModel loginScreenViewModel) 
            : base(context) {
            SettingsScreenViewModel = settingsScreenViewModel;
            UserRegistrationScreenViewModel = userRegistrationScreenViewModel;
            LoginScreenViewModel = loginScreenViewModel;
        }
    }
}
