using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottleWaterCtrl : MonoBehaviour
{
    public SkeletonGraphic spine, broomSpine, createSpine, changeSpine, magnetSpine;
    public GameObject spineGo, HideGo, broomItemGo, createItemGo, changeItemGo, magnetItemGo;
    public Animator anim;
    public Image waterImg;
    public int waterColor;
    public bool isPlayItemAnim;

    public Color color
    {
        get
        {
            return waterImg.color;
        }

        set
        {
            waterImg.color = value;
        }
    }
    // Start is called before the first frame update
    private void Start()
    {

    }

    public void SetSpineActive(bool active)
    {
        spineGo.SetActive(active);
    }

    public void PlayFillAnim(float time)
    {
        //StartCoroutine(CoroutinePlayFillAnim());
        waterImg.fillAmount = 0;
        waterImg.DOFillAmount(1, time).SetEase(Ease.Linear);
    }

    public void PlayOutAnim(float time)
    {
        //StartCoroutine(CoroutinePlayFillAnim());

        broomItemGo.SetActive(false);
        createItemGo.SetActive(false);
        changeItemGo.SetActive(false);
        waterImg.fillAmount = 1;
        waterImg.DOFillAmount(0, time).SetEase(Ease.Linear).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void PlayUseBroom()
    {
        StartCoroutine(CoroutinePlayUseBroom());
    }
    


    IEnumerator CoroutinePlayUseBroom()
    {
        isPlayItemAnim = true;
        //broomItemGo.SetActive(true);
        var go = Instantiate(broomItemGo);
        go.transform.parent = broomItemGo.transform.parent;
        go.transform.localScale = new Vector3(1, 1, 1);
        go.transform.localPosition = broomItemGo.transform.localPosition;
        go.transform.parent = LevelManager.Instance.gameCanvas;
        var useSpine = go.GetComponent<SkeletonGraphic>();
        yield return new WaitForEndOfFrame();
        useSpine.AnimationState.SetAnimation(0, "combine", false);
        yield return new WaitForSeconds(1f);
        broomItemGo.SetActive(false);
        isPlayItemAnim = false;
        gameObject.SetActive(false);
        Destroy(go);
    }

    public void PlayUseChange()
    {
        StartCoroutine(CoroutinePlayUseChange());
    }

    IEnumerator CoroutinePlayUseChange()
    {
        isPlayItemAnim = true;
        //broomItemGo.SetActive(true);
        var go = Instantiate(changeItemGo);
        go.transform.parent = changeItemGo.transform.parent;
        go.transform.localScale = new Vector3(1, 1, 1);
        go.transform.localPosition = changeItemGo.transform.localPosition;
        go.transform.parent = LevelManager.Instance.gameCanvas;
        var useSpine = go.GetComponent<SkeletonGraphic>();
        changeItemGo.SetActive(false);
        yield return new WaitForEndOfFrame();
        useSpine.AnimationState.SetAnimation(0, "combine", false);
        yield return new WaitForSeconds(1f);
        isPlayItemAnim = false;
        gameObject.SetActive(false);
        Destroy(go);
    }

    public void PlayFillAnimConnect()
    {
        string spineAnimName = "";
        switch (waterColor)
        {
            case 0:
                spineAnimName = "daoshui_dh";
                break;
            case 1:
                spineAnimName = "daoshui_cl";
                break;
        }
        spineGo.SetActive(true);
        anim.Play("WaterFillConnect");
        spine.AnimationState.SetAnimation(0, spineAnimName, false);
        //StartCoroutine(CoroutinePlayFillAnim());
    }

    public void PlayEmptyAnim()
    {
        anim.Play("NormalEmpty");
    }

    public void SetHide(bool isHide)
    {
        HideGo.SetActive(isHide);
    }
}
