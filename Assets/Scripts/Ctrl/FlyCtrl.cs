using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCtrl : MonoBehaviour
{
    public Transform target;
    public float flyTime;
    // Start is called before the first frame update
    public void BeginFly()
    {

        var tween = transform.DOMove(target.position, flyTime)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                LevelManager.Instance.isPlayFxAnim = false;
                Destroy(gameObject);
            });

        //tween.OnUpdate(() =>
        //{
        //    tween.SetTarget(target.position);
        //});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
