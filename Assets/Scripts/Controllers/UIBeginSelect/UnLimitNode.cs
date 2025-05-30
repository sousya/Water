using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnLimitNode : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] countDownTxts;

    void Update()
    {
        foreach (var txt in countDownTxts)
        {
            txt.text = HealthManager.Instance.UnLimitHpTimeStr;
        }
    }
}
