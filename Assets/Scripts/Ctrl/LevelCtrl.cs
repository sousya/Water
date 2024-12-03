using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCtrl : MonoBehaviour, IController
{
    [SerializeField] List<LevelCreateCtrl> levels;
    [SerializeField] GameObject cake;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Start()
    {

    }

   
}
