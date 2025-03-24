using System.Collections.Generic;
using GameDefine;

public class BottleData
{
    public bool IsFinish { get; set; }
    public bool IsFreeze { get; set; }
    public bool IsClearHide { get; set; }
    public bool IsNearHide { get; set; }
    public bool IsPlayAnim { get; set; }
    public bool IsSelect { get; set; }
    public bool IsClearHideAnim { get; set; }

    public List<int> Waters { get; set; }
    public List<bool> HideWaters { get; set; }
    public List<WaterItem> WaterItems { get; set; }
    public int MaxNum { get; set; }
    public int LimitColor { get; set; }
    public int UnlockClear { get; set; }
    public int BottleIdx { get; set; }  

    public int TopIdx => Waters.Count - 1;

    public void Initialize(LevelCreateCtrl.BottleProperty property, int idx)
    {
        Waters = new List<int>(property.waterSet);  
        HideWaters = new List<bool>(property.isHide);
        WaterItems = new List<WaterItem>(property.waterItem);
        IsClearHide = property.isClearHide;
        IsFreeze = property.isFreeze;
        UnlockClear = property.lockType;
        LimitColor = property.limitColor;
        BottleIdx = idx;
        IsFinish = false;
        IsSelect = false;
        IsPlayAnim = false;
        IsClearHideAnim = false;
        IsNearHide = false;
        MaxNum = 4;
    }
}
