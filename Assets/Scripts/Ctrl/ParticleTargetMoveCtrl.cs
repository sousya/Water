using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleTargetMoveCtrl : MonoBehaviour
{

    private ParticleSystem par;

    private ParticleSystem.Particle[] arrPar;

    private int arrCount;

    public Transform target;

    public float speed = 0.1f;

    public float delayTime = 1f;

    public float originEmissionRate;

    public bool isActive;

    //public Vector3 pos = Vector3.zero;

    public float speedAdd;

    public float speedAddDelta = 2f;

    public bool oncePar = false;

    private Vector3 wPos;



    private void Awake()
    {
        par = this.GetComponent<ParticleSystem>();
        arrPar = new ParticleSystem.Particle[par.main.maxParticles];
        speedAdd = 0f;
        if (oncePar)
        {
            originEmissionRate = par.emission.GetBurst(0).count.curveMultiplier;
        }
        else
        {
            originEmissionRate = par.emission.rateOverTimeMultiplier;//par.emissionRate;
        }

    }

    private void setActive()
    {
        isActive = true;
    }

    //Dictionary<int, List<Vector3>> pointDic = new Dictionary<int, List<Vector3>>();

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Play(10);
        }

        if (!isActive || !par)
        {
            return;
        }
        arrCount = par.GetParticles(arrPar);
        if (arrCount < 1)
        {
            isActive = false;
            par.Stop();
            speedAdd = 0f;
        }
        else
        {
            speedAdd += Time.unscaledDeltaTime * speedAddDelta;
        }
        for (int i = 0; i < arrCount; i++)
        {
             Vector3 vector = wPos - arrPar[i].position;
            if (vector.magnitude <= (speed + speedAdd) * Time.unscaledDeltaTime)
            {
                arrPar[i].position = wPos;
                arrPar[i].remainingLifetime = 0f;
                //par.emissionRate = 0f;
                var emis = par.emission;
                emis.rateOverTimeMultiplier = 0;
            }
            else
            {
                //arrPar[i].position += vector.normalized * (speed + speedAdd) * Time.unscaledDeltaTime;
                arrPar[i].position += vector.normalized * (speed + speedAdd);
                //arrPar[i].position = Vector3.Lerp(arrPar[i].position, Vector3.zero, this.speed * Time.unscaledDeltaTime);
            }
        }
        par.SetParticles(arrPar, arrCount);


    }

    public void Play(Vector3 _pos, int emit_count)
    {
        //if (par.isStopped)
        //{
        wPos = _pos;
        Invoke("setActive", delayTime + 0.01f);
        if (par == null)
        {
            par = this.GetComponent<ParticleSystem>();
        }
        if (oncePar)
        {
            var b = par.emission.GetBurst(0);
            b.count = new ParticleSystem.MinMaxCurve(originEmissionRate);
            //b.minCount = (short)originEmissionRate;
            //b.maxCount = (short)originEmissionRate;
            par.emission.SetBurst(0, b);
        }
        else
        {
            //par.emissionRate = originEmissionRate;
            var emis = par.emission;
            emis.rateOverTimeMultiplier = originEmissionRate;
        }
        speedAdd = 0;
        isActive = false;
        par.Stop();
        //par.Play(withChildren: true);
        //par.Play();
        par.Emit(emit_count);
        //pointDic.Clear();
        //}
    }

    public void Play(int emit_count)
    {
        //if (par.isStopped)
        //{
        wPos = target.transform.position;
        Invoke("setActive", delayTime + 0.01f);
        if (par == null)
        {
            par = this.GetComponent<ParticleSystem>();
        }
        if (oncePar)
        {
            var b = par.emission.GetBurst(0);
            b.count = new ParticleSystem.MinMaxCurve(originEmissionRate);
            //b.minCount = (short)originEmissionRate;
            //b.maxCount = (short)originEmissionRate;
            par.emission.SetBurst(0, b);
        }
        else
        {
            //par.emissionRate = originEmissionRate;
            var emis = par.emission;
            emis.rateOverTimeMultiplier = originEmissionRate;
        }
        speedAdd = 0;
        isActive = false;
        par.Stop();
        //par.Play(withChildren: true);
        //par.Play();
        par.Emit(emit_count);
        //pointDic.Clear();
        //}
    }
}