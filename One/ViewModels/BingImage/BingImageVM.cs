using One.Core.ExtensionMethods;
using One.Core.Helpers;
using One.Toolbox.Helpers;
using One.Toolbox.ViewModels.Base;

using RestSharp;

using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace One.Toolbox.ViewModels.BingImage;

public partial class BingImageVM : BaseVM
{
    public BingImageVM()
    {
    }

    public override void OnNavigatedEnter()
    {
        base.OnNavigatedEnter();
        InitData();
    }

    [ObservableProperty]
    private ObservableCollection<UsefullImageInfoVM> obImageListInfo = new ObservableCollection<UsefullImageInfoVM>();

    private List<UsefullImageInfoVM> ImageList = new List<UsefullImageInfoVM>();
    //public ObservableCollection<UsefullImageInfoViewModel> ObImageListInfo { get; set; } = new ObservableCollection<UsefullImageInfoViewModel>();

    async void InitData()
    {
        ShowLocalImage();
        var a = await GetImageInfo();

        var b = FilterImageInfoAndSave(a);

        ImageList.Clear();
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
                ImageList.Add(item);
            }
        }

        ObImageListInfo.AddRange(ImageList.OrderByDescending(x => x.LocalImageName));
    }

    private void ShowLocalImage()
    {
        ObImageListInfo.Clear();

        var temp = Directory.GetFiles(Helpers.PathHelper.imagePath);
        var temp2 = temp.Where(x => x.EndsWith("jpg"));
        var temp3 = temp2.Reverse();
        foreach (var item in temp3)
        {
            UsefullImageInfoVM showInfo = new UsefullImageInfoVM();

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

    private List<UsefullImageInfoVM> FilterImageInfoAndSave(BingImageOriginalModel bingImageModel)
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
        List<UsefullImageInfoVM> listVM = new List<UsefullImageInfoVM>();
        foreach (var item in list)
        {
            listVM.Add(item.ToVM());
        }
        return listVM;
    }

    private async Task DownloadImage(UsefullImageInfoVM usefullImageInfos)
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