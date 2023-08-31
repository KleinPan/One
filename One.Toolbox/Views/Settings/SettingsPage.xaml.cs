// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels;

using System.Diagnostics;
using System.Windows.Navigation;

namespace One.Toolbox.Views.Settings;

/// <summary> Interaction logic for SettingsPage.xaml </summary>
public partial class SettingsPage
{
    public SettingsPage()
    {
        DataContext = App.Current.Services.GetService<SettingsViewModel>();

        InitializeComponent();
    }

    private static bool loaded = false;

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        if (loaded)
            return;
        loaded = true;

        //设置为手动检查
        AutoUpdaterDotNET.AutoUpdater.CheckForUpdateEvent += checkUpdateEvent;
        checkUpdate();
    }

    private AutoUpdaterDotNET.UpdateInfoEventArgs G_args;

    private void checkUpdateEvent(AutoUpdaterDotNET.UpdateInfoEventArgs args)
    {
        if (args != null)
        {
            G_args = args;
            if (args.IsUpdateAvailable)
            {
                Tools.Global.HasNewVersion = !Tools.Global.IsMSIX();
                if (Tools.Global.IsMSIX())
                {
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        CheckUpdateButton.Content = "检测到有更新，请前往微软商店更新";
                    }));
                }
                else if (Tools.Global.setting.autoUpdate)
                {
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        CheckUpdateButton.Content = "检测到有更新，获取中";
                        AutoUpdaterDotNET.AutoUpdater.ShowUpdateForm(G_args);
                    }));
                }
                else
                {
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        CheckUpdateButton.IsEnabled = true;
                        CheckUpdateButton.Content = "检测到有更新，立即更新";
                    }));
                }
            }
            else
            {
                this.Dispatcher.Invoke(new Action(delegate
                {
                    CheckUpdateButton.Content = "已是最新版，无需更新";
                }));
            }
        }
        else
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                CheckUpdateButton.Content = "检查更新失败，请检查网络";
            }));
        }
    }

    private void checkUpdate()
    {
        try
        {
            Random r = new Random();//加上随机参数，确保获取的是最新数据
            this.Dispatcher.Invoke(new Action(delegate
            {
                AutoUpdaterDotNET.AutoUpdater.Start("https://One.Toolbox.papapoi.com/autoUpdate.xml?" + r);
            }));
        }
        catch
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                CheckUpdateButton.Content = "检查更新失败，请检查网络";
            }));
        }
    }

    private void NewissueButton_Click(object sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://github.com/KleinPan/One.Toolbox/issues") { UseShellExecute = true });
    }

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
    }

    private void OpenSourceButton_Click(object sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://github.com/KleinPan/One.Toolbox") { UseShellExecute = true });
    }

    private void CheckUpdateButton_Click(object sender, RoutedEventArgs e)
    {
        CheckUpdateButton.IsEnabled = false;
        CheckUpdateButton.Content = "获取更新信息中";
        AutoUpdaterDotNET.AutoUpdater.ShowUpdateForm(G_args);
    }
}