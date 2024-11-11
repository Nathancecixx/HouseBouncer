using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace HouseBouncer.Services
{
    // Define the interface
    public interface IDialogService
    {
        Task<string> ShowInputDialogAsync(string title, string message, string placeholder);
        Task ShowAlertAsync(string title, string message, string cancel);
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
    }
}
