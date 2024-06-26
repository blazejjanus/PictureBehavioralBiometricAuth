﻿using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using PictureBehavioralBiometricAuth.ViewModels;
using PictureBehavioralBiometricAuth.Views;

namespace PictureBehavioralBiometricAuth;

public partial class App : Application {
    public override void Initialize() {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted() {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        var collection = new ServiceCollection();
        collection.AddCommonServices();
        var services = collection.BuildServiceProvider();
        var vm = services.GetRequiredService<MainViewModel>();
        var ctx = services.GetRequiredService<ApplicationContext>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            desktop.MainWindow = new MainWindow {
                DataContext = vm
            };
            ctx.View = desktop.MainWindow;
        } else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform) {
            singleViewPlatform.MainView = new MainView {
                DataContext = vm
            };
            ctx.View = singleViewPlatform.MainView;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
