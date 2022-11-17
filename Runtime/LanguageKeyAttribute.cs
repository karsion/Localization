using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

[AttributeUsage(AttributeTargets.Field)]
public class LanguageKeyAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(LanguageKeyAttribute), true)]
internal class LanguageKeyAttributeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float h = EditorGUI.GetPropertyHeight(property, label, property.hasVisibleChildren);
        if (property.propertyType == SerializedPropertyType.String)
        {
            h += EditorGUIUtility.singleLineHeight + 6;
        }

        return h;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        bool isArray = property.propertyPath.Contains(".Array.data[");
        if (property.propertyType == SerializedPropertyType.String)
        {
            #region DrawBG

            int positionDepth = 1;
            if (isArray)
            {
                string str = property.propertyPath.Remove(0, property.propertyPath.LastIndexOf(']'));
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == '.')
                    {
                        positionDepth++;
                    }
                }
            }
            else
            {
                positionDepth = property.depth;
            }

            int depthOffset = positionDepth * 15 - (isArray ? 16 : 0);
            Rect bg = position;
            bg.x += depthOffset;
            bg.width -= depthOffset;
            EditorGUI.DrawRect(bg, Color.black);

            #endregion

            Rect button = position;
            button.y += 2;
            button.height = EditorGUIUtility.singleLineHeight;
            button.x += 2 + depthOffset;
            button.width = (bg.width - 6) * 0.5f;
            label.text = $"{label.text} (LocalizedKey)";
            if (GUI.Button(button, "PingText"))
            {
                LanguageStringData.PingData(property.stringValue);
            }

            button.x += button.width + 2;
            if (GUI.Button(button, "PingAudioClip"))
            {
                LanguageAudioClipData.PingData(property.stringValue);
            }

            position.y += EditorGUIUtility.singleLineHeight + 4;
            position.height = EditorGUIUtility.singleLineHeight;
            position.width -= 2;
        }

        position.x += 2;
        position.width -= 2;
        EditorGUI.PropertyField(position, property, label, property.hasVisibleChildren);
    }
}
#endif