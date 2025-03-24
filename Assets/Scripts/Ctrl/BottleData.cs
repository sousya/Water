public class BottleData
{
    public bool IsFinish { get; set; }
    public bool IsFreeze { get; set; }
    public bool IsClearHide { get; set; }
    public bool IsNearHide { get; set; }
    public List<int> Waters { get; private set; } = new List<int>();
    public List<bool> HideWaters { get; private set; } = new List<bool>();
    public List<WaterItem> WaterItems { get; private set; } = new List<WaterItem>();
    public int MaxNum { get; private set; } = 4;
    public int LimitColor { get; set; }
    public int UnlockClear { get; set; }
    public int BottleIdx { get; set; }

    public int TopIdx => Waters.Count - 1;
    
    public void Initialize(BottleProperty property)
    {
        Waters = new List<int>(property.waterSet);
        HideWaters = new List<bool>(property.isHide);
        WaterItems = new List<WaterItem>(property.waterItem);
        IsClearHide = property.isClearHide;
        IsNearHide = property.isNearHide;
        IsFreeze = property.isFreeze;
        UnlockClear = property.lockType;
        LimitColor = property.limitColor;
    }
} 