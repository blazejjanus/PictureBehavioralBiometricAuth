using Avalonia.Controls;
using MsBox.Avalonia.Enums;
using PictureBehavioralBiometricAuth.Db;
using PictureBehavioralBiometricAuth.Services;
using PictureBehavioralBiometricAuth.Shared;
using PictureBehavioralBiometricAuth.Shared.Config;
using System;
using System.IO;

namespace PictureBehavioralBiometricAuth {
    public class ApplicationContext {
        public AppSettings Settings { get; set; }
        public UserDialogService DialogService { get; } = new UserDialogService();
        public DataContext DbContext { get; private set; }

        public Control View { get; internal set; }

        public static string AppDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PictureBehavioralBiometricAuth");

        public ApplicationContext() {
            try {
                CreateAppDirectoryIfNotExists();
                Settings = AppSettings.ReadSettings();
                DbContext = new DataContext(Settings.DbSettings);
            } catch (Exception exc) {
                if (exc is ConfigurationException) {
                    DialogService.ShowDialog(Resources.Common.DialogTitleError, Resources.Common.ConfigurationNotFoundErrorDialogMessage, ButtonEnum.YesNo, DialogCallback);
                    SaveDefaultSettings();
                }
            }
        }

        public void Refresh() {
            DbContext = new DataContext(Settings.DbSettings);
        }

        private void DialogCallback(ButtonResult result) {
            if (result == ButtonResult.Yes) {
                SaveDefaultSettings();
            }
        }

        private void SaveDefaultSettings() {
            Settings = new AppSettings();
            Settings.WriteSettings();
        }

        private void CreateAppDirectoryIfNotExists() {
            if (!Directory.Exists(AppDirectory))
                Directory.CreateDirectory(AppDirectory);
        }
    }
}