using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Unity.VisualScripting;

public class RewardItemManager : MonoSingleton<RewardItemManager>
{
    public SimpleObjectPool<Image> RewardPool;
    private RectTransform mRectTransformPar;

    private List<int> availableSlots;
    // 每轮动态传入
    private int slotCount;

    private const int X_MIN = -300;
    private const int X_MAX = 300;
    private const int Y_MIN = 700;
    private const int Y_MAX = 850;

    public override void OnSingletonInit()
    {
        mRectTransformPar = new GameObject("PropNodePar", typeof(RectTransform))
            .GetComponent<RectTransform>();
        mRectTransformPar.SetParent(this.transform, false);
        mRectTransformPar.anchorMin = Vector2.zero;
        mRectTransformPar.anchorMax = Vector2.one;
        mRectTransformPar.offsetMin = Vector2.zero;
        mRectTransformPar.offsetMax = Vector2.zero;

        RewardPool = new SimpleObjectPool<Image>(
        () =>
        {
            var par = Resources.Load("Prefab/PropPoolNode");
            var image = Instantiate(par, mRectTransformPar).GetComponent<Image>();
            return image;

        },
        (Image img)=> 
        {
            img.Hide();
            img.rectTransform.localPosition = Vector3.zero;
            img.rectTransform.localScale = Vector3.one;
        },
        initCount: 10);
    }

    /// <summary>
    /// 播放奖励道具的飞出动画
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="itemID"></param>
    /// <param name="itemNum"></param>
    /// <returns>返回道具飞出屏外的方法(用于间隔飞行)</returns>
    /// 调用前先调用 PrepareSlotLayout 设置本轮道具数量
    public System.Action PlayRewardInit(Sprite sprite ,int itemID ,int itemNum)
    {
        var image = RewardPool.Allocate();
        image.TryGetComponent(out PropRewardPoolNode _node);
        if (_node == null)
            _node = image.gameObject.AddComponent<PropRewardPoolNode>();
        _node.Init(sprite, SetRandomScreenPosition(image), itemID, itemNum);
        return ()=> _node.MoveOffScreen();
    }

    /// <summary>
    /// 设置本轮道具数量
    /// </summary>
    /// <param name="count"></param>
    public void PrepareSlotLayout(int count)
    {
        availableSlots = new List<int>();
        slotCount = count;
        availableSlots.Clear();
        for (int i = 0; i < slotCount; i++)
            availableSlots.Add(i);
    }

    private Vector2 SetRandomScreenPosition(Image propImage)
    {
        if (availableSlots.Count == 0)
        {
            Debug.LogWarning("先调用 PrepareSlotLayout！");
            return Vector2.zero;
        }

        float slotWidth = (X_MAX - X_MIN) / slotCount;

        // 抽一个槽
        int slotIndex = availableSlots[Random.Range(0, availableSlots.Count)];
        availableSlots.Remove(slotIndex);

        float slotXStart = X_MIN + slotWidth * slotIndex;
        float randX = Random.Range(slotXStart, slotXStart + slotWidth);
        float randY = Random.Range(Y_MIN, Y_MAX);

        return new Vector2(randX, randY);
    }
}
