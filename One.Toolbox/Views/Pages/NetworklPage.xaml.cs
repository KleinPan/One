﻿using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels;

namespace One.Toolbox.Views.Pages
{
    public partial class NetworklPage
    {
        public ViewModels.NetworkViewModel ViewModel { get; }

        public NetworklPage()
        {
            DataContext = ViewModel = App.Current.Services.GetService<NetworkViewModel>();

            InitializeComponent();
        }
    }
}