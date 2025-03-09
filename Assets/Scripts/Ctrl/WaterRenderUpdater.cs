using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterRenderUpdater : MonoBehaviour
{
    private static readonly int MainColor = Shader.PropertyToID("_Color");
    private const float HALF_WATAER_WIDTH = 2.0f * 0.5f;
    public Transform[] waterSurface;
    public bool bBottom = false;
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
    
    public float FillAmount
    {
        set
        {
            float waterHeight = waterSurface[1].position.y - waterSurface[0].position.y;
            float fillHeight = waterHeight * value + waterSurface[0].position.y;
            _material.SetFloat("_FillHeight", fillHeight);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _material = _meshRenderer.material;
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waterSurface.Length < 2)
        {
            Debug.LogError("Set Water Surface!!");
            return;
        }

        Vector3 topCenter = waterSurface[1].position;
        Vector3 bottomCenter = waterSurface[0].position;
        var rotation = transform.rotation;
        rotation.ToAngleAxis(out float angle, out _);
        var oneDivCos = 1.0f / Mathf.Max(Mathf.Cos(angle * Mathf.Deg2Rad), 0.001f);
        var halfWaterWidth = HALF_WATAER_WIDTH * oneDivCos;
        if (bBottom)
        {
            Vector3 bottomRight = bottomCenter + transform.right * HALF_WATAER_WIDTH;
            bottomCenter = bottomRight - new Vector3(halfWaterWidth, 0, 0);
        }

        var verts = new Vector3[4];
        verts[0] = bottomCenter + new Vector3(-halfWaterWidth,  0, 0);
        verts[1] = bottomCenter + new Vector3(halfWaterWidth,  0, 0);
        verts[2] = topCenter + new Vector3(halfWaterWidth,  0, 0);
        verts[3] = topCenter + new Vector3(-halfWaterWidth,  0, 0);
        verts[1].x = verts[2].x;
        verts[3].x = verts[0].x;

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
    }
}
