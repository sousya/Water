using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnim : MonoBehaviour
{
    public Transform pos1, pos2;
    // Start is called before the first frame update
    void OnEnable()
    {
        transform.position = pos1.position;
        transform.DOMove(pos2.position, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
