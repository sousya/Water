
using System;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class BottleRenderUpdate : MonoBehaviour
{
    public WaterRenderUpdater[] waterRenders;
    public SkeletonGraphic[] Brooms;
    public Transform WaterSpine;
    public Image BotteImage;
    public Image MaskImage;

    public int bottleIndex = 1;
    public GameObject waterTopSurface;

    private Material _material;
    private Vector3 _waterScale;
    public void Start()
    {
        _material = new Material(MaskImage.material);
        _material.SetFloat("_StencilRef", bottleIndex);
        MaskImage.material = _material;
        
        foreach (var waterRenderUpdater in waterRenders)
        {
            waterRenderUpdater.Stencil = bottleIndex;
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
        waterSpineMaterial.SetFloat("_StencilCompWater", (int)UnityEngine.Rendering.CompareFunction.Equal);
        WaterSpine.GetComponent<SkeletonGraphic>().material = waterSpineMaterial;
    }

    public void LateUpdate()
    {
        WaterSpine.rotation = Quaternion.identity;
        transform.rotation.ToAngleAxis(out float angle, out _);
        var oneDivCos = 1.0f / Mathf.Max(Mathf.Cos(angle * Mathf.Deg2Rad), 0.1f);
        WaterSpine.localScale = new Vector3(oneDivCos *_waterScale.x, _waterScale.y, _waterScale.z);

        // 设置最高水位线
        var waterHeightClip = waterTopSurface.transform.position.y;
        foreach (var waterRenderUpdater in waterRenders)
        {
            waterRenderUpdater.FillHeightClip = waterHeightClip;
        }
        var position = WaterSpine.position;
        var waterSpineHeight = Mathf.Min(position.y, waterHeightClip + 0.5f);
        position = new Vector3(position.x, waterSpineHeight, position.z);
        WaterSpine.position = position;
    }

    // 移动的瓶子，最后渲染
    public void SetMoveBottleRenderState(bool isMove)
    {
        var transparentRenderQueue = isMove ? 3100 : 3000;

        foreach (var waterRenderUpdater in waterRenders)
        {
            waterRenderUpdater.RenderQueue = transparentRenderQueue;
        }

        MaskImage.material.renderQueue = transparentRenderQueue - 1;
        WaterSpine.GetComponent<SkeletonGraphic>().material.renderQueue = transparentRenderQueue;
        BotteImage.material.renderQueue = transparentRenderQueue + 1;
    }
}
