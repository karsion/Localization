using System;
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
public class LanguageAudioClipData : ScriptableObject
{
    private static LanguageAudioClipData[] lsDatas;

    #region UNITY_EDITOR

#if UNITY_EDITOR
    [ButtonEx("AddGroupName", "RemoveGroupName")]
    private void SortAlphabetically()
    {
	    Undo.RecordObject(this, "Sort");
	    dataPairs.Sort();
	    EditorUtility.SetDirty(this);
    }

    private void AddGroupName()
    {
	    for (int j = 0; j < dataPairs.Count; j++)
	    {
		    DataPair instanceDataPair = dataPairs[j];
		    if (!string.IsNullOrEmpty(groupName))
		    {
			    if (!instanceDataPair.key.StartsWith(groupName))
			    {
					instanceDataPair.key = groupName + instanceDataPair.key;
					dataPairs[j] = instanceDataPair;
			    }
		    }
	    }
    }

    private void RemoveGroupName()
    {
	    for (int j = 0; j < dataPairs.Count; j++)
	    {
		    DataPair instanceDataPair = dataPairs[j];
		    if (!string.IsNullOrEmpty(groupName))
		    {
			    if (instanceDataPair.key.StartsWith(groupName))
			    {
				    instanceDataPair.key = instanceDataPair.key.Remove(0, groupName.Length);
				    dataPairs[j] = instanceDataPair;
			    }
		    }
	    }
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
        lsDatas = Resources.LoadAll<LanguageAudioClipData>(string.Empty);
        datas.Clear();
        for (int i = 0; i < lsDatas.Length; i++)
        {
	        LanguageAudioClipData languageStringData = lsDatas[i];
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
	                    LanguageAudioClipData stringData = lsDatas[k];
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

    public static Dictionary<string, AudioClip[]> datas = new Dictionary<string, AudioClip[]>();

    [Serializable]
    public struct DataPair : IComparable<DataPair>
    {
        public string key;
        public AudioClip[] data;
        public int CompareTo(DataPair other) => String.CompareOrdinal(key, other.key);
    }

    public string groupName;
    public List<DataPair> dataPairs = new List<DataPair>();
}