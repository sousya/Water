
using System;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class BottleRenderUpdate : MonoBehaviour
{
    public WaterRenderUpdater[] waterRenders;
    public SkeletonGraphic[] Brooms;
    public Transform WaterSpine;

    public int bottleIndex = 1;

    private Material _material;
    private Vector3 _waterScale;
    public void Start()
    {
        _material = new Material(GetComponent<Image>().material);
        _material.SetFloat("_StencilRef", bottleIndex);
        GetComponent<Image>().material = _material;
        
        foreach (var waterRenderUpdater in waterRenders)
        {
            waterRenderUpdater.Stencil = bottleIndex;
        }
        
        foreach (var skeletonGraphic in Brooms)
        {
            var material = new Material(skeletonGraphic.material);
            material.SetFloat("_Stencil", bottleIndex);
            material.SetFloat("_StencilOp", ((int)UnityEngine.Rendering.StencilOp.Keep) * 1.0f);
            material.SetFloat("_StencilComp", ((int)UnityEngine.Rendering.CompareFunction.Always) * 1.0f);
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

    public void Update()
    {
        // foreach (var waterSurface in waterSurfaces)
        // {
        //     waterSurface.localRotation = Quaternion.Inverse(_bottleTransform.rotation);
        // }
        WaterSpine.localRotation = Quaternion.Inverse(transform.rotation);
        transform.rotation.ToAngleAxis(out float angle, out _);
        var oneDivCos = 1.0f / Mathf.Max(Mathf.Cos(angle * Mathf.Deg2Rad), 0.001f);
        WaterSpine.localScale = new Vector3(oneDivCos *_waterScale.x, _waterScale.y, _waterScale.z);
    }
}
