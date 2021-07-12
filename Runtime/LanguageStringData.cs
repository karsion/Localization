﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#region UNITY_EDITOR

#if UNITY_EDITOR
using UnityEditor;
#endif

#endregion

using UnityEngine;

[CreateAssetMenu]
public class LanguageStringData : ScriptableObject
{
    private static LanguageStringData[] lsDatas;

    #region UNITY_EDITOR

#if UNITY_EDITOR
    [ButtonEx("Eng")]
    protected void Chs()
    {
        LanguageManager.SetLanguage(0);
    }

    protected void Eng()
    {
        LanguageManager.SetLanguage(1);
    }

    [ButtonEx("LoadFromJson", "OpenJsonFile")]
    public void SaveToJson()
    {
        JsonSavesHelper.GetSave(name + ".json").SaveFile(dataPairs);
    }

    public void LoadFromJson()
    {
        if (EditorUtility.DisplayDialog("注意！", "从文件加载会丢失当前设置！", "加载", "取消"))
            JsonSavesHelper.GetSave(name + ".json").ReadFile(ref dataPairs);
    }

    public void OpenJsonFile()
    {
        System.Diagnostics.Process.Start(Path.Combine(PathHelper.strSaveDataPath, name + ".json"));
    }

    [InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod]
#endif

    #endregion

    private static void Init()
    {
        ReloadData();
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += mode =>
        {
            if (mode != PlayModeStateChange.EnteredEditMode) return;
            ReloadData();
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        };
#endif
    }

    public static void ReloadData()
    {
        lsDatas = Resources.LoadAll<LanguageStringData>(string.Empty);
        datas.Clear();
        for (int i = 0; i < lsDatas.Length; i++)
        {
            LanguageStringData languageStringData = lsDatas[i];
            List<DataPair> dataPairs = languageStringData.dataPairs;
            for (int j = 0; j < dataPairs.Count; j++)
            {
                DataPair instanceDataPair = dataPairs[j];
                if (datas.ContainsKey(instanceDataPair.key))
                {
                    #region EDITOR

#if UNITY_EDITOR
                    //查找哪几个文件重复了
                    StringBuilder stringBuilder = new StringBuilder($"重复的数据：[{instanceDataPair.key}]").AppendLine().Append(" 文件：");
                    for (int k = 0; k < lsDatas.Length; k++)
                    {
                        LanguageStringData stringData = lsDatas[k];
                        if (stringData.dataPairs.Exists(d => d.key == instanceDataPair.key))
                        {
                            stringBuilder.Append(" [").Append(stringData.name).Append("] ");
                        }
                    }

                    Debug.LogError(stringBuilder.ToString());
#endif

                    #endregion

                    continue;
                }

                datas.Add(instanceDataPair.key, instanceDataPair.data);
            }
        }
    }

    public static Dictionary<string, string[]> datas = new Dictionary<string, string[]>();

    [Serializable]
    public struct DataPair
    {
        public string key;

        [Multiline(2)] public string[] data;
    }

    public List<DataPair> dataPairs = new List<DataPair>();
}