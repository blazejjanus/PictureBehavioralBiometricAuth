using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Threading.Tasks;
using Avalonia.Threading;
using MsBox.Avalonia.Base;
using System;

namespace PictureBehavioralBiometricAuth.Services
{
    public class UserDialogService
    {
        public void ShowDialog(string title, string message, ButtonEnum button = ButtonEnum.Ok, Action<ButtonResult>? callback = null)
        {
            var dialog = MessageBoxManager.GetMessageBoxStandard(title, message, button);
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var result = await dialog.ShowAsync();
                callback?.Invoke(result);
            });
        }
    }
}