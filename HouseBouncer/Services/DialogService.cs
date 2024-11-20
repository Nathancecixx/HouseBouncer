using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace HouseBouncer.Services
{
    // Define the interface
    public interface IDialogService
    {
        Task<string> ShowInputDialogAsync(string title, string message, string placeholder);
        Task ShowAlertAsync(string title, string message, string cancel);
        Task<bool> ShowConfirmationDialogAsync(string title, string message, string accept, string cancel);
        Task<string> ShowOptionsDialogAsync(string title, string message, IEnumerable<string> options);
    }

    // Implement the interface
    public class DialogService : IDialogService
    {
        public async Task<string> ShowInputDialogAsync(string title, string message, string placeholder)
        {
            return await Application.Current.MainPage.DisplayPromptAsync(title, message, placeholder: placeholder);
        }
        public async Task ShowAlertAsync(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
        public async Task<bool> ShowConfirmationDialogAsync(string title, string message, string accept, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
        public async Task<string> ShowOptionsDialogAsync(string title, string message, IEnumerable<string> options)
        {
            return await Application.Current.MainPage.DisplayActionSheet(message, "Cancel", null, options.ToArray());
        }
    }
}
