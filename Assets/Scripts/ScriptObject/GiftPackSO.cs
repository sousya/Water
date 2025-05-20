using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

[CreateAssetMenu(fileName = "GiftPack", menuName = "Game/Gift Pack")]
public class GiftPackSO : ScriptableObject
{
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
    public IReadOnlyList<ItemReward> Items => items;
    public int UnlimitedHp => unlimitedHp;
    public bool RemoveAds => removeAds;
    public string ID => PackID;
}

[System.Serializable]
public class ItemReward
{
    [Tooltip("道具索引，1~8")]
    [Range(1, 8)]
    [SerializeField] private int itemIndex;

    [Tooltip("道具数量")]
    [SerializeField] private int quantity;

    // 只读属性
    public int ItemIndex => itemIndex;
    public int Quantity => quantity;
}