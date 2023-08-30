// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using Microsoft.Extensions.Hosting;

using One.Toolbox.Interfaces;
using One.Toolbox.Views;

 

namespace One.Toolbox.Services;

/// <summary> Managed host of the application. </summary>
public class ApplicationHostService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
  
    public ApplicationHostService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary> Triggered when the application host is ready to start the service. </summary>
    /// <param name="cancellationToken"> Indicates that the start process has been aborted. </param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await HandleActivationAsync();
    }

    /// <summary> Triggered when the application host is performing a graceful shutdown. </summary>
    /// <param name="cancellationToken"> Indicates that the shutdown process should no longer be graceful. </param>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    /// <summary> Creates main window during activation. </summary>
    private async Task HandleActivationAsync()
    {
        await Task.CompletedTask;

        if (!Application.Current.Windows.OfType<MainWindow>().Any())
        {
            var mainWindow = _serviceProvider.GetService(typeof(IWindow)) as IWindow;
            mainWindow?.Show();
        }

        await Task.CompletedTask;
    }
}