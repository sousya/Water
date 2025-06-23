using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnLimitNode : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] countDownTxts;

    void Update()
    {
        if (!CountDownTimerManager.Instance.IsTimerFinished(GameDefine.GameConst.UNLIMIT_ITEM_SIGN))
        {
            foreach (var txt in countDownTxts)
            {
                txt.text = CountDownTimerManager.Instance.GetRemainingTimeText(GameDefine.GameConst.UNLIMIT_ITEM_SIGN);
                //txt.text = HealthManager.Instance.UnLimitHpTimeStr;
            }
        }
    }
}
