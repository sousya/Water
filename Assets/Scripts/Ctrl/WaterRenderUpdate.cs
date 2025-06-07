using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterRenderUpdate : MonoBehaviour
{
    private static readonly int MainColor = Shader.PropertyToID("_Color");
    private const float HALF_WATAER_WIDTH = 2.0f * 0.5f;
    private const float CORRECT_WATER_SURFACE = 0.4f;
    public Transform[] waterSurface;
    public bool bBottom = false;
    public Transform bottleTransform;
    public Image blackWater;// 黑水的效果。
    private Mesh _mesh;
    private MeshRenderer _meshRenderer = null;
    private MeshFilter _meshFilter = null;
    private Material _material = null;
    private Image _image;
    private float _fillAmount;
    private Material _blackWaterMaterial = null;
    private MeshRenderer _blackWaterRenderer = null;
    private MeshFilter _blackWaterFilter = null;

    private void ValidMaterial()
    {
        if (!_material)
        {
            if (!_meshRenderer)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }
            _material = _meshRenderer.material;
        }

        if (!_blackWaterMaterial)
        {
            if (!_blackWaterRenderer)
            {
                _blackWaterRenderer = blackWater.GetComponent<MeshRenderer>();
            }
            _blackWaterMaterial = _blackWaterRenderer.material;
        }
    }
    
    public Color WaterColor
    {
        get
        {
            ValidMaterial();
            return _material.GetColor(MainColor);
        }
        set
        {
            ValidMaterial();
            _material.SetColor(MainColor, value);
        }
    }

    public float FillHeightClip
    {
        set
        {
            //Debug.Log(value + "-----------------");
            ValidMaterial();
            _material.SetFloat("_FillHeight", Mathf.Min(value, FillAmount));
        }
    }

    protected float FillAmount
    {
        get => _fillAmount;
        set
        {
            float newValue = waterSurface[1].position.y - waterSurface[0].position.y;
            newValue = newValue * value + waterSurface[0].position.y;
            _fillAmount = newValue;
        }
    }

    public float Stencil
    {
        get
        {
            ValidMaterial();
            return _material.GetFloat("_StencilRef");
        }
        set
        {
            ValidMaterial();
            _material.SetFloat("_StencilRef", value);
            _blackWaterMaterial.SetFloat("_StencilRef", value);
        }
    }

    public int RenderQueue
    {
        get
        {
            ValidMaterial();
            return _material.renderQueue;
        }
        set
        {
            ValidMaterial();
            _material.renderQueue = value;
            _blackWaterMaterial.renderQueue = value + 1;
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _image = GetComponent<Image>();
        
        _blackWaterRenderer = blackWater.GetComponent<MeshRenderer>();
        _blackWaterFilter = blackWater.GetComponent<MeshFilter>();
        ValidMaterial();
        _blackWaterMaterial.color = Color.black;
        _blackWaterMaterial.renderQueue = 3001;
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
        
        // fillAmount
        if (_image != null)
        {
            this.FillAmount = _image.fillAmount;
        }
        
        // 黑水的mesh同时设置
        _blackWaterFilter.mesh = _mesh;
    }
}
