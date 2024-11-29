// App.xaml.cs
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using HouseBouncer.Services;
using System;
using System.Threading.Tasks;

namespace HouseBouncer
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; }

        public App(IServiceProvider services)
        {
            InitializeComponent();

            Services = services;

            // Set the Shell as the main page
            MainPage = Services.GetRequiredService<AppShell>();
        }
    }
}
