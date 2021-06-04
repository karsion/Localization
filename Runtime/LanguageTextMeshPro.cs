using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class LanguageTextMeshPro : MonoBehaviourLanguage
{
	[Serializable]
	public struct TextData
	{
		//[Multiline]
		//public string text;

		public TMP_FontAsset fontAsset;
		public Material fontMaterial;
		public float fontSize;
		public float characterSpacing;
		public float wordSpacing;
        public Vector3 scale;
	}

	public bool isSetText = true;
	[EnableIf(nameof(isSetText))]
	public string key;
	public bool isOverrideFontAsset;
	public bool isOverrideFontSize;
	public bool isOverrideCharacterSpacing;
	public bool isOverrideWordSpacing;
    public bool isOverrideScale;
	public TextData[] textData = new TextData[0];
	public TMP_Text textSelf;

	public override void SwitchLanguage(int nLanguage)
	{
		if (nLanguage >= textData.Length)
		{
			Debug.LogError(" SwitchLanguage Error, GameObject：" + transform.GetPath());
			return;
		}

		TextData data = textData[nLanguage];
		if (isSetText)
		{
			textSelf.text = LanguageManager.GetLanguageStringData(key, nLanguage);
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

	[ButtonEx()]
	private void PingDataFloder()
	{
		Object obj = AssetDatabase.LoadMainAssetAtPath("Assets/Localization/Resources");
		if (obj == null) return;
		EditorGUIUtility.PingObject(obj);
		Selection.activeObject = obj;
	}
#endif
}
