using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LanguageStringData))]
public class LanguageStringDataEditor : UnrealInspector
{
	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		LanguageStringData lsd = (target as LanguageStringData);
		int n = lsd.dataPairs.Count;
		base.OnInspectorGUI();
        bool isDuplicate = false;
        if (n < lsd.dataPairs.Count)
        {
            SerializedProperty serializedProperty = serializedObject.FindProperty("dataPairs");
            for (int i = 0; i < lsd.dataPairs.Count - 2; i++)
            {
                SerializedProperty sp1 = serializedProperty.GetArrayElementAtIndex(i);
                SerializedProperty sp2 = serializedProperty.GetArrayElementAtIndex(i + 1);
                if (sp1.displayName == sp2.displayName)
                {
                    sp2.isExpanded = true;
                    sp2.FindPropertyRelative("data").isExpanded = true;
                    isDuplicate = true;
                }
            }
        }

        //增加数组时，不重新加载
		if (EditorGUI.EndChangeCheck() && !isDuplicate)
		{
			LanguageStringData.ReloadData();
		}
	}
}