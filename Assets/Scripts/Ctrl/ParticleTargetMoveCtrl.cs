using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleTargetMoveCtrl : MonoBehaviour
{
    private ParticleSystem par;
    private ParticleSystem.Particle[] arrPar;
    private int arrCount;
    private Vector3 wPos;

    [Header("�����ƶ�Ŀ��")]
    public Transform target;
    [Header("�����ƶ������ٶ�")]
    public float speed = 0.1f;
    [Header("�ӳټ���ʱ�䣨�룩")]
    public float delayTime = 1f;
    [Header("����ϵͳԭʼ��������")]
    public float originEmissionRate;
    [Header("�����Ƿ񼤻��ƶ�״̬")]
    public bool isActive;

    [Header("�ٶȵ���ֵ")]
    public float speedAdd;
    [Header("�ٶȵ�����������")]
    public float speedAddDelta = 2f;
    [Header("�Ƿ�һ���Ա�������ģʽ��Burst��")]
    public bool oncePar = false;

    private void Awake()
    {
        par = GetComponent<ParticleSystem>();
        arrPar = new ParticleSystem.Particle[par.main.maxParticles];
        speedAdd = 0f;
        if (oncePar)
            originEmissionRate = par.emission.GetBurst(0).count.curveMultiplier;
        else
            originEmissionRate = par.emission.rateOverTimeMultiplier;
    }

    private void setActive()
    {
        isActive = true;
    }

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
        // ��ȡ��ǰ�������������
        arrCount = par.GetParticles(arrPar);
        // ���û�����ӣ�ֹͣ����ϵͳ������״̬
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

        // �������ӣ�����wPos
        for (int i = 0; i < arrCount; i++)
        {
            // ��ǰ���ӵ�Ŀ�������
            Vector3 vector = wPos - arrPar[i].position;
            // �����������С�ڵ����ٶȣ�������λ������ΪĿ��λ�ò���������������
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
        wPos = target.transform.position;
        // �ӳټ���
        Invoke("setActive", delayTime + 0.01f);

        if (par == null)
        {
            par = this.GetComponent<ParticleSystem>();
        }

        if (oncePar)
        {
            var b = par.emission.GetBurst(0);
            b.count = new ParticleSystem.MinMaxCurve(originEmissionRate);
            par.emission.SetBurst(0, b);
        }
        else
        {
            var emis = par.emission;
            emis.rateOverTimeMultiplier = originEmissionRate;
        }

        speedAdd = 0;
        isActive = false;
        par.Stop();
        par.Emit(emit_count);
    }
}