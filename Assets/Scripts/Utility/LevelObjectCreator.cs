using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// һ���������࣬�������ٵ����ɴ�����������Ҫ���scriptsObject
/// </summary>
public class LevelObjectCreator : EditorWindow
{
#region ���ɵ���Ʒ�Ļ�������
    private string baseName = "Level";

    private int startNum = 1;

    private int endNum = 1;

    private string folderPath = "Assets/Scripts/Level/";

    // ��δʹ��
    private string objectType = null;
    #endregion

    private LevelManager levelManager;
        
    [MenuItem("Tools/Create Level Obeject Assets")]
    public static void ShowWindow()
    {
        GetWindow<LevelObjectCreator>("Level Obeject Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Object Creation Settings", EditorStyles.boldLabel);

        baseName = EditorGUILayout.TextField("Base Name:", baseName);
        startNum = EditorGUILayout.IntField("Start Number:", startNum);
        endNum = EditorGUILayout.IntField("End Number:", endNum);
        folderPath = EditorGUILayout.TextField("Folder Path:", folderPath);
        objectType = EditorGUILayout.TextField("Object Type:", objectType);
        if (GUILayout.Button("Create Level Data Assets"))
        {
            CreateLevelObject();
        }
        if (GUILayout.Button("Batch Level Object too LevelManager"))
        {
            BatchLevelToManager();
        }
    
}

    /// <summary>
    /// ������������
    /// </summary>
    private void CreateLevelObject()
    {
        
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            AssetDatabase.Refresh();
        }
       
        for (int i = startNum; i <= endNum; i++)
        {
            
            string assetName = $"{baseName}{i}";
            string path = $"{folderPath}/{assetName}.asset";

            // ����Ƿ��Ѵ���ͬ���ʲ�
            if (File.Exists(path))
            {
                Debug.LogWarning($"Asset already exists at {path}, skipping...");
                continue;
            }
            
            // �����µ� ScriptableObject
            LevelCreateCtrl levelObject = CreateInstance<LevelCreateCtrl>();
            levelObject.topNum = 5;
            levelObject.bottomNum = 5;
           

            // �����ʲ�
            AssetDatabase.CreateAsset(levelObject, path);
            AssetDatabase.SaveAssets();
        }

        AssetDatabase.Refresh();
        Debug.Log($"Successfully created {endNum-startNum+1} LevelCreateCtrl assets in {folderPath}");
    }
    /// <summary>
    /// �������ű���ֵobject��δ���
    /// </summary>
    private void BatchLevelToManager()
    {



        for(int i= startNum;i<=endNum;i++)
        {


        }
    }
}


