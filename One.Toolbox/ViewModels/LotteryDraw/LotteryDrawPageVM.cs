﻿using One.Toolbox.ViewModels.Base;

using System.Collections.ObjectModel;

namespace One.Toolbox.ViewModels.LotteryDraw;

public partial class LotteryDrawPageVM : BaseVM
{
    public LotteryDrawPageVM()
    {
    }

    public override void OnNavigatedEnter()
    {
        base.OnNavigatedEnter();
        InitData();
    }

    // public List<int> ListAngle { get; set; } = new List<int>();
    public ObservableCollection<LotteryDrawModel> MenuArray { get; set; } = new ObservableCollection<LotteryDrawModel>();

    void InitData()
    {
        //ListAngle.Clear();
        MenuArray.Clear();
        var angle = 0;
        var anglePrize = 2000;

        int count = 6;
        int every = 360 / (count);
        for (int i = 0; i < count; i++)
        {
            var prizeTitle = i == 0 ? "谢谢参与" : $"{i}等奖";
            angle += every;
            anglePrize += every;
            //ListAngle.Add(anglePrize);
            MenuArray.Add(new LotteryDrawModel { Angle = every, Title = prizeTitle, StartAngle = angle });
        }
    }
}