using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;
using HouseBouncer.Services;

namespace HouseBouncer
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; }
        private readonly DataService _dataService;

        public App(IServiceProvider services)
        {
            InitializeComponent();

            Services = services;

            _dataService = services.GetRequiredService<DataService>();

            Task.Run(async () => await _dataService.LoadDataAsync());

            // Set the Shell as the main page
            MainPage = services.GetRequiredService<AppShell>();
        }


    }
}
