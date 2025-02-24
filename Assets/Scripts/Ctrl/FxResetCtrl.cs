using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxResetCtrl : MonoBehaviour
{

    public ParticleSystemRenderer ps;

    // Start is called before the first frame update
    void Start()
    {
        ps.material.shader = Shader.Find(ps.material.shader.name);
    }

    void SetPaticle()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
