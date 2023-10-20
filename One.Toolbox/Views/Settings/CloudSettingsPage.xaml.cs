﻿using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels;

namespace One.Toolbox.Views.Settings;

public partial class CloudSettingsPage
{
    public CloudSettingsPage()
    {
        DataContext = App.Current.Services.GetService<CloudSettingsViewModel>();

        InitializeComponent();
    }
}