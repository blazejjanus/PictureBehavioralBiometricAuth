using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Threading.Tasks;
using Avalonia.Threading;
using MsBox.Avalonia.Base;
using System;

namespace PictureBehavioralBiometricAuth {
    public class UserDialogService {
        //TODO: Fix dialogs
        public void ShowDialog(string title, string message, ButtonEnum button = ButtonEnum.Ok, Action<ButtonResult>? callback = null) {
            ShowDialogInternal(title, message, button, callback);
        }

        private void ShowDialogInternal(string title, string message, ButtonEnum button, Action<ButtonResult>? callback) {
            var dialog = MessageBoxManager.GetMessageBoxStandard(title, message, button);
            Dispatcher.UIThread.InvokeAsync(async () => {
                var result = await dialog.ShowAsync();
                callback?.Invoke(result);
            });
        }
    }
}