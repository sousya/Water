using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxResetCtrl : MonoBehaviour
{
    public ParticleSystemRenderer ps;

    void Start()
    {
        ps.material.shader = Shader.Find(ps.material.shader.name);
    }

    void SetPaticle()
    {

    }
}
