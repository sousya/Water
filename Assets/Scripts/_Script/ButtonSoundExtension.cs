using QFramework;

/// <summary>
/// 通过编辑器添加的按钮扩展类，用于播放按钮点击音效
/// </summary>
/// 通过反射获取静态方法订阅
/// 不同按钮可以使用不同的音效
public static class ButtonSoundExtension
{
    public static void PlayButtonClickSound()
    {
        AudioKit.PlaySound("resources://Audio/BtnSound");
    }

    //可以添加更多的音效方法
    //public static void PlayButtonClickSound2()
    //{
    //    AudioKit.PlaySound($"resources://Audio/btnClick2");
    //}
}
