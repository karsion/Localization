using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;


#region UNITY_EDITOR

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(LanguageText.TextData))]
public class TextTextDataDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
        return 20 * 4;
    }

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent title)
	{
		static void NextLine(ref Rect rect, ref Rect lineToggle1)
		{
			rect.y += EditorGUIUtility.singleLineHeight + 2;
			lineToggle1.y += EditorGUIUtility.singleLineHeight + 2;
		}

		LanguageText ltmp = property.serializedObject.targetObject as LanguageText;
		GUIContent empty = EditorGUIUtility.TrTextContent(string.Empty);
		Rect line = position;
		line.height = EditorGUIUtility.singleLineHeight;
		Rect lineToggle = line;
		lineToggle.width = 20;
		line.x += lineToggle.width;
		line.width -= lineToggle.width;
		ltmp.isOverrideFontAsset = EditorGUI.ToggleLeft(lineToggle, empty, ltmp.isOverrideFontAsset);
		using (new EditorGUI.DisabledScope(!ltmp.isOverrideFontAsset))
		{
			EditorGUI.PropertyField(line, property.FindPropertyRelative("fontMaterial"), true);
		}

		NextLine(ref line, ref lineToggle);
		ltmp.isOverrideFontSize = EditorGUI.ToggleLeft(lineToggle, empty, ltmp.isOverrideFontSize);
		using (new EditorGUI.DisabledScope(!ltmp.isOverrideFontSize))
		{
			EditorGUI.PropertyField(line, property.FindPropertyRelative("fontSize"), true);
		}

		NextLine(ref line, ref lineToggle);
		ltmp.isOverrideLineSpacing = EditorGUI.ToggleLeft(lineToggle, empty, ltmp.isOverrideLineSpacing);
		using (new EditorGUI.DisabledScope(!ltmp.isOverrideLineSpacing))
		{
			EditorGUI.PropertyField(line, property.FindPropertyRelative("lineSpacing"), true);
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

[RequireComponent(typeof(Text))]
public class LanguageText : MonoBehaviourLanguage
{
    public bool isSetText = true;

    [LanguageKey]
    [EnableIf(nameof(isSetText))]
    public string key;
    
    [HideInInspector] public bool isOverrideFontAsset;

    [HideInInspector] public bool isOverrideFontSize;

    [HideInInspector] public bool isOverrideLineSpacing;

    [HideInInspector] public bool isOverrideScale;

    [Serializable]
    public struct TextData
    {
        public Material fontMaterial;
        public int fontSize;
        public float lineSpacing;
        public Vector3 scale;
    }

    //[SerializeField]
    public Text textSelf;
    public TextData[] textData = new TextData[0];

    public override void SwitchLanguage(int nLanguageIndex)
    {
        if (nLanguageIndex >= textData.Length)
        {
            Debug.LogError(" SwitchLanguage Error, GameObject：" + transform.GetPath());
            return;
        }

        //textSelf.text = strText[nLanguageIndex];
        TextData data = textData[nLanguageIndex];
        if (isSetText)
        {
            textSelf.text = LanguageManager.GetText(key, nLanguageIndex);
        }

        UpdateLanguage(data);
    }

    //自动建立数组，并引用Text中的text
    private void Reset()
    {
        //strText = new string[LanguageManager.nLanguageCount];
        textSelf = GetComponent<Text>();
        if (!textSelf) { return; }

        //strText[0] = textSelf.text;
        textData = new TextData[LanguageManager.nLanguageCount];
        for (int i = 0; i < textData.Length; i++)
        {
            textData[i].scale = textSelf.transform.localScale;
            textData[i].fontSize = textSelf.fontSize;
            textData[i].lineSpacing = textSelf.lineSpacing;
        }
    }

    private void UpdateLanguage(TextData data)
    {
        if (isOverrideFontAsset)
        {
            textSelf.material = data.fontMaterial;
        }

        if (isOverrideFontSize && data.fontSize > 0)
        {
            textSelf.fontSize = data.fontSize;
        }

        if (isOverrideLineSpacing)
        {
            textSelf.lineSpacing = data.lineSpacing;
        }

        if (isOverrideScale)
        {
            textSelf.transform.localScale = data.scale;
        }
    }

#if UNITY_EDITOR
    //Call by UnityRush "NameGo" button
    private void NameGo() { gameObject.name = isSetText ? $"localizedText[{key}]" : "localizedText(StyleOnly)"; }

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