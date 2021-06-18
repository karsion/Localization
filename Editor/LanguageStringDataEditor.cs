using UnityEditor;

[CustomEditor(typeof(LanguageStringData))]
public class LanguageStringDataEditor : UnrealInspector
{
	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		LanguageStringData lsd = (target as LanguageStringData);
		int n = lsd.dataPairs.Count;
		base.OnInspectorGUI();
		if (n < lsd.dataPairs.Count)
		{
			SerializedProperty serializedProperty = serializedObject.FindProperty("dataPairs");
			serializedProperty.GetArrayElementAtIndex(lsd.dataPairs.Count - 1).isExpanded = true;
		}

		//增加数组时，不重新加载
		if (EditorGUI.EndChangeCheck() && n >= lsd.dataPairs.Count)
		{
			LanguageStringData.ReloadData();
		}
	}
}