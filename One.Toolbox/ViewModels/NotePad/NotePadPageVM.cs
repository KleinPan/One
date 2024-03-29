﻿using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.Helpers;
using One.Toolbox.Messenger;
using One.Toolbox.Services;
using One.Toolbox.ViewModels.Base;
using One.Toolbox.ViewModels.Setting;

using System.Collections.ObjectModel;
using System.IO;

namespace One.Toolbox.ViewModels.NotePad;

public partial class NotePadPageVM : BaseVM
{
    [ObservableProperty]
    private EditFileInfoVM selectedEditFileInfo;

    public ObservableCollection<EditFileInfoVM> EditFileInfoViewModelOC { get; set; } = new ObservableCollection<EditFileInfoVM>();

    public NotePadPageVM()
    {
        // Register a message in some module
        WeakReferenceMessenger.Default.Register<CloseMessage>(this, (r, m) =>
        {
            // Handle the message here, with r being the recipient and m being the input message. Using the recipient passed as input makes it so that the lambda expression doesn't capture "this", improving performance.

            SaveSetting();
        });

        InitData();
    }

    public override void OnNavigatedEnter()
    {
        base.OnNavigatedEnter();
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

    #region Command

    [RelayCommand]
    private void NewFile()
    {
        int index = EditFileInfoViewModelOC.Count;
        string filePath = PathHelper.notePath + "unfitled" + index + ".txt";
        EditFileInfoVM editFileInfoViewModel = new EditFileInfoVM(filePath);
        var res = editFileInfoViewModel.CreateNewFile();
        if (res)
        {
            EditFileInfoViewModelOC.Add(editFileInfoViewModel);
            SaveSetting();
        }
        else
        {
            MessageShowHelper.ShowWarnMessage("File already exist!");
        }
    }

    [RelayCommand]
    private void OnSelectedEditFileChanged(System.Windows.Controls.SelectionChangedEventArgs args)
    {
        var newItems = args.AddedItems;

        if (newItems.Count == 1)
        {
            var item = newItems[0] as EditFileInfoVM;
            item.LoadDocument();
        }
        else
        {
        }
    }

    [RelayCommand]
    private void DeleteFile()
    {
        if (SelectedEditFileInfo != null)
        {
            if (File.Exists(SelectedEditFileInfo.FilePath))
            {
                File.Delete(SelectedEditFileInfo.FilePath);
            }

            EditFileInfoViewModelOC.Remove(SelectedEditFileInfo);

            SaveSetting();
        }
    }

    #endregion Command

    #region Setting

    public void SaveSetting()
    {
        var service = App.Current.Services.GetService<SettingService>();

        service.AllConfig.EditFileInfoList.Clear();
        foreach (var item in EditFileInfoViewModelOC)
        {
            EditFileInfo editFileInfo = new()
            {
                FilePath = item.FilePath,
                FileName = item.FileName,
                CreateTime = item.CreateTime,
                ModifyTime = item.ModifyTime,
            };
            item.SaveDocument();

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
            EditFileInfoVM editFileInfo = new(item.FilePath);
            editFileInfo.CreateTime = item.CreateTime;
            editFileInfo.ModifyTime = item.ModifyTime;
            editFileInfo.UpdateInfoAction += Update;

            EditFileInfoViewModelOC.Add(editFileInfo);
        }
    }

    private void Update()
    {
        SaveSetting();
    }

    #endregion Setting
}