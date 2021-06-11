using UnityEditor;

[CustomEditor(typeof(LanguageStringData))]
public class LanguageStringDataEditor : Editor
{
	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		base.OnInspectorGUI();
		if (EditorGUI.EndChangeCheck())
		{
			LanguageStringData.ReloadData();
		}
	}
}