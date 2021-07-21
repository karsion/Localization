using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[RequireComponent(typeof(Text))]
public class LanguageText : MonoBehaviourLanguage
{
    public bool isSetText = true;

    [EnableIf(nameof(isSetText))] public string key;
    
    [HideInInspector] public bool isOverrideFontAsset;

    [HideInInspector] public bool isOverrideFontSize;

    [HideInInspector] public bool isOverrideLineSpacing;

    [HideInInspector] public bool isOverrideScale;

    [Multiline] public string[] strText;

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
        if (nLanguageIndex >= strText.Length)
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
        strText = new string[LanguageManager.nLanguageCount];
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