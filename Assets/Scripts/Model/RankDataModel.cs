using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using A_PlayerRankData;
using QFramework;
using UnityEngine;

namespace A_PlayerRankData
{
    public struct PlayerRankData
    {
        public readonly string playerName;
        public readonly int trophyCount;

        public PlayerRankData(string name, int trophies)
        {
            playerName = name;
            trophyCount = trophies;
        }
    }
}

public class RankDataModel : AbstractModel
{
    //����ʹ��������������
    private List<PlayerRankData> mRankListData;
    public IReadOnlyList<PlayerRankData> RankDataList => mRankListData;

    //�����¼һ�����а����Ƿ�����״̬
    //����ʱ�����а������ұ���δ����
    //�����а���壬��һ����ȡ������������ȡ��رգ��������ȡ

    private readonly string FILE_PATH_RELATIVE = "RankExcel/Rank.csv";

    protected async override void OnInit()
    {
        mRankListData = new List<PlayerRankData>();
        await LoadRankDataAsync();
        //Debug.Log("�Ѽ���" + mRankListData.Count + "���������ݡ�");
    }

    #region LoadRankCSV

    private async Task LoadRankDataAsync()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, FILE_PATH_RELATIVE);

        string fileContent = "";

#if UNITY_ANDROID && !UNITY_EDITOR
        using (var www = UnityEngine.Networking.UnityWebRequest.Get(filePath))
        {
            var asyncOp = www.SendWebRequest();
            while (!asyncOp.isDone)
                await Task.Yield();

            if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                //Debug.LogError("��ȡ�ļ�ʧ��: " + www.error);
                return;
            }
            else
            {
                fileContent = www.downloadHandler.text;
            }
        }
#else
        if (File.Exists(filePath))
        {
            fileContent = await Task.Run(() => File.ReadAllText(filePath));
        }
        else
        {
            //Debug.LogError("�ļ������ڣ�" + filePath);
            return;
        }
#endif

        ParseCSV(fileContent);
    }

    private void ParseCSV(string content)
    {
        mRankListData.Clear();
        var lines = content.Split('\n');
        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine))
                continue;

            var parts = trimmedLine.Split(',');
            if (parts.Length < 2)
                continue;

            string name = parts[0].Trim();
            if (int.TryParse(parts[1].Trim(), out int trophies))
            {
                mRankListData.Add(new PlayerRankData(name, trophies));
            }
            else
            {
                //Debug.LogWarning($"���ݽ���ʧ�ܣ������ݣ�{line}");
            }
        }
    }

    #endregion

    /// <summary>
    /// ��ȡscore�����а��е�ʵ����������1��ʼ��
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public int GetPlayerRank(int score)
    {
        int left = 0;
        int right = mRankListData.Count - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (mRankListData[mid].trophyCount >= score)
                left = mid + 1;
            else
                right = mid - 1;
        }
        return left + 1;
    }
}
