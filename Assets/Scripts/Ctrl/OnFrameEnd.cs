using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFrameEnd : MonoBehaviour
{
    public GameObject FxGo;
    public Vector3 pos;
    public float xMin, xMax, yMin, yMax;
    public Vector3 localPos;
    public void OnChangeEnd()
    {
        LevelManager.Instance.ChangeEnd = true;
    }

    public void HideFx()
    {
        FxGo.SetActive(false);
    }

    public void HideSelf()
    {
        gameObject.SetActive(false);
    }

    public void DestroyFx()
    {
        Destroy(FxGo);
    }

    public void ShowFx()
    {
        FxGo.SetActive(true);
    }

    public void PlaySound(string name)
    {
        AudioKit.PlaySound("resources://Audio/" + name);
        //AudioKit.PlaySound("resources://Audio/cake");
    }

    public void ResetLocalPosition()
    {
        transform.localPosition = localPos;
    }

    public void SetOriginPosition(Vector3 position)
    {
        pos = position;
    }

    public void SetLocalOriginPosition(Vector3 position)
    {
        localPos = position;
    }

    public void WaitStart()
    {
        LevelManager.Instance.WaitStart();
    }

    private void Start()
    {
        //pos = transform.position;
    }

    public void RandomPos()
    {
        var x = Random.Range(xMin, xMax);
        var y = Random.Range(yMin, yMax);
        transform.position = new Vector3(x, y, 0);
    }
}
