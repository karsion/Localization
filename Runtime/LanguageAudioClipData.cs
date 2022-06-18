using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static NPinyin.Pinyin;

#region UNITY_EDITOR

#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEditor;
#endif

#endregion

[CreateAssetMenu]
public class LanguageAudioClipData : ScriptableObject
{
	private static LanguageAudioClipData[] lsDatas;

	public static Dictionary<string, AudioClip[]> datas = new Dictionary<string, AudioClip[]>();

	public string findData;

	public string groupName;
	public List<DataPair> dataPairs = new List<DataPair>();

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
					StringBuilder stringBuilder = new StringBuilder($"重复的数据：[{instanceDataPair.key}]").AppendLine()
						.Append(" 文件：");
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

	[Serializable]
	public struct DataPair : IComparable<DataPair>
	{
		public string key;

		public AudioClip[] data;

		//public int CompareTo(DataPair other) => String.CompareOrdinal(key, other.key);
		public int CompareTo(DataPair other)
		{
			string x1 = GetInitials(key);
			string x2 = GetInitials(other.key);
			return string.CompareOrdinal(x1, x2);
		}
	}

	#region UNITY_EDITOR

#if UNITY_EDITOR
	[ButtonEx("SortAlphabetically2", "AddGroupName", "RemoveGroupName")]
	private void SortAlphabetically()
	{
		Undo.RecordObject(this, "Sort");
		dataPairs.Sort();
		EditorUtility.SetDirty(this);
	}

	private void SortAlphabetically2()
	{
		Undo.RecordObject(this, "Sort");
		dataPairs.Sort((pair, dataPair) => pair.data[0].name.CompareTo(dataPair.data[0].name));
		EditorUtility.SetDirty(this);
	}

	private void PrintDataName()
	{
		for (int j = 0; j < dataPairs.Count; j++)
		{
			DataPair instanceDataPair = dataPairs[j];
			Debug.Log(instanceDataPair.data[0].name);
		}
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