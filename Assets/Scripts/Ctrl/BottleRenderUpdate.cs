
using System;
using UnityEngine;
using UnityEngine.UI;

public class BottleRenderUpdate : MonoBehaviour
{
    public WaterRenderUpdater[] waterRenders;

    public int bottleIndex = 1;

    private Material _material;
    public void Start()
    {
        _material = GetComponent<Image>().material;
        _material.SetFloat("_StencilRef", bottleIndex);
        
        foreach (var waterRenderUpdater in waterRenders)
        {
            waterRenderUpdater.Stencil = bottleIndex;
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
