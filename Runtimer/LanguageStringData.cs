// // Copyright: Shenzhen Magic Tree Games Studio
// // Author: Nice

using System;
using System.Collections.Generic;

#region UNITY_EDITOR

#if UNITY_EDITOR
using UnityEditor;
#endif

#endregion
using UnityEngine;

[CreateAssetMenu]
public class LanguageStringData : ScriptableObject
{
	#region UNITY_EDITOR

#if UNITY_EDITOR
	[ButtonEx("Eng")]
	protected void Chs() { LanguageManager.SetLanguage(0); }

	protected void Eng() { LanguageManager.SetLanguage(1); }

	[InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod]
#endif
	#endregion
	private static void Init()
	{
		datas.Clear();
		LanguageStringData[] lsDatas = Resources.FindObjectsOfTypeAll<LanguageStringData>();
		for (int i = 0; i < lsDatas.Length; i++)
		{
			LanguageStringData languageStringData = lsDatas[i];
			List<DataPair> dataPairs = languageStringData.dataPairs;
			for (int j = 0; j < dataPairs.Count; j++)
			{
				DataPair instanceDataPair = dataPairs[j];
				if (datas.ContainsKey(instanceDataPair.key))
				{
					Debug.LogError($"重复的数据：{instanceDataPair.key}");
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

		[Multiline]
		public string[] data;
	}

	public List<DataPair> dataPairs = new List<DataPair>();

	private void OnValidate() { Init(); }
}