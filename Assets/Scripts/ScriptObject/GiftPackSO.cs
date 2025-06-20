using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

[CreateAssetMenu(fileName = "GiftPack", menuName = "Game/Gift Pack")]
public class GiftPackSO : ScriptableObject, IPackSoInterface
{
    //后续吧无限体力和无广告的奖励放到SpecialReward中
    //可以考虑吧精灵放到表中，不需要外部维护
    [Header("道具ID")]
    [SerializeField] private string PackID;
    [Header("礼包内容")]
    [SerializeField] private int coins;
    [SerializeField] private List<ItemReward> items;
    [Header("无限体力(单位：分钟)")]
    [SerializeField] private int unlimitedHp;
    [Header("无广告")]
    [SerializeField] private bool removeAds;

    public int Coins => coins;
    public int UnlimitedHp => unlimitedHp;
    public bool RemoveAds => removeAds;
    public string ID => PackID;
    public IReadOnlyList<ItemReward> ItemReward => items;
    private List<SpecialReward> emptySpeciaRewards = new();


    //接口实现
    IReadOnlyList<SpecialReward> IPackSoInterface.SpecialRewards => emptySpeciaRewards;
    IReadOnlyList<ItemReward> IPackSoInterface.ItemReward => items;
}

[System.Serializable]
public class ItemReward
{
    [Tooltip("道具索引，1~8")]
    [Range(1, 8)]
    [SerializeField] private int itemIndex;

    [Tooltip("道具数量")]
    [SerializeField] private int quantity;

    [SerializeField] private Sprite rewardSprite;

    // 只读属性
    public int ItemIndex => itemIndex;
    public int Quantity => quantity;
    public Sprite RewardSprite => rewardSprite;
}