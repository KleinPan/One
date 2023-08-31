// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels;

namespace One.Toolbox.Views.Pages;

public partial class DashboardPage
{
    public DashboardViewModel ViewModel { get; }

    public DashboardPage()
    {
        DataContext = ViewModel = App.Current.Services.GetService<DashboardViewModel>();

        InitializeComponent();
    }
}