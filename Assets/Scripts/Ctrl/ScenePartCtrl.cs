using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenePartCtrl : MonoBehaviour
{
    public GameObject ScenePart1, ScenePart2, ScenePart3, ScenePart4, ScenePart5;
    public Image SceneImg1, SceneImg2, SceneImg3, SceneImg4, SceneImg5;
    public Material shine;


    public IEnumerator ShowUnlock(int i)
    {
        switch (i)
        {
            case 1:
                SceneImg1.material = shine;
                StartCoroutine(WaterShine());
                yield return new WaitForSeconds(1f);
                SceneImg1.material = null;
                break;
            case 2:
                SceneImg2.material = shine;
                StartCoroutine(WaterShine());
                yield return new WaitForSeconds(1f);
                SceneImg2.material = null;
                break;
            case 3:
                SceneImg3.material = shine;
                StartCoroutine(WaterShine());
                yield return new WaitForSeconds(1f);
                SceneImg3.material = null;
                break;
            case 4:
                SceneImg4.material = shine;
                StartCoroutine(WaterShine());
                yield return new WaitForSeconds(1f);
                SceneImg4.material = null;
                break;
            case 5:
                SceneImg5.material = shine;
                StartCoroutine(WaterShine());
                yield return new WaitForSeconds(1f);
                SceneImg5.material = null;
                break;
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
