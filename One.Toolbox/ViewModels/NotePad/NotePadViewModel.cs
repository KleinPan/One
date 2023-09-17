// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Utils;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

using One.Toolbox.Models.Setting;
using One.Toolbox.Services;
using One.Toolbox.ViewModels.Base;
using One.Toolbox.ViewModels.NotePad;

using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace One.Toolbox.ViewModels;

public partial class NotePadViewModel : BaseViewModel
{
    [ObservableProperty]
    private EditFileInfoViewModel selectedEditFileInfo;

    public ObservableCollection<EditFileInfoViewModel> EditFileInfoViewModelOC { get; set; } = new ObservableCollection<EditFileInfoViewModel>();

    public NotePadViewModel()
    {
    }

    public override void OnNavigatedEnter()
    {
        base.OnNavigatedEnter();
        InitData();
    }

    void InitData()
    {
        LoadSetting();
    }

    public override void OnNavigatedLeave()
    {
        base.OnNavigatedLeave();

        SaveSetting();
    }

    [RelayCommand]
    private void NewFile()
    {
        EditFileInfoViewModel editFileInfoViewModel = new EditFileInfoViewModel();
        editFileInfoViewModel.CreateNewFile();

        EditFileInfoViewModelOC.Add(editFileInfoViewModel);
    }

    [RelayCommand]
    private void OnSelectedEditFileChanged(System.Windows.Controls.SelectionChangedEventArgs args)
    {
        var newItems = args.AddedItems;

        if (newItems.Count == 1)
        {
            var item = newItems[0] as EditFileInfoViewModel;
            item.LoadDocument();
        }
        else
        {
        }
    }

    public void SaveSetting()
    {
        var service = App.Current.Services.GetService<SettingService>();

        foreach (var item in EditFileInfoViewModelOC)
        {
            EditFileInfo editFileInfo = new()
            {
                FilePath = item.FilePath,
            };
            service.AllConfig.EditFileInfoList.Add(editFileInfo);
        }

        service.Save();
    }

    public void LoadSetting()
    {
        EditFileInfoViewModelOC.Clear();

        var service = App.Current.Services.GetService<SettingService>();

        foreach (var item in service.AllConfig.EditFileInfoList)
        {
            EditFileInfoViewModel editFileInfo = new()
            {
                FilePath = item.FilePath,
            };
            EditFileInfoViewModelOC.Add(editFileInfo);
        }
    }
}