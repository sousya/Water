using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using System;

//1��ǳ��ɫ
//2�����ɫ
//3��ǳ��ɫ
//4������ɫ
//5��ǳ��ɫ
//6����ɫ
//7����ɫ
//8��ǳ��ɫ
//9������ɫ
//10������ɫ
//11��ǳ��(��)ɫ
//12�����ɫ
[CreateAssetMenu(fileName = "Level", menuName = "Levels")]
public class LevelCreateCtrl : ScriptableObject
{
    // ƿ������(����ΪtopNum + bottomNum)
    public List<BottleProperty> bottles;
    // ��ǰ�ؿ�����Ϸ����
    public GameType gameType;

    /// <summary>
    /// ƿ�ӵ����Զ���
    /// </summary>
    [System.Serializable]
    public class BottleProperty
    {
        // ƿ����ÿ��ˮ����ɫ��ţ�1-12 ��ʾ��ɫ��>1000 ��ʾ������ߣ�
        // �����޸���ɫ������ͬʱ����changeList
        public List<int> waterSet = new List<int>();
        // �Ƿ�����ˮ��ɫ����ɫ�ʺţ�
        public List<bool> isHide = new List<bool>();
        // ÿ��ˮ�ĸ���״̬������顢ը���ȣ�
        public List<WaterItem> waterItem = new List<WaterItem>();
        // ���ˮ����
        public int numCake = 4;
        // ������ƿ�ӵ�ˮ����ɫ-ͬˮ��ɫ��ţ�0 ��ʾ�����ƣ�
        public int limitColor;
        // ��ȷ�����������ڿ���ƿ�ӵĽ����߼�����Ҫ�ض���ɫ��Ų��ܽ����������ݣ�
        public int lockType;
        // �����ϰ�(�ڵ����������������ײ�����)
        public bool isClearHide, isNearHide, isFreeze;
        public bool isFinish;
        public List<int> bombCounts = new List<int>();


    }
    // ��Ҫ�������ɫ�б��ؿ�Ŀ�꣩
    public List<int> clearList;
    // ���ص���ɫ�б���ʼ���ص���ɫ��ͨ������1003����Ȼ����ʾ��
    public List<int> hideList;
    // ��ǰ�ؿ���ը��������_���ö�Ϊ0��ȷ����; ����
    public int bombNum;
    // ��ǰ�ؿ��ĵ���ʱ���������ڲ�������ģʽ��_���ö�Ϊ0��ȷ����;
    public int countDownNum;
    // ��ǰ�ؿ��ĵ���ʱʱ�䣨����ʱ������ģʽ��_���ö�Ϊ0��ȷ����;
    public float timeCountDown;
    // ����ƿ�ӵ�����������ƿ�Ӳ��֣�
    public int topNum;
    // �ײ�ƿ�ӵ�����������ƿ�Ӳ��֣�
    public int bottomNum;
    // �ùؿ����ڵ���ɫ�任�б�����ĳЩ��������߼�,2001-2006�ĵ��������ã�
    public List<ChangePair> changeList;


}

[Serializable]
public class ChangePair
{
    // �����任�ĵ�������(�任��Ŀ����ɫ)
    public ItemType item;
    // ��Ҫ�任����ɫ���
    public int NeedChangeColor;
}
