namespace PictureBehavioralBiometricAuth.ViewModels {
    public partial class MainViewModel : ViewModelBase {
        public SettingsScreenViewModel SettingsScreenViewModel { get; set; }
        public UserRegistrationScreenViewModel UserRegistrationScreenViewModel { get; set; }

        public MainViewModel(ApplicationContext context, SettingsScreenViewModel settingsScreenViewModel, 
                            UserRegistrationScreenViewModel userRegistrationScreenViewModel) 
            : base(context) {
            SettingsScreenViewModel = settingsScreenViewModel;
            UserRegistrationScreenViewModel = userRegistrationScreenViewModel;
        }
    }
}
