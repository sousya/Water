using System.ComponentModel;

public enum EWaterOutAnimation
{
    [Description("BottleOut")]
    WaterLeftNone,
    [Description("BottleOut1")]
    WaterLeftOne,
    [Description("BottleOut2")]
    WaterLeftTwo,
    [Description("BottleOut3")]
    WaterLeftThree
}

// 入场扰动动画
public enum ESpinWaitAnimName
{
    [Description("ruchanghuangdong_cl")]
    ANIM_CL = 1,
    [Description("ruchanghuangdong_dh")]
    ANIM_DH = 2,
    [Description("ruchanghuangdong_fh")]
    ANIM_FH = 3,
    [Description("ruchanghuangdong_gl")]
    ANIM_GL = 4,
    [Description("ruchanghuangdong_hl")]
    ANIM_HL = 5,
    [Description("ruchanghuangdong_hs")]
    ANIM_HS = 6,
    [Description("ruchanghuangdong_jh")]
    ANIM_JH = 7,
    [Description("ruchanghuangdong_lh")]
    ANIM_LH = 8,
    [Description("ruchanghuangdong_sl")]
    ANIM_SL = 9,
    [Description("ruchanghuangdong_zh")]
    ANIM_ZE = 10,
    [Description("ruchanghuangdong_zs")]
    ANIM_ZS = 11,
    [Description("ruchanghuangdong_mh")]
    ANIM_MH = 12,
    
    ANIM_MAX = 13
}

public enum ESpineWaterOutAnimName
{
    [Description("daoshui_cl")]
    ANIM_CL = 1,
    [Description("daoshui_dh")]
    ANIM_DH = 2,
    [Description("daoshui_fh")]
    ANIM_FH = 3,
    [Description("daoshui_gl")]
    ANIM_GL = 4,
    [Description("daoshui_hl")]
    ANIM_HL = 5,
    [Description("daoshui_hs")]
    ANIM_HS = 6,
    [Description("daoshui_jh")]
    ANIM_JH = 7,
    [Description("daoshui_lh")]
    ANIM_LH = 8,
    [Description("daoshui_sl")]
    ANIM_SL = 9,
    [Description("daoshui_zh")]
    ANIM_ZE = 10,
    [Description("daoshui_zs")]
    ANIM_ZS = 11,
    [Description("daoshui_mh")]
    ANIM_MH = 12,
    
    ANIM_MAX = 13
}

public enum EClearHideAnim
{
    [Description("idle_cl")]
    IDLE_CL = 1,
    [Description("idle_dh")]
    IDLE_DH = 2,
    [Description("idle_fh")]
    IDLE_FH = 3,
    [Description("idle_gl")]
    IDLE_GL = 4,
    [Description("idle_hl")]
    IDLE_HL = 5,
    [Description("idle_hs")]
    IDLE_HS = 6,
    [Description("idle_jh")]
    IDLE_JH = 7,
    [Description("idle_lh")]
    IDLE_LH = 8,
    [Description("idle_sl")]
    IDLE_SL = 9,
    [Description("idle_ze")]
    IDLE_ZE = 10,
    [Description("idle_zs")]
    IDLE_ZS = 11,
    [Description("idle_mh")]
    IDLE_MH = 12,
    IDLE_MAX = 13
}

public class DATA
{
    public static string GetDescription<T>(T value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : value.ToString();
    }
}
