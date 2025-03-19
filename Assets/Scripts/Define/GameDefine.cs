using System.ComponentModel;
using GameAttributes;
using Unity.VisualScripting;

namespace GameDefine
{
    public static class GameConst
    {
        public const int MaxVitality = 5;
        public const int RecoveryTime = 1800;
    }
    public enum GameType
    {
        Normal = 0,
        Bomb = 1,
        Count = 2,
        Step = 3,
        Hide = 4,
    }

    public enum WaterItem
    {
        None = 0,
        Ice = 1,
        BreakIce = 2,
        Bomb = 3,
    }

    public enum BottleType
    {
        None = 0,
        ClearShow = 1,
        NearShow = 2
    }

    public enum ItemType
    {
        [WaterColorState(false, false, false, false, "", EColorStateSpineType.None)]
        UseColor = 1,
        
        [WaterColorState(true, false, false, false, "idle_cl", EColorStateSpineType.EBroomSpine)]
        ClearItem = 1001,
        
        [WaterColorState(false, false, false, true, "idle", EColorStateSpineType.EMagnetSpine)]
        MagnetItem = 1002, 
        
        [WaterColorState(false, true, false, false, "idle", EColorStateSpineType.ECreateSpine)]
        MakeColorItem = 1003,
        
        [WaterColorState(false, false, true, false, "idle_cl", EColorStateSpineType.EChangeSpine)]
        ChangeGreen = 2001,
        
        [WaterColorState(false, false, true, false, "idle_jh", EColorStateSpineType.EChangeSpine)]
        ChangeOrange = 2002,
        
        [WaterColorState(false, false, true, false, "idle_fs", EColorStateSpineType.EChangeSpine)]
        ChangePink = 2003,
        
        [WaterColorState(false, false, true, false, "idle_zs", EColorStateSpineType.EChangeSpine)]
        ChangePurple = 2004,
        
        [WaterColorState(false, false, true, false, "idle_hs", EColorStateSpineType.EChangeSpine)]
        ChangeYellow = 2005,
        
        [WaterColorState(false, false, true, false, "idle_sl", EColorStateSpineType.EChangeSpine)]
        ChangeDarkBlue = 2006,
        
        [WaterColorState(true, false, false, false, "idle_fh", EColorStateSpineType.EBroomSpine)]
        ClearPink = 3001,
        
        [WaterColorState(true, false, false, false, "idle_jh", EColorStateSpineType.EBroomSpine)]
        ClearOrange = 3002,
        
        [WaterColorState(true, false, false, false, "idle_gl", EColorStateSpineType.EBroomSpine)]
        ClearBlue = 3003,
        
        [WaterColorState(true, false, false, false, "idle_hs", EColorStateSpineType.EBroomSpine)]
        ClearYellow = 3004,
        
        [WaterColorState(true, false, false, false, "idle_sl", EColorStateSpineType.EBroomSpine)]
        ClearDarkGreen = 3005,
        
        [WaterColorState(true, false, false, false, "idle_dh", EColorStateSpineType.EBroomSpine)]
        ClearRed = 3006,
        
        [WaterColorState(true, false, false, false, "idle_cl", EColorStateSpineType.EBroomSpine)]
        ClearGreen = 3007,
    }
    public enum LanguageType
    {
        zh = 0,
        ja = 1,
        en = 2,
        ko = 3,
    }
    
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

    public enum EDisappearAnim
    {
        [Description("disapear_cl")]
        DISAPPEAR_CL = 1,
        [Description("disapear_dh")]
        DISAPPEAR_DH = 2,
        [Description("idledisapear_fh")]
        DISAPPEAR_FH = 3,
        [Description("disapear_gl")]
        DISAPPEAR_GL = 4,
        [Description("disapear_hl")]
        DISAPPEAR_HL = 5,
        [Description("disapear_hs")]
        DISAPPEAR_HS = 6,
        [Description("disapear_jh")]
        DISAPPEAR_JH = 7,
        [Description("disapear_lh")]
        DISAPPEAR_LH = 8,
        [Description("disapear_sl")]
        DISAPPEAR_SL = 9,
        [Description("disapear_ze")]
        DISAPPEAR_ZE = 10,
        [Description("disapear_zs")]
        DISAPPEAR_ZS = 11,
        [Description("disapear_mh")]
        DISAPPEAR_MH = 12,
        DISAPPEAR_MAX = 13
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
}
