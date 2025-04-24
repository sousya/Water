using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public Button btn;

    private void Awake()
    {
        HealthManager healthManager = HealthManager.Instance;
    }

    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            HealthManager.Instance.UseHp();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
