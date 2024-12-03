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

    public CakeCtrl from;

    public int idx;
}