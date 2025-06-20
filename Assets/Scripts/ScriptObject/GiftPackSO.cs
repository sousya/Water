using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

[CreateAssetMenu(fileName = "GiftPack", menuName = "Game/Gift Pack")]
public class GiftPackSO : ScriptableObject, IPackSoInterface
{
    //�����������������޹��Ľ����ŵ�SpecialReward��
    //���Կ��ǰɾ���ŵ����У�����Ҫ�ⲿά��
    [Header("����ID")]
    [SerializeField] private string PackID;
    [Header("�������")]
    [SerializeField] private int coins;
    [SerializeField] private List<ItemReward> items;
    [Header("��������(��λ������)")]
    [SerializeField] private int unlimitedHp;
    [Header("�޹��")]
    [SerializeField] private bool removeAds;

    public int Coins => coins;
    public int UnlimitedHp => unlimitedHp;
    public bool RemoveAds => removeAds;
    public string ID => PackID;
    public IReadOnlyList<ItemReward> ItemReward => items;
    private List<SpecialReward> emptySpeciaRewards = new();


    //�ӿ�ʵ��
    IReadOnlyList<SpecialReward> IPackSoInterface.SpecialRewards => emptySpeciaRewards;
    IReadOnlyList<ItemReward> IPackSoInterface.ItemReward => items;
}

[System.Serializable]
public class ItemReward
{
    [Tooltip("����������1~8")]
    [Range(1, 8)]
    [SerializeField] private int itemIndex;

    [Tooltip("��������")]
    [SerializeField] private int quantity;

    [SerializeField] private Sprite rewardSprite;

    // ֻ������
    public int ItemIndex => itemIndex;
    public int Quantity => quantity;
    public Sprite RewardSprite => rewardSprite;
}