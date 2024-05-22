using Avalonia.Controls;
using MsBox.Avalonia.Enums;
using PictureBehavioralBiometricAuth.Db;
using PictureBehavioralBiometricAuth.Shared;
using System;
using System.IO;

namespace PictureBehavioralBiometricAuth {
    public class ApplicationContext {
        public DbSettings Settings { get; set; }
        public UserDialogService DialogService { get; } = new UserDialogService();
        public DataContext DbContext { get; private set; }

        public Control View { get; internal set; }

        public static string AppDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PictureBehavioralBiometricAuth");

        public ApplicationContext() {
            try {
                CreateAppDirectoryIfNotExists();
                Settings = DbSettings.ReadDbSettings();
                DbContext = new DataContext(Settings);
            } catch (Exception exc) {
                if (exc is ConfigurationException) {
                    DialogService.ShowDialog(Resources.Common.DialogTitleError, Resources.Common.ConfigurationNotFoundErrorDialogMessage, ButtonEnum.YesNo, DialogCallback);
                    Settings = new DbSettings();
                    Settings.WriteDbSettings();
                }
            }
        }

        public void Refresh() {
            DbContext = new DataContext(Settings);
        }

        private void DialogCallback(ButtonResult result) {
            if(result == ButtonResult.Yes) {
                Settings = new DbSettings();
                Settings.WriteDbSettings();
            }
        }

        private void CreateAppDirectoryIfNotExists() {
            if (!Directory.Exists(AppDirectory))
                Directory.CreateDirectory(AppDirectory);
        }
    }
}