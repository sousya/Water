using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

public class RankNodePool : MonoSingleton<RankNodePool>, ICanGetModel
{
    private const string RankNodePrefabPath = "Prefab/RankNode";
    private const int MAX_RANK_COUNT = 30;
    private const int MAX_PAGE_RANK_COUNT = 10;   // ��ҳ���ڵ���(��Ϊ13,�߼���Ӱ��)

    private RankDataModel mRankDataModel;
    private List<GameObject> mTempRankFrontNodes; // ���ǰ�Ľڵ�
    private List<GameObject> mTempRankBackNodes;  // ��Һ�Ľڵ�
    private GameObject mPlayerNode;
    private Transform mRankNodeParent;

    private SimpleObjectPool<GameObject> mRankNodePool;

    [SerializeField] private Sprite mPlayerSprite;
    [SerializeField] private Sprite mDefaultSprite;
    [SerializeField] private Sprite mFirSprite;
    [SerializeField] private Sprite mSecSprite;
    [SerializeField] private Sprite mThiSprite;
    
    public override void OnSingletonInit()
    {
        mRankDataModel = this.GetModel<RankDataModel>();
        mTempRankFrontNodes = new();
        mTempRankBackNodes = new();
        mRankNodeParent = GetComponent<Transform>();

        mRankNodePool = new SimpleObjectPool<GameObject>(
        () =>
        {
            var obj = Resources.Load(RankNodePrefabPath);
            var node = Instantiate(obj, mRankNodeParent) as GameObject;
            node.Hide();
            return node;
        },
        (obj) =>
        {
            obj.GetComponent<Image>().sprite = mDefaultSprite;
            obj.Hide();
            obj.transform.SetParent(mRankNodeParent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
        },
        MAX_RANK_COUNT);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// ���뵱ǰ������Ŀ�����
    public void RankRaise(int curRank, int targetScore, ScrollRect scrollRect)
    {
        #region ȡ�ڵ㡢��ʼ��

        // ����ǰ����ֽڵ�����
        int _frontCount;
        int _backCount;

        if (curRank >= MAX_RANK_COUNT)
        {
            _frontCount = MAX_RANK_COUNT - 1;
            _backCount = 0;
        }
        else
        {
            _frontCount = curRank - 1;
            _backCount = (MAX_RANK_COUNT - 1) - _frontCount;
        }

        // ȡǰ��Ľڵ�(�б�˳�������Ӹߵ���)
        for (int i = _frontCount; i > 0; i--)
        {
            int _index = curRank - i - 1;
            if (_index >= 0 && _index < mRankDataModel.RankDataList.Count)
            {
                var _node = mRankNodePool.Allocate();
                _node.Show();
                _node.TryGetComponent<RankNode>(out var _rankNode);
                var _data = mRankDataModel.RankDataList[_index];
                _rankNode.Init(_index + 1, _data);
                ApplyRankSprite(_node.GetComponent<Image>(), _index + 1, mDefaultSprite);
                mTempRankFrontNodes.Add(_node);
            }
        }

        // ��ҽڵ�
        mPlayerNode = mRankNodePool.Allocate();
        mPlayerNode.Show();
        mPlayerNode.GetComponent<Image>().sprite = mPlayerSprite;
        mPlayerNode.TryGetComponent<RankNode>(out var playerRankNode);
        playerRankNode.InitPlayer(curRank, targetScore);
        ApplyRankSprite(mPlayerNode.GetComponent<Image>(), curRank, mPlayerSprite);

        // ȡ����Ľڵ�(�б�˳�������ӵ͵���)
        for (int i = 1; i <= _backCount; i++)
        {
            int _index = curRank - 1 + i;
            if (_index >= 0 && _index < mRankDataModel.RankDataList.Count)
            {
                var _node = mRankNodePool.Allocate();
                _node.Show();
                _node.TryGetComponent<RankNode>(out var _rankNode);
                var _data = mRankDataModel.RankDataList[_index];
                _rankNode.Init(_index + 1, _data);
                ApplyRankSprite(_node.GetComponent<Image>(), _index + 1, mDefaultSprite);
                mTempRankBackNodes.Add(_node);
            }
        }

        //�����нڵ㰴˳����ΪGridLayoutGroup���ӽڵ�(���Զ�����)
        foreach (var node in mTempRankFrontNodes)
        {
            node.transform.SetParent(scrollRect.content, false);
        }
        mPlayerNode.transform.SetParent(scrollRect.content, false);
        foreach (var node in mTempRankBackNodes)
        {
            node.transform.SetParent(scrollRect.content, false);
        }

        #endregion

        if (curRank == 1)
        {
            //Debug.Log("��ǰ�Ѿ��ǵ�һ����");
            return;
        }

        scrollRect.StartCoroutine(SetScrollPositionCoroutine(scrollRect, curRank));

        // Ŀ������
        var playerTargetRank = mRankDataModel.GetPlayerRank(targetScore);

        if (curRank == playerTargetRank)
        {
            //Debug.Log("����û�б仯��ֱ�ӷ���");
            return;
        }

        //������GridLayoutGroup�ڶ����ڵ��λ�����
        ActionKit.Delay(0.5f, () =>
        {
            int rankDifference = curRank - playerTargetRank; // ����������
            int moveToIndex; // �����ӽڵ�λ��
            // �������������ܽڵ���
            if (rankDifference >= MAX_RANK_COUNT - 1)
            {
                // ����ǰ10���ڣ���Ϊ�ڶ����ڵ�
                if (playerTargetRank > MAX_PAGE_RANK_COUNT)
                {
                    moveToIndex = 1;
                }
                //ǰʮ����������Ϊ׼
                else
                {
                    moveToIndex = playerTargetRank - 1;
                }
            }
            else
            {
                int frontNodesCount = mTempRankFrontNodes.Count;
                moveToIndex = frontNodesCount - rankDifference;
                if (moveToIndex < 0) moveToIndex = 0;
            }

            #region ��������+��������仯

            // 1����ҽڵ�����
            float targetY = CalculateGridChildLocalY(scrollRect, moveToIndex);
            var rect = mPlayerNode.GetComponent<RectTransform>();
            rect.DOScale(1.2f, 0.5f);
            rect.DOLocalMoveY(targetY, 2f).OnComplete(() =>
            {
                mPlayerNode.transform.SetSiblingIndex(moveToIndex);
                rect.localScale = Vector3.one;

                ApplyRankSprite(mPlayerNode.GetComponent<Image>(), playerTargetRank, mPlayerSprite);
            });

            // 2��ScrollRect����
            int playerIndex = moveToIndex;
            float targetNormalizedPos = CalculateNormalizedPositionForNode(scrollRect, playerIndex);
            DOTween.To(() => scrollRect.verticalNormalizedPosition,
                x => scrollRect.verticalNormalizedPosition = x,
                targetNormalizedPos,
                2f);

            // 3����ҽڵ���������
            if (mPlayerNode.TryGetComponent<RankNode>(out var playerRankNode))
            {
                float startValue = curRank;
                DOTween.To(() => startValue, x =>
                {
                    startValue = x;
                    playerRankNode.TxtRank.text = Mathf.FloorToInt(startValue).ToString();
                }, playerTargetRank, 2f);
            }

            // 4�������ڵ���������
            var content = scrollRect.content;
            List<GameObject> frontNodesNew = new List<GameObject>();
            List<GameObject> backNodesNew = new List<GameObject>();

            int totalCount = content.childCount;
            int playerSiblingIndex = mPlayerNode.transform.GetSiblingIndex();
            for (int i = 0; i < totalCount; i++)
            {
                var child = content.GetChild(i).gameObject;

                if (child == mPlayerNode)
                    continue; // ��ҽڵ㲻����

                if (i < moveToIndex)
                    frontNodesNew.Add(child);
                //else if (i > moveToIndex)
                else
                    backNodesNew.Add(child);
            }

            // Tweenǰ��ڵ�����
            for (int i = 0; i < frontNodesNew.Count; i++)
            {
                int nodeRank = playerTargetRank - frontNodesNew.Count + i; // Ŀ������
                var node = frontNodesNew[i];
                if (node.TryGetComponent<RankNode>(out var rankNode))
                {
                    rankNode.RankingChangeOther(nodeRank, () =>
                    {
                        ApplyRankSprite(node.GetComponent<Image>(), nodeRank, mDefaultSprite);
                    });
                }
            }

            // Tween����ڵ�����
            for (int i = 0; i < backNodesNew.Count; i++)
            {
                int nodeRank = playerTargetRank + i + 1; // Ŀ������
                var node = backNodesNew[i];
                if (node.TryGetComponent<RankNode>(out var rankNode))
                {
                    ApplyRankSprite(node.GetComponent<Image>(), nodeRank, mDefaultSprite);
                }
            }
            #endregion

        }).Start(this);

    }

    public void ClearRankNode()
    {
        foreach (var node in mTempRankFrontNodes)
        {
            mRankNodePool.Recycle(node);
        }

        foreach (var node in mTempRankBackNodes)
        {
            mRankNodePool.Recycle(node);
        }

        if (mPlayerNode != null)
        {
            mRankNodePool.Recycle(mPlayerNode);
            mPlayerNode = null;
        }

        mTempRankFrontNodes.Clear();
        mTempRankBackNodes.Clear();
    }

    #region �ڵ�λ�õȼ���
    
    // ������ҽڵ����ӿڵײ�
    private IEnumerator SetScrollPositionCoroutine(ScrollRect scrollRect, int curRank)
    {
        yield return null;
        yield return null;

        if (mPlayerNode == null) yield break;

        var content = scrollRect.content;
        var viewport = scrollRect.viewport;

        var contentRect = content.GetComponent<RectTransform>();
        var viewportRect = viewport.GetComponent<RectTransform>();
        var playerRect = mPlayerNode.GetComponent<RectTransform>();

        // ��ȡ��ҽڵ㶥����Content�����ľ���
        float playerTopY = Mathf.Abs(playerRect.anchoredPosition.y);

        // ��ҽڵ�ĸ߶�
        float playerHeight = playerRect.rect.height / 2;
       
        // ����ҵĵײ����� viewport �ĵײ�
        // ������Ҫ�����ľ��룺playerTopY + playerHeight - viewportHeight
        float offset = playerTopY + playerHeight - viewportRect.rect.height;

        // ��ƫ����ת�� normalizedPosition
        float scrollRange = contentRect.rect.height - viewportRect.rect.height;

        float targetNormalizedPos = 1f;

        if (scrollRange > 0f)
        {
            targetNormalizedPos = 1f - (offset / scrollRange);
            targetNormalizedPos = Mathf.Clamp01(targetNormalizedPos);
        }
        else
        {
            targetNormalizedPos = 1f;
        }

        scrollRect.verticalNormalizedPosition = targetNormalizedPos;
    }

    // ��ҽڵ���GridLayoutGroup�еı���Yλ��
    private float CalculateGridChildLocalY(ScrollRect scrollRect, int siblingIndex)
    {
        var grid = scrollRect.content.GetComponent<GridLayoutGroup>();
        if (grid == null) return 0f;

        float spacing = grid.spacing.y;
        float cellHeight = grid.cellSize.y;
        float offset = grid.cellSize.y / 2;

        return -(cellHeight + spacing) * siblingIndex - offset;
    }

    // ScrollBar �Ĺ�һ��λ�ü���
    private float CalculateNormalizedPositionForNode(ScrollRect scrollRect, int nodeIndex)
    {
        var contentRect = scrollRect.content.GetComponent<RectTransform>();
        var viewportRect = scrollRect.viewport;

        var grid = scrollRect.content.GetComponent<GridLayoutGroup>();
        if (grid == null) return 1f;

        float spacingY = grid.spacing.y;
        float cellHeight = grid.cellSize.y;

        float nodeHeightWithSpacing = cellHeight + spacingY;

        float contentHeight = contentRect.rect.height;
        float viewportHeight = viewportRect.rect.height;

        // ����Ŀ��ڵ㶥������content�����ľ���
        // nodeIndex��0��ʼ���ڵ㶥��λ��Ϊ��nodeIndex * nodeHeightWithSpacing
        float nodeTopPos = nodeIndex * nodeHeightWithSpacing;

        // ����ʹ�ýڵ�ײ�����viewport�ײ�ʱ��content����λ��
        // ����content�������� = �ڵ�ײ�λ�� - viewport�߶�
        float nodeBottomPos = nodeTopPos + cellHeight;

        // scrollPos = nodeBottomPos - viewportHeight
        float scrollPos = nodeBottomPos - viewportHeight;

        // ���Ʒ�Χ scrollPos ����С��0�����ܴ��� contentHeight - viewportHeight
        scrollPos = Mathf.Clamp(scrollPos, 0, contentHeight - viewportHeight);

        float normalizedPos = 1f - (scrollPos / (contentHeight - viewportHeight));

        return Mathf.Clamp01(normalizedPos);
    }

    private void ApplyRankSprite(Image image, int rank, Sprite del)
    {
        if (rank == 1)
            image.sprite = mFirSprite;
        else if (rank == 2)
            image.sprite = mSecSprite;
        else if (rank == 3)
            image.sprite = mThiSprite;
        else
            image.sprite = del;
    }
    #endregion

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }
}
