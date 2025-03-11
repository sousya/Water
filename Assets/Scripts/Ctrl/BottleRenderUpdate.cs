
using System;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class BottleRenderUpdate : MonoBehaviour
{
    public WaterRenderUpdater[] waterRenders;
    public SkeletonGraphic[] Brooms;

    public int bottleIndex = 1;

    private Material _material;
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
            material.renderQueue = 3000 - 1;
            skeletonGraphic.material = material;
            //skeletonGraphic.TrimRenderers();
            var skeletonSubmeshGraphics = skeletonGraphic.gameObject.GetComponentsInChildren<SkeletonSubmeshGraphic>();
            foreach (var skeletonSubmeshGraphic in skeletonSubmeshGraphics)
            {
                skeletonSubmeshGraphic.material = material;
            }
        }
    }

    // public void Update()
    // {
    //     foreach (var waterSurface in waterSurfaces)
    //     {
    //         waterSurface.localRotation = Quaternion.Inverse(_bottleTransform.rotation);
    //     }
    // }
}
