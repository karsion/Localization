using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#region UNITY_EDITOR

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(LanguageTextMeshPro.TextData))]
public class TextDataDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUIUtility.singleLineHeight * 7;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent title)
	{
		static void NextLine(ref Rect rect, ref Rect lineToggle1)
		{
			rect.y += EditorGUIUtility.singleLineHeight + 2;
			lineToggle1.y += EditorGUIUtility.singleLineHeight + 2;
		}

		LanguageTextMeshPro ltmp = property.serializedObject.targetObject as LanguageTextMeshPro;
		GUIContent empty = EditorGUIUtilityEx.TempContent(string.Empty);
		Rect line = position;
		line.height = EditorGUIUtility.singleLineHeight;
		Rect lineToggle = line;
		lineToggle.width = 20;
		line.x += lineToggle.width;
		line.width -= lineToggle.width;
		ltmp.isOverrideFontAsset = EditorGUI.ToggleLeft(lineToggle, empty, ltmp.isOverrideFontAsset);
		using (new EditorGUI.DisabledScope(!ltmp.isOverrideFontAsset))
		{
			EditorGUI.PropertyField(line, property.FindPropertyRelative("fontAsset"), true);
			NextLine(ref line, ref lineToggle);
			EditorGUI.PropertyField(line, property.FindPropertyRelative("fontMaterial"), true);
		}

		NextLine(ref line, ref lineToggle);
		ltmp.isOverrideFontSize = EditorGUI.ToggleLeft(lineToggle, empty, ltmp.isOverrideFontSize);
		using (new EditorGUI.DisabledScope(!ltmp.isOverrideFontSize))
		{
			EditorGUI.PropertyField(line, property.FindPropertyRelative("fontSize"), true);
		}

		NextLine(ref line, ref lineToggle);
		ltmp.isOverrideCharacterSpacing = EditorGUI.ToggleLeft(lineToggle, empty, ltmp.isOverrideCharacterSpacing);
		using (new EditorGUI.DisabledScope(!ltmp.isOverrideCharacterSpacing))
		{
			EditorGUI.PropertyField(line, property.FindPropertyRelative("characterSpacing"), true);
		}

		NextLine(ref line, ref lineToggle);
		ltmp.isOverrideWordSpacing = EditorGUI.ToggleLeft(lineToggle, empty, ltmp.isOverrideWordSpacing);
		using (new EditorGUI.DisabledScope(!ltmp.isOverrideWordSpacing))
		{
			EditorGUI.PropertyField(line, property.FindPropertyRelative("wordSpacing"), true);
		}

		NextLine(ref line, ref lineToggle);
		ltmp.isOverrideScale = EditorGUI.ToggleLeft(lineToggle, empty, ltmp.isOverrideScale);
		using (new EditorGUI.DisabledScope(!ltmp.isOverrideScale))
		{
			EditorGUI.PropertyField(line, property.FindPropertyRelative("scale"), true);
		}
	}
}
#endif

#endregion

public class LanguageTextMeshPro : MonoBehaviourLanguage
{
	public bool isSetText = true;

	[EnableIf(nameof(isSetText))] public string key;

	[HideInInspector] public bool isOverrideFontAsset;

	[HideInInspector] public bool isOverrideFontSize;

	[HideInInspector] public bool isOverrideCharacterSpacing;

	[HideInInspector] public bool isOverrideWordSpacing;

	[HideInInspector] public bool isOverrideScale;

	public TextData[] textData = new TextData[0];
	public TMP_Text textSelf;

	//自动建立数组，并引用Text中的text
	private void Reset()
	{
		textSelf = GetComponent<TMP_Text>();
		if (!textSelf)
		{
			return;
		}

		if (textData != null && textData.Length > 0)
		{
			return;
		}

		textData = new TextData[LanguageManager.nLanguageCount];
		for (int i = 0; i < textData.Length; i++)
		{
			textData[i].scale = textSelf.transform.localScale;
			textData[i].fontSize = textSelf.fontSize;
			textData[i].characterSpacing = textSelf.characterSpacing;
			textData[i].wordSpacing = textSelf.wordSpacing;
		}
	}

	public override void SwitchLanguage(int nLanguageIndex)
	{
		if (nLanguageIndex >= textData.Length)
		{
			Debug.LogError(" SwitchLanguage Error, GameObject：" + transform.GetPath());
			return;
		}

		TextData data = textData[nLanguageIndex];
		if (isSetText)
		{
			textSelf.text = LanguageManager.GetText(key, nLanguageIndex);
		}

		UpdateLanguage(data);
	}

	private void UpdateLanguage(TextData data)
	{
		if (isOverrideFontAsset)
		{
			textSelf.font = data.fontAsset;
			textSelf.fontMaterial = data.fontMaterial;
		}

		if (isOverrideFontSize && data.fontSize > 0)
		{
			textSelf.fontSize = data.fontSize;
		}

		if (isOverrideCharacterSpacing)
		{
			textSelf.characterSpacing = data.characterSpacing;
		}

		if (isOverrideWordSpacing)
		{
			textSelf.wordSpacing = data.wordSpacing;
		}

		if (isOverrideScale)
		{
			textSelf.transform.localScale = data.scale;
		}
	}

	[Serializable]
	public struct TextData
	{
		public TMP_FontAsset fontAsset;
		public Material fontMaterial;
		public float fontSize;
		public float characterSpacing;
		public float wordSpacing;
		public Vector3 scale;
	}

#if UNITY_EDITOR
	//Call by UnityRush "NameGo" button
	private void NameGo() { gameObject.name = isSetText ? $"localizedTMP[{key}]" : "localizedTMP(StyleOnly)"; }

	public override void Refresh()
	{
		for (int i = 0; i < textData.Length; i++)
		{
			if (textData[i].scale == Vector3.zero)
			{
				textData[i].scale = Vector3.one;
			}
		}

		base.Refresh();
	}

	protected override void Setup()
	{
		Reset();
		base.Setup();
	}

	[ButtonEx]
	private void PingDataFloder()
	{
		Object obj = AssetDatabase.LoadMainAssetAtPath("Assets/Localization/Resources");
		if (obj == null) return;
		EditorGUIUtility.PingObject(obj);
		Selection.activeObject = obj;
	}
#endif
}