// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using One.Core.ExtensionMethods;
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

    public ObservableCollection<UsefullImageInfo> ObImageListInfo { get; set; } = new ObservableCollection<UsefullImageInfo>();

    async void InitData()
    {
        ShowLocalImage();
        var a = await GetImageInfo();

        var b = FilterImage(a);

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

        var temp = Directory.GetFiles(PathHelper.imagePath);

        foreach (var item in temp)
        {
            UsefullImageInfo showInfo = new UsefullImageInfo();

            showInfo.LocalImageName = System.IO.Path.GetFileNameWithoutExtension(item);
            showInfo.LocalImagePath = item;

            ObImageListInfo.Add(showInfo);
        }
    }

    private static async Task<BingImageModel> GetImageInfo()
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
        var timeline = await client.GetAsync<BingImageModel>(request);

        return timeline;
    }

    private List<UsefullImageInfo> FilterImage(BingImageModel bingImageModel)
    {
        List<UsefullImageInfo> strings = new List<UsefullImageInfo>();

        foreach (var item in bingImageModel.images)
        {
            UsefullImageInfo usefullImageInfo = new UsefullImageInfo();

            usefullImageInfo.DownloadUrl = "http://cn.bing.com" + item.url;
            usefullImageInfo.Copyright = item.copyright;
            usefullImageInfo.Title = item.title;
            usefullImageInfo.LocalImageName = item.fullstartdate;

            usefullImageInfo.LocalImagePath = PathHelper.imagePath + item.fullstartdate + ".jpg";

            strings.Add(usefullImageInfo);
        }

        return strings;
    }

    private async Task DownloadImage(UsefullImageInfo usefullImageInfos)
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