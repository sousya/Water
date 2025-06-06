using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenePartCtrl : MonoBehaviour
{
    public GameObject[] SceneParts;
    public Image[] SceneImgs;
    public Material shine;

    public IEnumerator ShowUnlock(int i)
    {
        //建筑从1开始计算
        var _index = i - 1;
        if (_index >= 0 && _index < SceneImgs.Length)
        {
            SceneImgs[_index].material = shine;
            StartCoroutine(WaterShine());
            yield return new WaitForSeconds(1f);
            SceneImgs[_index].material = null;
        }
    }

    public IEnumerator WaterShine()
    {
        float shineTime = -0.2f;

        while (shineTime <= 2f)
        {
            shineTime += 0.05f * 1;
            shine.SetFloat("_BandPosition", shineTime);
            yield return new WaitForSeconds(0.02f);
        }
    }
}
