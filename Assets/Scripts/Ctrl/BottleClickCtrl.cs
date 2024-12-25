using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BottleClickCtrl : MonoBehaviour
{
    public Button bottle;
    public BottleCtrl bottleCtrl;
    // Start is called before the first frame update
    void Start()
    {
        bottle.onClick.AddListener(test);
    }

    void test()
    {
        Debug.Log("1111");
    }

    private void OnSelected()
    {
        GameCtrl.Instance.OnSelect(bottleCtrl);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
