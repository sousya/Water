using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Unity.VisualScripting;

public class RewardItemManager : MonoSingleton<RewardItemManager>
{
    [SerializeField] private Sprite[] RewardSprites;
    [SerializeField] private Animator BoxAnimator;
    [SerializeField] private Button BtnContinue;
    [SerializeField] private RectTransform mRectTransformPar;
    [SerializeField] private ParticleTargetMoveCtrl CoinParticle;
    public SimpleObjectPool<Image> RewardPool;

    private RectTransform mMask;

    private List<int> availableSlots;
    private List<System.Action> actionList;
    // 每轮动态传入
    private int slotCount;
    private bool addCoin;
    private const int YAXIS = 800;

    public override void OnSingletonInit()
    {
        mMask = BoxAnimator.transform.parent.GetComponent<RectTransform>();

        actionList = new List<System.Action>();
        availableSlots = new List<int>();

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

        BtnContinue.onClick.AddListener(() =>
        {
            StartCoroutine(ContinueClickEvent());
        });
    }

    public IEnumerator PlayRewardAnim(GiftPackSO packSO ,bool addCoin = false)
    {
        this.addCoin = addCoin;
        mMask.Show();

        slotCount = packSO.ItemReward.Count;
        availableSlots.Clear();
        actionList.Clear();
        for (int i = 0; i < slotCount; i++)
            availableSlots.Add(i);

        BoxAnimator.Show();

        BoxAnimator.Play("BoxOpen");
        yield return new WaitForSeconds(1f); // 等待盒子打开动画完成
        BtnContinue.Show();
        BoxAnimator.Hide();

        foreach (var item in packSO.ItemReward)
        {
            var image = RewardPool.Allocate();
            image.TryGetComponent(out PropRewardPoolNode _node);
            if (_node == null)
                _node = image.gameObject.AddComponent<PropRewardPoolNode>();
            _node.Init(RewardSprites[item.ItemIndex - 1], SetRandomScreenPosition(image), item.ItemIndex, item.Quantity);
            actionList.Add(() => _node.MoveOffScreen());
        }
    }

    private IEnumerator ContinueClickEvent()
    {
        foreach (var item in actionList)
        {
            item?.Invoke();
            yield return new WaitForSeconds(0.2f);
        }

        BtnContinue.Hide();
        mMask.Hide();

        if (addCoin)
            CoinParticle.Play(100);
    }

    private Vector2 SetRandomScreenPosition(Image propImage)
    {
        if (availableSlots.Count == 0)
        {
            Debug.LogWarning("槽位用尽，请先调用 PrepareSlotLayout！");
            return Vector2.zero;
        }

        // 抽一个槽位索引
        int slotIndex = availableSlots[Random.Range(0, availableSlots.Count)];
        availableSlots.Remove(slotIndex);

        // 每个道具间隔 200，整体居中
        float spacing = 200f;
        float x = slotIndex * spacing - (slotCount - 1) * spacing * 0.5f;

        // 超过5个道具分两排补充位置

        return new Vector2(x, YAXIS);
    }
}
