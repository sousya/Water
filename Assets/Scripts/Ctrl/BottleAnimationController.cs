using UnityEngine;
using System.Collections;

public class BottleAnimationController : MonoBehaviour
{
    private BottleCtrl _bottleCtrl;
    public Animator BottleAnim { get; private set; }
    public Animator FillWaterGoAnim { get; private set; }
    public SkeletonGraphic Spine { get; private set; }
    
    public void Initialize(BottleCtrl bottleCtrl)
    {
        _bottleCtrl = bottleCtrl;
        // 初始化动画组件
    }
    
    public void PlayFillAnim(int num, int color)
    {
        StartCoroutine(CoroutinePlayFillAnim(num, color));
    }
    
    public void PlayOutAnim(int num, int useIdx, int useColor)
    {
        StartCoroutine(CoroutinePlayOutAnim(num, useIdx, useColor));
    }
    
    // ... 其他动画相关方法
} 