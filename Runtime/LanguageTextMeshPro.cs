using System;
using System.IO;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

#region UNITY_EDITOR

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(LanguageTextMeshPro.TextData))]
public class TextDataDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 20 * 9;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent title)
    {
        static void NextLine(ref Rect rect, ref Rect lineToggle1)
        {
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            lineToggle1.y += EditorGUIUtility.singleLineHeight + 2;
        }

        LanguageTextMeshPro ltmp = property.serializedObject.targetObject as LanguageTextMeshPro;
        LanguageTextMeshPro.OverrideOptions options = ltmp.overrideOptions;

        GUIContent empty = EditorGUIUtility.TrTextContent(string.Empty);
        Rect line = position;
        line.height = EditorGUIUtility.singleLineHeight;
        Rect lineToggle = line;
        lineToggle.width = 20;
        line.x += lineToggle.width;
        line.width -= lineToggle.width;

        SerializedProperty overriteOptionsProperty = property.serializedObject.FindProperty("overrideOptions");
        float labelWidth = EditorGUIUtility.labelWidth;
        PropertyFieldNoName(lineToggle, overriteOptionsProperty, "isOverrideFontAsset", empty, labelWidth);
        using (new EditorGUI.DisabledScope(!options.isOverrideFontAsset))
        {
            EditorGUI.PropertyField(line, property.FindPropertyRelative("fontAsset"), true);
            NextLine(ref line, ref lineToggle);
            EditorGUI.PropertyField(line, property.FindPropertyRelative("fontMaterial"), true);
        }

        NextLine(ref line, ref lineToggle);
        PropertyFieldNoName(lineToggle, overriteOptionsProperty, "isOverrideFontSize", empty, labelWidth);
        using (new EditorGUI.DisabledScope(!options.isOverrideFontSize))
        {
            SerializedProperty spAutoSize = property.FindPropertyRelative("autoSize");
            using (new EditorGUI.DisabledScope(spAutoSize.boolValue))
            {
                EditorGUI.PropertyField(line, property.FindPropertyRelative("fontSize"), true);
                NextLine(ref line, ref lineToggle);
            }

            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(line, spAutoSize, true);
            NextLine(ref line, ref lineToggle);

            Rect halfLine = line;
            //float labelWidth = EditorGUIUtility.labelWidth;
            halfLine.width = labelWidth;
            EditorGUI.LabelField(halfLine, "Auto Size Options");
            halfLine.x += halfLine.width;
            halfLine.width = (line.width - labelWidth) * .5f;
            EditorGUIUtility.labelWidth = 32;
            EditorGUI.PropertyField(halfLine, property.FindPropertyRelative("fontSizeMin"),
                EditorGUIUtility.TrTextContent("Min"));
            halfLine.x += halfLine.width;
            EditorGUI.PropertyField(halfLine, property.FindPropertyRelative("fontSizeMax"),
                EditorGUIUtility.TrTextContent("Max"));
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.indentLevel--;
        }


        NextLine(ref line, ref lineToggle);
        PropertyFieldNoName(lineToggle, overriteOptionsProperty, "isOverrideFontStyles", empty, labelWidth);
        using (new EditorGUI.DisabledScope(!options.isOverrideFontStyles))
        {
            EditorGUI.PropertyField(line, property.FindPropertyRelative("fontStyle"), true);
        }

        NextLine(ref line, ref lineToggle);
        PropertyFieldNoName(lineToggle, overriteOptionsProperty, "isOverrideCharacterSpacing", empty, labelWidth);
        using (new EditorGUI.DisabledScope(!options.isOverrideCharacterSpacing))
        {
            EditorGUI.PropertyField(line, property.FindPropertyRelative("characterSpacing"), true);
        }

        NextLine(ref line, ref lineToggle);
        PropertyFieldNoName(lineToggle, overriteOptionsProperty, "isOverrideWordSpacing", empty, labelWidth);
        using (new EditorGUI.DisabledScope(!options.isOverrideWordSpacing))
        {
            EditorGUI.PropertyField(line, property.FindPropertyRelative("wordSpacing"), true);
        }

        NextLine(ref line, ref lineToggle);
        PropertyFieldNoName(lineToggle, overriteOptionsProperty, "isOverrideScale", empty, labelWidth);
        using (new EditorGUI.DisabledScope(!options.isOverrideScale))
        {
            EditorGUI.PropertyField(line, property.FindPropertyRelative("scale"), true);
        }
    }

    private static void PropertyFieldNoName(Rect lineToggle, SerializedProperty overriteOptionsProperty,
        string propertyRelative, GUIContent empty,
        float labelWidth)
    {
        EditorGUIUtility.labelWidth = 0;
        EditorGUI.PropertyField(lineToggle, overriteOptionsProperty.FindPropertyRelative(propertyRelative), empty);
        EditorGUIUtility.labelWidth = labelWidth;
    }
}

[CustomPropertyDrawer(typeof(LanguageTextMeshPro.OverrideOptions))]
public class OverrideOptionsDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 20;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent title)
    {
        int toggleWidth = 20;
        Rect line = position;
        GUIContent empty = EditorGUIUtility.TrTextContent(string.Empty);
        LanguageTextMeshPro ltmp = property.serializedObject.targetObject as LanguageTextMeshPro;
        line.width = EditorGUIUtility.labelWidth;
        EditorGUI.PropertyField(line, property, EditorGUIUtility.TrTextContent(property.displayName));
        line.x += EditorGUIUtility.labelWidth;
        line.width = toggleWidth;
        ltmp.overrideOptions.isOverrideFontAsset =
            EditorGUI.ToggleLeft(line, empty, ltmp.overrideOptions.isOverrideFontAsset);
        line.x += toggleWidth;
        ltmp.overrideOptions.isOverrideFontSize =
            EditorGUI.ToggleLeft(line, empty, ltmp.overrideOptions.isOverrideFontSize);
        line.x += toggleWidth;
        ltmp.overrideOptions.isOverrideFontStyles =
            EditorGUI.ToggleLeft(line, empty, ltmp.overrideOptions.isOverrideFontStyles);
        line.x += toggleWidth;
        ltmp.overrideOptions.isOverrideCharacterSpacing =
            EditorGUI.ToggleLeft(line, empty, ltmp.overrideOptions.isOverrideCharacterSpacing);
        line.x += toggleWidth;
        ltmp.overrideOptions.isOverrideWordSpacing =
            EditorGUI.ToggleLeft(line, empty, ltmp.overrideOptions.isOverrideWordSpacing);
        line.x += toggleWidth;
        ltmp.overrideOptions.isOverrideScale = EditorGUI.ToggleLeft(line, empty, ltmp.overrideOptions.isOverrideScale);
    }
}
#endif

#endregion

[RequireComponent(typeof(TextMeshProUGUI))]
public class LanguageTextMeshPro : MonoBehaviourLanguage
{
    [LanguageKey]
    [EnableIf(nameof(isSetText))]
    public string key;

    public bool isSetText = true;

    public OverrideOptions overrideOptions;

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
            textData[i].fontSizeMin = textSelf.fontSizeMin;
            textData[i].fontSizeMax = textSelf.fontSizeMax;
            textData[i].characterSpacing = textSelf.characterSpacing;
            textData[i].wordSpacing = textSelf.wordSpacing;
        }
    }

    public override void SwitchLanguage(int nLanguageIndex)
    {
        #region EDITOR

#if UNITY_EDITOR
        if (!textSelf)
        {
            textSelf = GetComponent<TMP_Text>();
            base.Setup();
        }
#endif

        #endregion

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
        if (overrideOptions.isOverrideFontAsset)
        {
            textSelf.font = data.fontAsset;
            textSelf.fontMaterial = data.fontMaterial;
        }

        if (overrideOptions.isOverrideFontSize && data.fontSize > 0)
        {
            if (data.autoSize)
            {
                textSelf.enableAutoSizing = true;
                if (data.fontSizeMin > 0)
                {
                    textSelf.fontSizeMin = data.fontSizeMin;
                }

                if (data.fontSizeMax > 0)
                {
                    textSelf.fontSizeMax = data.fontSizeMax;
                }
            }
            else
            {
                textSelf.enableAutoSizing = false;
                if (data.fontSize > 0)
                {
                    textSelf.fontSize = data.fontSize;
                }
            }
        }

        if (overrideOptions.isOverrideFontStyles)
        {
            textSelf.fontStyle = data.fontStyle;
        }

        if (overrideOptions.isOverrideCharacterSpacing)
        {
            textSelf.characterSpacing = data.characterSpacing;
        }

        if (overrideOptions.isOverrideWordSpacing)
        {
            textSelf.wordSpacing = data.wordSpacing;
        }

        if (overrideOptions.isOverrideScale)
        {
            textSelf.transform.localScale = data.scale;
        }
    }

    [Serializable]
    public struct OverrideOptions
    {
        [HideInInspector] public bool isOverrideFontAsset;

        [HideInInspector] public bool isOverrideFontSize;

        [HideInInspector] public bool isOverrideFontStyles;

        [HideInInspector] public bool isOverrideCharacterSpacing;

        [HideInInspector] public bool isOverrideWordSpacing;

        [HideInInspector] public bool isOverrideScale;
    }

    [Serializable]
    public struct TextData
    {
        public TMP_FontAsset fontAsset;
        public Material fontMaterial;
        public float fontSize;
        public bool autoSize;
        public float fontSizeMin;
        public float fontSizeMax;
        public FontStyles fontStyle;
        public float characterSpacing;
        public float wordSpacing;
        public Vector3 scale;
    }

#if UNITY_EDITOR
    //Call by UnityRush "NameGo" button
    private void NameGo()
    {
        gameObject.name = isSetText ? $"localizedTMP[{key}]" : "localizedTMP(StyleOnly)";
    }

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

    private void PingDataFolder()
    {
        Object obj = AssetDatabase.LoadMainAssetAtPath("Assets/Localization/Resources");
        if (obj == null)
        {
            return;
        }

        Type type = Assembly.Load("UnityEditor").GetType("UnityEditor.ProjectBrowser");
        FieldInfo fieldInfo =
            type.GetField("s_LastInteractedProjectBrowser", BindingFlags.Static | BindingFlags.Public);
        object lastBrowser = fieldInfo.GetValue(null);
        bool isTwo = (bool) type.GetMethod("IsTwoColumns", BindingFlags.Instance | BindingFlags.NonPublic)
            .Invoke(lastBrowser, null);
        if (isTwo)
        {
            int instanceID = obj.GetInstanceID();
            type.GetMethod("ShowFolderContents", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(lastBrowser, new[] {(object) instanceID, true,});
        }
        else
        {
            EditorGUIUtility.PingObject(obj);
        }
    }

    private static SerializedObject copyer;

    [ButtonEx("PasteStyles", "PingDataFolder")]
    private void CopyStyles()
    {
        copyer = new SerializedObject(this);
    }

    private void PasteStyles()
    {
        if (copyer == null || !copyer.targetObject)
        {
            return;
        }

        SerializedObject self = new SerializedObject(this);
        if (self.targetObject == copyer.targetObject)
        {
            return;
        }

        self.CopyFromSerializedProperty(copyer.FindProperty("textData"));
        self.CopyFromSerializedProperty(copyer.FindProperty("overrideOptions"));
        self.ApplyModifiedProperties();
    }
#endif
}