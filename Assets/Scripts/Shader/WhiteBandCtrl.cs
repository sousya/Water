using UnityEngine;
using UnityEngine.UI;

public class WhiteBandCtrl : MonoBehaviour
{
    public Material uiMaterial; // 材质
    public float speed = 1.0f; // 光带移动速度

    private void Update()
    {
        // 更新光带位置
        float bandPosition = Mathf.Repeat(Time.time * speed, 1.0f); // 循环从0到1
        uiMaterial.SetFloat("_BandPosition", bandPosition);
    }
}