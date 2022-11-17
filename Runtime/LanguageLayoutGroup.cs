using System;
using UnityEngine;
using UnityEngine.UI;

#region UNITY_EDITOR

#if UNITY_EDITOR
using UnityEditor;


[CustomPropertyDrawer(typeof(LanguageLayoutGroup.LayoutGroupData))]
public class LayoutGroupDataDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.FindPropertyRelative("padding").isExpanded)
        {
            return 20 * 6;
        }

        return 20 * 2;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent title)
    {
        static void NextLine(ref Rect rect, ref Rect lineToggle1)
        {
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            lineToggle1.y += EditorGUIUtility.singleLineHeight + 2;
        }

        LanguageLayoutGroup languageLayoutGroup = property.serializedObject.targetObject as LanguageLayoutGroup;
        GUIContent empty = EditorGUIUtility.TrTextContent(string.Empty);
        Rect line = position;
        line.height = EditorGUIUtility.singleLineHeight;
        Rect lineToggle = line;
        lineToggle.width = 28;
        line.x += lineToggle.width;
        line.width -= lineToggle.width;
        SerializedProperty spPadding = property.FindPropertyRelative("padding");
        languageLayoutGroup.isOverridePadding = EditorGUI.ToggleLeft(lineToggle, empty,
            languageLayoutGroup.isOverridePadding);
        using (new EditorGUI.DisabledScope(!languageLayoutGroup.isOverridePadding))
        {
            EditorGUI.PropertyField(line, spPadding, true);
        }

        NextLine(ref line, ref lineToggle);
        if (spPadding.isExpanded)
        {
            NextLine(ref line, ref lineToggle);
            NextLine(ref line, ref lineToggle);
            NextLine(ref line, ref lineToggle);
            NextLine(ref line, ref lineToggle);
        }

        languageLayoutGroup.isOverrideChildAlignment =
            EditorGUI.ToggleLeft(lineToggle, empty, languageLayoutGroup.isOverrideChildAlignment);
        using (new EditorGUI.DisabledScope(!languageLayoutGroup.isOverrideChildAlignment))
        {
            EditorGUI.PropertyField(line, property.FindPropertyRelative("childAlignment"), true);
        }
    }
}
#endif

#endregion

[RequireComponent(typeof(LayoutGroup))]
public class LanguageLayoutGroup : MonoBehaviourLanguage
{
    [HideInInspector] public bool isOverridePadding = true;

    [HideInInspector] public bool isOverrideChildAlignment;
    public LayoutGroup lgSelf; //控件

    public LayoutGroupData[] layoutGroupDatas = new LayoutGroupData[0];

    public override void SwitchLanguage(int nLanguageIndex)
    {
        if (layoutGroupDatas.Length == 0 || layoutGroupDatas.Length <= nLanguageIndex)
        {
            Debug.Log(this.Debug(" 切换语言错误：" + nLanguageIndex));
            return;
        }

        LayoutGroupData layoutGroupData = layoutGroupDatas[nLanguageIndex]; //如果数组元素不对，故意留着报错
        if (isOverridePadding)
        {
            lgSelf.padding.left = layoutGroupData.padding.left;
            lgSelf.padding.right = layoutGroupData.padding.right;
            lgSelf.padding.top = layoutGroupData.padding.top;
            lgSelf.padding.bottom = layoutGroupData.padding.bottom;
#if UNITY_EDITOR
            lgSelf.enabled = false;
            lgSelf.enabled = true;
#endif
        }

        if (isOverrideChildAlignment)
        {
            lgSelf.childAlignment = layoutGroupData.childAlignment;
        }
    }

    [Serializable]
    public struct LayoutGroupData
    {
        public RectOffset padding;

        public TextAnchor childAlignment;
    }

#if UNITY_EDITOR
    private void Reset()
    {
        Setup();
    }

    protected override void Setup()
    {
        if (!lgSelf)
        {
            lgSelf = GetComponent<LayoutGroup>();
        }

        if (lgSelf)
        {
            layoutGroupDatas = new LayoutGroupData[LanguageManager.nLanguageCount]; //如果数组元素不对，故意留着报错
            for (int i = 0; i < layoutGroupDatas.Length; i++)
            {
                RectOffset lgSelfPadding = lgSelf.padding;
                layoutGroupDatas[i] = new LayoutGroupData
                {
                    padding = new RectOffset(lgSelfPadding.left, lgSelfPadding.right, lgSelfPadding.top,
                        lgSelfPadding.bottom),
                    childAlignment = lgSelf.childAlignment,
                };
            }

            base.Setup();
        }
    }

    private void OnValidate()
    {
        SwitchLanguage(LanguageManager.nLanguage);
    }
#endif
}