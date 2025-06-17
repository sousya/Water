using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

[MonoSingletonPath("[Avatar]/AvatarManager")]
public class AvatarManager : MonoSingleton<AvatarManager> ,ICanRegisterEvent, ICanGetUtility
{
    private Dictionary<int, Sprite> mAvatarSprites;
    private Dictionary<int, Sprite> mAvatarFrameSprites;

    private const string AVATAR_SIGN = "g_WaterAvatar";
    private const string AVATAR_FRAME_SIGN = "g_WaterAvatarFrame";

    private SaveDataUtility mStroge;

    public override void OnSingletonInit()
    {
        mStroge = this.GetUtility<SaveDataUtility>();
        mAvatarSprites = new Dictionary<int, Sprite>();
        mAvatarFrameSprites = new Dictionary<int, Sprite>();

        LoadAllSprites("Sprites/AvatarPics/Avatars", mAvatarSprites);
        LoadAllSprites("Sprites/AvatarPics/AvatarFrames", mAvatarFrameSprites);

        this.RegisterEvent<AvatarEvent>(e =>
        {
            SetAvatar(e.AvatarId , AVATAR_SIGN);
            SetAvatar(e.AvatarFrameId, AVATAR_FRAME_SIGN);
        });
    }

    public Sprite GetAvatarSprite(bool isAvatar,int id)
    {
        if (isAvatar)
            return mAvatarSprites[id];
        return mAvatarFrameSprites[id];
    }

    /// <summary>
    /// 初始化调用
    /// </summary>
    /// <param name="isAvatar"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public Sprite GetAvatarSprite(bool isAvatar)
    {
        if (isAvatar)
            return mAvatarSprites[mStroge.LoadIntValue(AVATAR_SIGN, 0)];
        return mAvatarFrameSprites[mStroge.LoadIntValue(AVATAR_FRAME_SIGN, 0)];
    }

    public int GetAvatarId(bool isAvatar)
    {
        if (isAvatar)
            return mStroge.LoadIntValue(AVATAR_SIGN, 0);
        return mStroge.LoadIntValue(AVATAR_FRAME_SIGN, 0);
    }

    private void SetAvatar(int id, string sign)
    {
        mStroge.SaveInt(sign, id);
    }

    private void LoadAllSprites(string path, Dictionary<int, Sprite> spritesDic)
    {
        Sprite[] _sprites = Resources.LoadAll<Sprite>(path);

        for (int i = 0; i < _sprites.Length; i++)
        {
            spritesDic[i] = _sprites[i];
        }
    }

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }
}
