using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using Spine.Unity;
using UnityEngine.UI;
using QFramework;

public partial class BottleCtrl : MonoBehaviour
{
    public bool isFinish, isFreeze, isClearHide, isNearHide, isPlayAnim, isSelect, isClearHideAnim;
    public List<int> waters = new List<int>();
    public List<bool> hideWaters = new List<bool>();
    public List<WaterItem> waterItems = new List<WaterItem>();
    public List<BottleWaterCtrl> waterImg = new List<BottleWaterCtrl>();
    public List<Transform> spineNode = new List<Transform>();
    public List<Transform> waterNode = new List<Transform>();
    public Transform spineGo, modelGo, leftMovePlace, freezeGo;
    public Animator bottleAnim, fillWaterGoAnim;
    public SkeletonGraphic spine, finishSpine, freezeSpine;
    public int maxNum = 4, limitColor = 0;
    public Image ImgWaterTop, ImgWaterDown, ImgLimit;
    public SkeletonGraphic nearHide, clearHide, thunder;
    public bool isUp;
    public GameObject finishGo;
    public GameObject waterTopSurface;// 倒水的过程中，水面的最高高度不会超过这个线。
    
    public List<BottleRecord> moveRecords = new List<BottleRecord>();
    
    public int bottleIdx;
    public int unlockClear = 0;
    public Button bottle;
    
    // 存储每种情况的水的旋转角度。index表示瓶子剩余水的个数。
    private Quaternion[] _waterRotations = new Quaternion[4];
    private BottleRenderUpdate _bottleRenderUpdate;
    
    // Start is called before the first frame update
    void Start()
    {
        bottle.onClick.AddListener(OnSelected);
        
        // 计算瓶子的旋转角度(根据三角形公式推导)
        var sinEdge = Mathf.Abs(waterTopSurface.transform.position.x - this.transform.position.x);
        _bottleRenderUpdate = bottleAnim.GetComponent<BottleRenderUpdate>();
        var waterRenderUpdaters = _bottleRenderUpdate.GetComponentsInChildren<WaterRenderUpdater>();
        for (int i = waterRenderUpdaters.Length - 1; i >= 1; i--)
        {
            var position = waterRenderUpdaters[i].waterSurface[0].position;
            var cosEdge = Mathf.Abs(waterTopSurface.transform.position.y - position.y);
            _waterRotations[i] = GetBottleRotation(sinEdge, cosEdge);
        }
        // 倒完水使用120度写死角度。
        _waterRotations[0] = Quaternion.Euler(0, 0, -120); 
    }
    
    private void LateUpdate()
    {
        fillWaterGoAnim.transform.localRotation = Quaternion.Inverse(fillWaterGoAnim.transform.parent.rotation);
    }
    
    private void OnSelected()
    {
        if(!LevelManager.Instance.isPlayAnim && !LevelManager.Instance.isPlayFxAnim)
        {
            GameCtrl.Instance.OnSelect(this);
        }
    }
    
    private Quaternion GetBottleRotation(float sinEdge, float cosEdge)
    {
        float angle = Mathf.Atan(sinEdge / cosEdge);
        angle = Mathf.PI / 2 - angle;
        return Quaternion.Euler(0, 0, -angle * Mathf.Rad2Deg);
    }
}
