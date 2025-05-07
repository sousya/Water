using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UI;
using GameAttributes;

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

    // public enum ItemType
    // {
    //     ClearItem = 1001,       // ���Բ���Ч��
    //     MagnetItem = 1002,      // ħ���飬�������Debuff(�ϰ�������Ч��)
    //     MakeColorItem = 1003,   // ������ص���ɫ��ƿ����
    //     ChangeGreen = 2001,     // ��ĳ����ɫ��Ϊ��ɫ-���1
    //     ChangeOrange = 2002,    // ��ĳ����ɫ��Ϊ��ɫ-���7
    //     ChangePink = 2003,      // ��ĳ����ɫ��Ϊ��ɫ-���3
    //     ChangePurple = 2004,    // ��ĳ����ɫ��Ϊ��ɫ-���10
    //     ChangeYellow = 2005,    // ��ĳ����ɫ��Ϊ��ɫ-���6
    //     ChangeDarkBlue = 2006,  // ��ĳ����ɫ��Ϊ����ɫ-���4
    //     ClearPink = 3001,       // ������з�ɫˮ��-���3
    //     ClearOrange = 3002,     // ������г�ɫˮ��-���7
    //     ClearBlue = 3003,       // ���������ɫˮ��-���4
    //     ClearYellow = 3004,     // ������л�ɫˮ��-���6
    //     ClearDarkGreen = 3005,  // �����������ɫˮ-����9
    //     ClearRed = 3006,        // ������к�ɫˮ��-���2
    //     ClearGreen = 3007,      // ���������ɫˮ��-���1
    // }
    
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

    public enum EIdleAnim
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

    public class GameEnum
    {
        public static string GetDescription<T>(T value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
