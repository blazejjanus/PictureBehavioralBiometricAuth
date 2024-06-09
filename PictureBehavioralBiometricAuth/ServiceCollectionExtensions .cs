using Microsoft.Extensions.DependencyInjection;
using PictureBehavioralBiometricAuth.Services;
using PictureBehavioralBiometricAuth.ViewModels;

namespace PictureBehavioralBiometricAuth {
    public static class ServiceCollectionExtensions {
        public static void AddCommonServices(this IServiceCollection collection) {
            collection.AddSingleton<ApplicationContext>();
            collection.AddSingleton<UserManagementService>();
            collection.AddSingleton<AuthenticationService>();
            collection.AddTransient<MainViewModel>();
            collection.AddTransient<SettingsScreenViewModel>();
            collection.AddTransient<UserRegistrationScreenViewModel>();
        }
    }
}
