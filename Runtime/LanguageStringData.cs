#region UNITY_EDITOR

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using static NPinyin.Pinyin;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

[CreateAssetMenu]
public class LanguageStringData : ScriptableObject
{
	[LanguageKey]
	public string search;
	private static LanguageStringData[] lsDatas;

	public static Dictionary<string, string[]> datas = new Dictionary<string, string[]>();

	public string groupName;
	public List<DataPair> dataPairs = new List<DataPair>();

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
				if (datas.ContainsKey(dataPairs[j].key))
				{
					#region EDITOR

#if UNITY_EDITOR
					//查找哪几个文件重复了
					StringBuilder stringBuilder = new StringBuilder($"重复的数据：[{instanceDataPair.key}]").AppendLine()
						.Append(" 文件：");
					for (int k = 0; k < lsDatas.Length; k++)
					{
						LanguageStringData stringData = lsDatas[k];
						DataPair instanceDataPairTemp = dataPairs[j];
						if (stringData.dataPairs.Exists(d => d.key == instanceDataPairTemp.key))
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

	[Serializable]
	public struct DataPair : IComparable<DataPair>
	{
		public string key;

		[Multiline(2)] public string[] data;

		public int CompareTo(DataPair other)
		{
			string x1 = GetInitials(key);
			string x2 = GetInitials(other.key);
			return string.CompareOrdinal(x1, x2);
		}
	}

	#region UNITY_EDITOR

#if UNITY_EDITOR
	[ButtonEx("Eng")]
	protected void Chs() { LanguageManager.SetLanguage(0); }

	protected void Eng() { LanguageManager.SetLanguage(1); }

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

	[ButtonEx("LoadFromJson", "OpenJsonFile")]
	public void SaveToJson() { JsonSavesHelper.GetSave(name + ".json").SaveFile(dataPairs); }

	public void LoadFromJson()
	{
		if (EditorUtility.DisplayDialog("注意！", "从文件加载会丢失当前设置！", "加载", "取消"))
		{
			JsonSavesHelper.GetSave(name + ".json").ReadFile(ref dataPairs);
		}
	}

	public void OpenJsonFile() { Process.Start(Path.Combine(PathHelper.strSaveDataPath, name + ".json")); }

    internal static void PingData(string key)
    {
        LanguageStringData lsdDefault = null;
        List<string> strResults = new List<string>();
        string[] dataFiles = Directory.GetFiles(Application.dataPath + "/Localization/Resources/", "*",
            SearchOption.AllDirectories);
        //循环遍历每一个路径，单独加载
        foreach (string path in dataFiles)
        {
            //替换路径中的反斜杠为正斜杠       
            string strTempPath = path.Replace(@"\", "/");

            //截取我们需要的路径
            strTempPath = strTempPath.Substring(strTempPath.IndexOf("Assets"));

            //根据路径加载资源
            Object obj = AssetDatabase.LoadAssetAtPath(@strTempPath, typeof(Object));
            LanguageStringData lsd = obj as LanguageStringData;
            if (!lsd)
            {
                continue;
            }

            if (lsd.name == "_Default")
            {
                lsdDefault = lsd;
            }

            SerializedObject so = new SerializedObject(obj);
            bool isHit = false;
            SerializedProperty spHit = null;
            SerializedProperty spDataPairs = so.FindProperty("dataPairs");
            for (int j = 0; j < lsd.dataPairs.Count; j++)
            {
                DataPair lsdDataPair = lsd.dataPairs[j];
                SerializedProperty spData = spDataPairs.GetArrayElementAtIndex(j);
                spData.isExpanded = false;

                if (lsdDataPair.key == key)
                {
                    spHit = spData;
                    isHit = true;
                }

                if (lsdDataPair.key.Contains(key))
                {
	                strResults.Add($"{lsd.name}.{lsdDataPair.key}");
                }
            }

            if (isHit)
            {
                Selection.activeObject = null;

                EditorApplication.delayCall += () =>
                {
	                EditorGUIUtility.PingObject(obj);
	                Selection.activeObject = obj;
	                spHit.isExpanded = true;
                };

                return;
            }
        }

        Debug.LogWarning($"没有找到这个Key => [{key}]");
        for (int i = 0; i < strResults.Count; i++)
        {
	        Debug.Log(strResults[i]);
        }
        
        Debug.LogWarning($"以上是找到的相关结果");
        
        bool isCreate = EditorUtility.DisplayDialog("本地化管理器提示", $"没有找到这个Key => [{key}]", "创建", "取消");
        if (isCreate && lsdDefault)
        {
            lsdDefault.dataPairs.Add(new DataPair() {key = key, data = new string[LanguageManager.nLanguageCount],});
            SerializedObject so = new SerializedObject(lsdDefault);
            // bool isHit = false;
            for (int j = 0; j < lsdDefault.dataPairs.Count; j++)
            {
                DataPair lsdDataPair = lsdDefault.dataPairs[j];
                SerializedProperty sp = so.FindProperty("dataPairs");
                sp = sp.GetArrayElementAtIndex(j);
                sp.isExpanded = lsdDataPair.key == key;
            }

            EditorGUIUtility.PingObject(lsdDefault);
            Selection.activeObject = lsdDefault;
        }
    }

	[InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif

	#endregion

	private static void Init()
	{
		ReloadData();
#if UNITY_EDITOR
		EditorApplication.playModeStateChanged += mode =>
		{
			if (mode != PlayModeStateChange.EnteredEditMode) { return; }

			ReloadData();
			InternalEditorUtility.RepaintAllViews();
		};
#endif
	}
}