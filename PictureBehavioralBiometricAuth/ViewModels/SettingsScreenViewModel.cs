using Avalonia.Controls;
using Avalonia.Platform.Storage;
using PictureBehavioralBiometricAuth.Shared.Config;
using Prism.Commands;
using System;
using System.IO;
using System.Text.Json;

namespace PictureBehavioralBiometricAuth.ViewModels
{
    public class SettingsScreenViewModel : ViewModelBase {
        private bool _settingsChanged = false;
        private bool _isSaveButtonEnabled = false;

        public SettingsScreenViewModel(ApplicationContext context) : base(context) {}

        public bool IsDebugMode {
            get => _context.Settings.DebugMode;
            set {
                _context.Settings.DebugMode = value;
                RaisePropertyChanged();
            }
        }

        public int AuthPassThreshold {
            get => _context.Settings.LoginPassThreshold;
            set {
                _context.Settings.LoginPassThreshold = value;
                RaisePropertyChanged();
            }
        }

        public string DatabaseURL { 
            get => _context.Settings.DbSettings.Url;
            set {
                _context.Settings.DbSettings.Url = value;
                _settingsChanged = true;
                IsSaveButtonEnabled = false;
                RaisePropertyChanged();
            }
        }

        public int DatabasePort { 
            get => _context.Settings.DbSettings.Port;
            set {
                _context.Settings.DbSettings.Port = value;
                _settingsChanged = true;
                IsSaveButtonEnabled = false;
                RaisePropertyChanged();
            }
        }

        public string DatabaseName { 
            get => _context.Settings.DbSettings.DatabaseName;
            set {
                _context.Settings.DbSettings.DatabaseName = value;
                _settingsChanged = true;
                IsSaveButtonEnabled = false;
                RaisePropertyChanged();
            }
        }

        public string DatabaseUsername { 
            get => _context.Settings.DbSettings.User;
            set {
                _context.Settings.DbSettings.User = value;
                _settingsChanged = true;
                IsSaveButtonEnabled = false;
                RaisePropertyChanged();
            }
        }

        public string DatabasePassword { 
            get => _context.Settings.DbSettings.Password;
            set {
                _context.Settings.DbSettings.Password = value;
                _settingsChanged = true;
                IsSaveButtonEnabled = false;
                RaisePropertyChanged();
            }
        }

        public bool IsSaveButtonEnabled { 
            get => _isSaveButtonEnabled;
            set {
                _isSaveButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand TextConnectionCommand => new DelegateCommand(TestConnectionAction);

        public DelegateCommand SaveCommand => new DelegateCommand(SaveSettingsAction);

        public DelegateCommand ImportCommand => new DelegateCommand(ImportAction);

        public DelegateCommand ExportCommand => new DelegateCommand(ExportAction);

        public DelegateCommand WriteDefaultsCommand => new DelegateCommand(WriteDefaultsAction);

        private void SaveSettingsAction() {
            _context.Settings.WriteSettings();
        }

        private void TestConnectionAction() {
            if (_settingsChanged) {
                _context.Refresh();
                _settingsChanged = false;
            }
            if (_context.DbContext.TestConnection()) {
                _context.DialogService.ShowDialog("Success", "Connection successful!");
                IsSaveButtonEnabled = true;
            } else {
                _context.DialogService.ShowDialog("Error", "Connection failed!");
                IsSaveButtonEnabled = false;
            }
        }

        private async void ImportAction() {
            try {
                var topLevel = TopLevel.GetTopLevel(_context.View);
                var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions {
                    Title = "Open Text File",
                    AllowMultiple = false
                });
                if (files.Count >= 1) {
                    await using var stream = await files[0].OpenReadAsync();
                    using var streamReader = new StreamReader(stream);
                    var fileContent = await streamReader.ReadToEndAsync();
                    var config = JsonSerializer.Deserialize<AppSettings>(fileContent ?? throw new NullReferenceException("Config file is empty.")) 
                        ?? throw new NullReferenceException("Deserialized config file was null!");
                    DatabaseURL = config.DbSettings.Url;
                    DatabasePort = config.DbSettings.Port;
                    DatabaseName = config.DbSettings.DatabaseName;
                    DatabaseUsername = config.DbSettings.User;
                    DatabasePassword = config.DbSettings.Password;
                    IsDebugMode = config.DebugMode;
                    AuthPassThreshold = config.LoginPassThreshold;
                    _settingsChanged = true;
                    IsSaveButtonEnabled = false;
                }
            }catch(Exception exc) {
                _context.DialogService.ShowDialog("Error", exc.Message);
            }
        }

        private async void ExportAction() {
            try {
                var topLevel = TopLevel.GetTopLevel(_context.View);
                var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions {
                    Title = "Save Text File"
                });
                if (file is not null) {
                    await using var stream = await file.OpenWriteAsync();
                    using var streamWriter = new StreamWriter(stream);
                    await streamWriter.WriteAsync(JsonSerializer.Serialize(_context.Settings, new JsonSerializerOptions() { WriteIndented = true })) ;
                }
            } catch(Exception exc) {
                _context.DialogService.ShowDialog("Error", exc.Message);
            }
        }

        private void WriteDefaultsAction() {
            _context.Settings = new AppSettings();
            _context.Settings.WriteSettings();
        }
    }
}
