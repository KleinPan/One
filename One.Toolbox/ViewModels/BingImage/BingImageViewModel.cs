// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using One.Core.ExtensionMethods;
using One.Core.Helpers;
using One.Toolbox.Helpers;
using One.Toolbox.ViewModels.Base;

using RestSharp;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace One.Toolbox.ViewModels.BingImage;

public partial class BingImageViewModel : BaseViewModel
{
    public BingImageViewModel()
    {
    }

    public override void OnNavigatedEnter()
    {
        base.OnNavigatedEnter();
        InitData();
    }

    public ObservableCollection<UsefullImageInfoViewModel> ObImageListInfo { get; set; } = new ObservableCollection<UsefullImageInfoViewModel>();

    async void InitData()
    {
        ShowLocalImage();
        var a = await GetImageInfo();

        var b = FilterImageInfoAndSave(a);

        foreach (var item in b)
        {
            await DownloadImage(item);

            var have = ObImageListInfo.ToList().FirstOrDefault(x => x.LocalImageName == item.LocalImageName);
            if (have != null)
            {
                have.DownloadUrl = "http://cn.bing.com" + item.DownloadUrl;
                have.Copyright = item.Copyright;
                have.Title = item.Title;
            }
            else
            {
                ObImageListInfo.Add(item);
            }
        }
    }

    private void ShowLocalImage()
    {
        ObImageListInfo.Clear();

        var temp = Directory.GetFiles(Helpers.PathHelper.imagePath);
        var temp2 = temp.Where(x => x.EndsWith("jpg"));
        foreach (var item in temp2)
        {
            UsefullImageInfoViewModel showInfo = new UsefullImageInfoViewModel();

            showInfo.LocalImageName = System.IO.Path.GetFileNameWithoutExtension(item);
            showInfo.LocalImagePath = item;

            ObImageListInfo.Add(showInfo);
        }
    }

    private static async Task<BingImageOriginalModel> GetImageInfo()
    {
        //获取图片api:http://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1
        //idx参数：指获取图片的时间，0（指获取当天图片），1（获取昨天照片），2（获取前天的图片），最多可获取8天前的照片。
        //n参数：从指定日期往前总共几张图片

        var options = new RestClientOptions("http://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=8")
        {
        };
        var client = new RestClient(options);

        var request = new RestRequest("");

        // The cancellation token comes from the caller. You can still make a call without it.
        var timeline = await client.GetAsync<BingImageOriginalModel>(request);

        return timeline;
    }

    private string ConfigPath = One.Toolbox.Helpers.PathHelper.imagePath + "ImageInfo.json";

    private List<UsefullImageInfoViewModel> FilterImageInfoAndSave(BingImageOriginalModel bingImageModel)
    {
        //Model
        List<UsefullImageInfoModel> list = new List<UsefullImageInfoModel>();
        try
        {
            list = IOHelper.Instance.ReadContentFromLocal<List<UsefullImageInfoModel>>(ConfigPath);
        }
        catch (Exception)
        {
        }

        foreach (var item in bingImageModel.images)
        {
            if (list.Any(x => x.LocalImageName == item.fullstartdate))
            {
                continue;
            }

            UsefullImageInfoModel usefullImageInfo = new UsefullImageInfoModel();

            usefullImageInfo.DownloadUrl = "http://cn.bing.com" + item.url;
            usefullImageInfo.Copyright = item.copyright;
            usefullImageInfo.Title = item.title;
            usefullImageInfo.LocalImageName = item.fullstartdate;

            usefullImageInfo.LocalImagePath = Helpers.PathHelper.imagePath + item.fullstartdate + ".jpg";

            list.Add(usefullImageInfo);
        }
        try
        {
            IOHelper.Instance.WriteContentTolocal(list, ConfigPath);
        }
        catch (Exception ex)
        {
            MessageShowHelper.ShowErrorMessage(ex.Message);
        }

        //VM
        List<UsefullImageInfoViewModel> listVM = new List<UsefullImageInfoViewModel>();
        foreach (var item in list)
        {
            listVM.Add(item.ToVM());
        }
        return listVM;
    }

    private async Task DownloadImage(UsefullImageInfoViewModel usefullImageInfos)
    {
        //查看图片是否已经下载，path为路径
        if (File.Exists(usefullImageInfos.LocalImagePath))
        {
            return;
        }

        var options = new RestClientOptions(usefullImageInfos.DownloadUrl)
        {
        };
        var client = new RestClient(options);

        var request = new RestRequest("");

        // The cancellation token comes from the caller. You can still make a call without it.
        var timeline = await client.DownloadDataAsync(request);

        if (timeline == null)
        {
            return;
        }
        //创造图片
        using (FileStream fileStream = new FileStream(usefullImageInfos.LocalImagePath, FileMode.Create))
        {
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);
            //写入图片信息
            binaryWriter.Write(timeline);
        }

        return;
    }
}