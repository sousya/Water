using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 刷新文本事件
/// </summary>
public struct RefreshUITextEvent
{

}

public struct LevelStartEvent
{
}

public struct UpdateReturnEvent
{

}
public struct TeachEvent
{
    public int step;
}
public struct TipEvent
{
    public string tipStr;
}

public struct LoadEnd
{

}

public struct BeginLevelEvent
{

}

public struct ShowChallengeEvent
{

}

public struct ShowJigsawEvent
{

}

public struct RefreshUnlockLevel
{

}

public struct LevelClearEvent
{

}

public struct ShowOrderEnd
{

}

public struct MoveCakeEvent
{

}

public struct CakeUnlock
{
    public int keyType;

    public BottleCtrl from;

    public int idx;
}

public struct CoinChangeEvent
{
    public int coin;
}

public struct VitalityChangeEvent
{
}

public struct UnlimtItemEvent
{
}

public struct VitalityTimeChangeEvent
{
    public long timeOffset;
}

public struct UnlockSceneEvent
{
    public int scene;
    public int part;
}

public struct RewardSceneEvent
{
}

public struct ReturnMainEvent
{
}

public struct RefreshItemEvent
{
}
public struct GameStartEvent
{
}