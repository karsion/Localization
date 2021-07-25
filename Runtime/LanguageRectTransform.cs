using System;
using UnityEngine;

#region UNITY_EDITOR

#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(LanguageRectTransform.RectTransformData))]
public class RectTransformDataDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent title)
	{
		static void NextLine(ref Rect rect, ref Rect lineToggle1)
		{
			rect.y += EditorGUIUtility.singleLineHeight + 2;
			lineToggle1.y += EditorGUIUtility.singleLineHeight + 2;
		}

        LanguageRectTransform ltmp = property.serializedObject.targetObject as LanguageRectTransform;
		GUIContent empty = EditorGUIUtility.TrTextContent(string.Empty);
		Rect line = position;
		line.height = EditorGUIUtility.singleLineHeight;
		Rect lineToggle = line;
		lineToggle.width = 20;
		line.x += lineToggle.width;
		line.width -= lineToggle.width;
		ltmp.isOverrideAnchoredPosition3D = EditorGUI.ToggleLeft(lineToggle, empty, ltmp.isOverrideAnchoredPosition3D);
		using (new EditorGUI.DisabledScope(!ltmp.isOverrideAnchoredPosition3D))
		{
			EditorGUI.PropertyField(line, property.FindPropertyRelative("anchoredPosition3D"), true);
		}

		NextLine(ref line, ref lineToggle);
		ltmp.isOverrideAizeDelta = EditorGUI.ToggleLeft(lineToggle, empty, ltmp.isOverrideAizeDelta);
		using (new EditorGUI.DisabledScope(!ltmp.isOverrideAizeDelta))
		{
			EditorGUI.PropertyField(line, property.FindPropertyRelative("sizeDelta"), true);
		}
	}
}
#endif

#endregion

[RequireComponent(typeof(LanguageRectTransform))]
public class LanguageRectTransform : MonoBehaviourLanguage
{
    [HideInInspector]
    public bool isOverrideAnchoredPosition3D = true;
    [HideInInspector]
    public bool isOverrideAizeDelta;
    public RectTransform rtSelf; //控件

    [Serializable]
    public struct RectTransformData
    {
        public Vector3 anchoredPosition3D;
        public Vector2 sizeDelta;
    }

    public RectTransformData[] rectTransformDatas = new RectTransformData[0];

    public override void SwitchLanguage(int nLanguageIndex)
    {
        if (rectTransformDatas.Length == 0 || rectTransformDatas.Length <= nLanguageIndex)
        {
            Debug.Log(this.Debug(" 切换语言错误：" + nLanguageIndex));
            return;
        }

        RectTransformData rectTransformData = rectTransformDatas[nLanguageIndex];
        if (isOverrideAnchoredPosition3D)
        {
            rtSelf.anchoredPosition3D = rectTransformData.anchoredPosition3D; //如果数组元素不对，故意留着报错
        }

        if (isOverrideAizeDelta)
        {
            rtSelf.sizeDelta = rectTransformData.sizeDelta; //如果数组元素不对，故意留着报错
        }
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
                    sizeDelta = rectTransform.sizeDelta, anchoredPosition3D = rectTransform.anchoredPosition3D
                };
            }

            base.Setup();
        }
    }
#endif
}