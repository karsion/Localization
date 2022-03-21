using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LanguageStringData))]
public class LanguageStringDataEditor : UnrealInspector
{
	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		base.OnInspectorGUI();
		serializedObject.Update();
		SerializedProperty spDataPairs = serializedObject.FindProperty("dataPairs");
		for (int i = 0; i < spDataPairs.arraySize - 1; i++)
		{
			SerializedProperty sp1 = spDataPairs.GetArrayElementAtIndex(i);
			SerializedProperty sp2 = spDataPairs.GetArrayElementAtIndex(i + 1);
			if (sp1.displayName == sp2.displayName)
			{
				sp2.isExpanded = true;
				sp2.FindPropertyRelative("key").stringValue += "(Clone)";
				sp2.FindPropertyRelative("data").isExpanded = true;
			}
		}

		serializedObject.ApplyModifiedProperties();
		//增加数组时，不重新加载
		if (EditorGUI.EndChangeCheck()) { LanguageStringData.ReloadData(); }
	}
}