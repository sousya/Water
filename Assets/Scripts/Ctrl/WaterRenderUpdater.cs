using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterRenderUpdater : MonoBehaviour
{
    private static readonly int MainColor = Shader.PropertyToID("_Color");
    private const float HALF_WATAER_WIDTH = 2.0f * 0.5f;
    private const float CORRECT_WATER_SURFACE = 0.4f;
    public Transform[] waterSurface;
    public bool bBottom = false;
    public Transform bottleTransform;
    public int bottleIndex; // 作为stencil标记使用，从1开始。
    private Mesh _mesh;
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Material _material;
    private Image _image;
    
    public Color WaterColor
    {
        get => _material.GetColor(MainColor);
        set => _material.SetColor(MainColor, value);
    }
    
    public float FillHeightClip
    {
        set
        {
            _material.SetFloat("_FillHeight", value);
        }
    }

    public float Stencil
    {
        get => _material.GetFloat("_StencilRef");
        set
        {
            _material.SetFloat("_StencilRef", value);
        }
    }
    
    public int RenderQueue
    {
        get => _material.renderQueue;
        set => _material.renderQueue = value;
    }
    
    // Start is called before the first frame update
    void OnEnable()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _material = _meshRenderer.material;
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (waterSurface.Length < 2)
        {
            Debug.LogError("Set Water Surface!!");
            return;
        }

        var rotation = bottleTransform.rotation;
        // var bottleTransformRotation = bottleTransform.rotation;
        rotation.ToAngleAxis(out float angle, out _);
        var oneDivCos = 1.0f / Mathf.Max(Mathf.Cos(angle * Mathf.Deg2Rad), 0.001f);

        Vector3 topCenter = waterSurface[1].position;
        topCenter.y += CORRECT_WATER_SURFACE * Mathf.Sin(angle * Mathf.Deg2Rad);
        Vector3 bottomCenter = waterSurface[0].position;


        var halfWaterWidth = HALF_WATAER_WIDTH * oneDivCos;
        if (bBottom)
        {
            Vector3 bottomRight = bottomCenter + transform.right * HALF_WATAER_WIDTH;
            bottomCenter = new Vector3(bottomCenter.x, bottomRight.y, 0);//bottomRight - new Vector3(halfWaterWidth, 0, 0);
        }

        var verts = new Vector3[4];
        verts[0] = bottomCenter + new Vector3(-halfWaterWidth,  0, 0);
        verts[1] = bottomCenter + new Vector3(halfWaterWidth,  0, 0);
        verts[2] = topCenter + new Vector3(halfWaterWidth,  0, 0);
        verts[3] = topCenter + new Vector3(-halfWaterWidth,  0, 0);
        verts[1].x = verts[2].x;
        verts[3].x = verts[0].x;
        verts[0].y = Mathf.Min(verts[0].y, verts[3].y);
        verts[1].y = Mathf.Min(verts[1].y, verts[2].y);

        _mesh.vertices = verts;
        _mesh.triangles = new int[] {0, 1, 2, 0, 2, 3};
        _mesh.uv = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };
        _mesh.UploadMeshData(false);
        _meshFilter.mesh = _mesh;
        
        // // fillAmount
        // if (_image != null)
        // {
        //     this.FillAmount = _image.fillAmount;
        // }
    }
}
