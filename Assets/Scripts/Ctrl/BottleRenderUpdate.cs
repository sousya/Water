
using System;
using System.Collections.Generic;
using QFramework;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BottleRenderUpdate : MonoBehaviour
{
    public WaterRenderUpdate[] waterRenders;
    public SkeletonGraphic[] Brooms;
    public Transform WaterSpine;
    public Transform WaterSpinePos;
    public Image BotteImage;
    public Image MaskImage;

    public int bottleIndex = 1;
    public GameObject waterTopSurface;
    public Transform fillWaterTransform;
    public Transform fillWaterPosition; // 固定倒水的位置
    public Transform[] corners;

    private Material _material;
    private Vector3 _waterScale;
    private BottleCtrl _bottleCtrl;
    
    private BottleCtrl _otherBottle = null;
    public void Start()
    {
        _bottleCtrl = transform.parent.GetComponent<BottleCtrl>();
        
        _material = new Material(MaskImage.material);
        _material.SetFloat("_StencilRef", bottleIndex);
        MaskImage.material = _material;

        BotteImage.material = new Material(BotteImage.material);

        //int i = 0;
        foreach (var waterRenderUpdater in waterRenders)
        {
            waterRenderUpdater.Stencil = bottleIndex;
            //waterRenderUpdater.RenderQueue = 2901 + i++;
        }
        
        foreach (var skeletonGraphic in Brooms)
        {
            var material = new Material(skeletonGraphic.material);
            material.SetFloat("_Stencil", bottleIndex);
            material.SetFloat("_StencilOp", ((int)UnityEngine.Rendering.StencilOp.Keep) * 1.0f);
            material.SetFloat("_StencilComp", ((int)UnityEngine.Rendering.CompareFunction.Disabled) * 1.0f);
            material.renderQueue = 3000 + 1;
            skeletonGraphic.material = material;
            //skeletonGraphic.TrimRenderers();
            var skeletonSubmeshGraphics = skeletonGraphic.gameObject.GetComponentsInChildren<SkeletonSubmeshGraphic>();
            foreach (var skeletonSubmeshGraphic in skeletonSubmeshGraphics)
            {
                skeletonSubmeshGraphic.material = material;
            }
        }
        
        _waterScale = WaterSpine.localScale;
        var waterSpineMaterial = new Material(WaterSpine.GetComponent<SkeletonGraphic>().material);
        waterSpineMaterial.SetFloat("_StencilWater", bottleIndex);
        //waterSpineMaterial.SetFloat("_StencilCompWater", (int)UnityEngine.Rendering.CompareFunction.Equal);
        waterSpineMaterial.SetFloat("_StencilCompWater", (int)UnityEngine.Rendering.CompareFunction.Equal);
        waterSpineMaterial.SetFloat("_StencilReadMaskWater", 255);
        WaterSpine.GetComponent<SkeletonGraphic>().material = waterSpineMaterial;
    }

    public void LateUpdate()
    {
        // 设置最高水位线
        var waterHeightClip = waterTopSurface.transform.position.y;
        foreach (var waterRenderUpdater in waterRenders)
        {
            waterRenderUpdater.FillHeightClip = waterHeightClip;
        }
        
        // 设置fill water效果的位置
        if (fillWaterTransform != null && _otherBottle != null)
        {
            fillWaterTransform.localRotation = Quaternion.Inverse(transform.rotation);
            fillWaterTransform.position = new Vector3(_otherBottle.transform.position.x, fillWaterPosition.position.y, fillWaterPosition.position.z);
        }
        
        // 计算顶部spine的位置和缩放
        WaterSpine.rotation = Quaternion.identity;
        var position = WaterSpinePos.position;
        var waterSpineHeight = Mathf.Min(position.y, waterHeightClip);
        Vector2 point1 = new Vector2(-1, waterSpineHeight);
        Vector2 point2 = new Vector2(1, waterSpineHeight);

        List<Vector2> intesectionPoints = new List<Vector2>();
        for (int i = 0; i < 4; i++)
        {
            bool bIntersect = LineIntersection.GetLineSegmentIntersection(point1, point2, new Vector2(corners[i].position.x, corners[i].position.y),
                new Vector2(corners[(i + 1) % 4].position.x, corners[(i + 1) % 4].position.y), out Vector2 intersectionPoint);
            if (bIntersect)
            {
                intesectionPoints.Add(intersectionPoint);
            }
        }
        
        Assert.IsTrue(intesectionPoints.Count == 2);
        
        position = new Vector3((intesectionPoints[0].x + intesectionPoints[1].x) * 0.5f, waterSpineHeight, position.z);
        WaterSpine.localScale = new Vector3(Mathf.Abs(intesectionPoints[1].x - intesectionPoints[0].x) / 1.688f, 1, 1);
        WaterSpine.position = position;
    }
    
    public float GetBottleBottomY()
    {
        // 获取瓶子底部的Y坐标
        //Debug.LogError(corners[3].position.y);
        return corners[3].position.y;
    }
    
    public void SetWaterSortingOrder(int order)
    {
        foreach (var waterRenderUpdater in waterRenders)
        {
            waterRenderUpdater.SortingOrder = order;
        }
    }

    // 移动的瓶子，最后渲染（改变）
    public void SetMoveBottleRenderState(bool isMove, BottleCtrl otherBottle = null)
    {
        _otherBottle = otherBottle;
        if (isMove)
        {
            _bottleCtrl.GetComponent<Canvas>().sortingOrder = 1;
            SetWaterSortingOrder(1);
            if (_otherBottle != null)
            {
                _otherBottle.GetComponent<Canvas>().sortingOrder = 2;
                
                var bottleRender = _otherBottle.GetComponentInChildren<BottleRenderUpdate>();
                bottleRender.SetWaterSortingOrder(2);
                _bottleCtrl.ImgWaterDown.material.SetFloat("_ClipHeight", bottleRender.GetBottleBottomY());
            }
        }
        else
        {
            _bottleCtrl.GetComponent<Canvas>().sortingOrder = 0;
            SetWaterSortingOrder(0);
            if (_otherBottle != null)
            {
                _otherBottle.GetComponent<Canvas>().sortingOrder = 0;
                
                var bottleRender = _otherBottle.GetComponentInChildren<BottleRenderUpdate>();
                bottleRender.SetWaterSortingOrder(0);
                _bottleCtrl.ImgWaterDown.material.SetFloat("_ClipHeight", -1000.0f);
            }
        }
        
    }
}
