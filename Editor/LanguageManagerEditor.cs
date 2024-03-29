﻿using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public class LanguageManagerLeftButton
{
	static LanguageManagerLeftButton()
	{
		ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
	}

	static void OnToolbarGUI()
	{
		GUILayout.FlexibleSpace();
		GUILayout.Label("Localization: ");

		if (GUILayout.Button(EditorGUIUtility.TrTempContent("CHS"), GUILayout.Width(64)))
		{
			LanguageManager.SetLanguage(0);
		}

		if (GUILayout.Button(EditorGUIUtility.TrTempContent("EN"), GUILayout.Width(64)))
		{
			LanguageManager.SetLanguage(1);
		}

        GUILayout.Space(128);
    }
}

public class LanguageManagerEditor
{
    private const string strMenuItemEng = "Tools/LanguageManger/Eng";
    private const string strMenuItemChs = "Tools/LanguageManger/Chs";

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        IniEx ini = IniExHelper.GetSettingIni("Gaming");
        LanguageManager.nLanguage = ini.ReadInt("Gaming", "语言", 0);
    }

    [MenuItem(strMenuItemEng)]
    private static void Eng()
    {
        IniEx ini = IniExHelper.GetSettingIni("Gaming");
        ini.WriteInt("Gaming", "语言", 1);
        LanguageManager.nLanguage = 1;
        //ini.SaveFile();
    }

    [MenuItem(strMenuItemEng, true)]
    private static bool Eng验证()
    {
        IniEx ini = IniExHelper.GetSettingIni("Gaming");
        return ini.ReadInt("Gaming", "语言", 1) == 0;
    }

    [MenuItem(strMenuItemChs)]
    private static void Chs()
    {
        IniEx ini = IniExHelper.GetSettingIni("Gaming");
        ini.WriteInt("Gaming", "语言", 0);
        LanguageManager.nLanguage = 0;
        //ini.SaveFile();
    }

    [MenuItem(strMenuItemChs, true)]
    private static bool Chs验证()
    {
        IniEx ini = IniExHelper.GetSettingIni("Gaming");
        return ini.ReadInt("Gaming", "语言", 0) == 1;
    }
}