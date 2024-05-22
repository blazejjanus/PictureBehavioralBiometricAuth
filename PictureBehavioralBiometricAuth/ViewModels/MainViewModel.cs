using Avalonia.Controls;
using Prism.Commands;
using System;

namespace PictureBehavioralBiometricAuth.ViewModels;

public partial class MainViewModel : ViewModelBase {
    private readonly ApplicationContext _context;

    public SettingsScreenViewModel SettingsScreenViewModel { get; set; }

    public MainViewModel(ApplicationContext context, SettingsScreenViewModel settingsScreenViewModel) {
        _context = context;
        SettingsScreenViewModel = settingsScreenViewModel;
    }

    public DelegateCommand<SelectionChangedEventArgs> TabChangedCommand => new DelegateCommand<SelectionChangedEventArgs>(TabChangedAction);

    private void TabChangedAction(SelectionChangedEventArgs args) {
        
    }
}
