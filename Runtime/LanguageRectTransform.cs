using System;
using UnityEngine;
using UnityEngine.Serialization;

#region UNITY_EDITOR

#if UNITY_EDITOR
using UnityEditor;


[CustomPropertyDrawer(typeof(LanguageRectTransform.RectTransformData))]
public class RectTransformDataDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 20 * 3;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent title)
	{
		static void NextLine(ref Rect rect, ref Rect lineToggle1)
		{
			rect.y += EditorGUIUtility.singleLineHeight + 2;
			lineToggle1.y += EditorGUIUtility.singleLineHeight + 2;
		}

		LanguageRectTransform languageRectTransform = property.serializedObject.targetObject as LanguageRectTransform;
		GUIContent empty = EditorGUIUtility.TrTextContent(string.Empty);
		Rect line = position;
		line.height = EditorGUIUtility.singleLineHeight;
		Rect lineToggle = line;
		lineToggle.width = 20;
		line.x += lineToggle.width;
		line.width -= lineToggle.width;
		languageRectTransform.isOverrideAnchoredPosition3D = EditorGUI.ToggleLeft(lineToggle, empty,
			languageRectTransform.isOverrideAnchoredPosition3D);
		using (new EditorGUI.DisabledScope(!languageRectTransform.isOverrideAnchoredPosition3D))
		{
			EditorGUI.PropertyField(line, property.FindPropertyRelative("anchoredPosition3D"), true);
		}

		NextLine(ref line, ref lineToggle);
		languageRectTransform.isOverrideSizeDelta =
			EditorGUI.ToggleLeft(lineToggle, empty, languageRectTransform.isOverrideSizeDelta);
		using (new EditorGUI.DisabledScope(!languageRectTransform.isOverrideSizeDelta))
		{
			EditorGUI.PropertyField(line, property.FindPropertyRelative("sizeDelta"), true);
		}

		NextLine(ref line, ref lineToggle);
		languageRectTransform.isOverrideScale =
			EditorGUI.ToggleLeft(lineToggle, empty, languageRectTransform.isOverrideScale);
		using (new EditorGUI.DisabledScope(!languageRectTransform.isOverrideScale))
		{
			EditorGUI.PropertyField(line, property.FindPropertyRelative("scale"), true);
		}
	}
}
#endif

#endregion

[RequireComponent(typeof(RectTransform))]
public class LanguageRectTransform : MonoBehaviourLanguage
{
	[HideInInspector] public bool isOverrideAnchoredPosition3D = true;

	[FormerlySerializedAs("isOverrideAizeDelta")] [HideInInspector]
	public bool isOverrideSizeDelta;

	[HideInInspector] public bool isOverrideScale;
	public RectTransform rtSelf; //控件

	public RectTransformData[] rectTransformDatas = new RectTransformData[0];

	public override void SwitchLanguage(int nLanguageIndex)
	{
		if (rectTransformDatas.Length == 0 || rectTransformDatas.Length <= nLanguageIndex)
		{
			Debug.Log(this.Debug(" 切换语言错误：" + nLanguageIndex));
			return;
		}

		RectTransformData rectTransformData = rectTransformDatas[nLanguageIndex]; //如果数组元素不对，故意留着报错
		if (isOverrideAnchoredPosition3D) { rtSelf.anchoredPosition3D = rectTransformData.anchoredPosition3D; }

		if (isOverrideSizeDelta) { rtSelf.sizeDelta = rectTransformData.sizeDelta; }

		if (isOverrideScale) { rtSelf.localScale = rectTransformData.scale; }
	}

	[Serializable]
	public struct RectTransformData
	{
		public Vector3 anchoredPosition3D;
		public Vector2 sizeDelta;
		public Vector3 scale;
	}

#if UNITY_EDITOR
	private void Reset() { Setup(); }

	protected override void Setup()
	{
		if (!rtSelf) { rtSelf = GetComponent<RectTransform>(); }

		if (rtSelf)
		{
			rectTransformDatas = new RectTransformData[LanguageManager.nLanguageCount]; //如果数组元素不对，故意留着报错
			for (int i = 0; i < rectTransformDatas.Length; i++)
			{
				RectTransform rectTransform = transform as RectTransform;
				rectTransformDatas[i] = new RectTransformData
				{
					sizeDelta = rectTransform.sizeDelta, anchoredPosition3D = rectTransform.anchoredPosition3D,
					scale = rectTransform.localScale,
				};
			}

			base.Setup();
		}
	}

	private void OnValidate() { SwitchLanguage(LanguageManager.nLanguage); }
#endif
}