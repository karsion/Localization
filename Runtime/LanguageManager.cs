using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public interface ILanguage
{
    void SwitchLanguage(int nLanguageIndex);
#if UNITY_EDITOR
    void Refresh();
#endif
}

[ExecuteInEditMode]
public abstract class MonoBehaviourLanguage : MonoBehaviour, ILanguage
{
    protected virtual void OnEnable()
    {
#if UNITY_EDITOR
        if (isPreview) return;
#endif

        LanguageManager.Add(this);
    }

    protected int nLanguage => LanguageManager.nLanguage;
    public abstract void SwitchLanguage(int nLanguageIndex);

    private void OnDisable()
    {
#if UNITY_EDITOR
        if (isPreview) return;
#endif
        
        LanguageManager.Remove(this);
    }

#if UNITY_EDITOR
    protected virtual void Setup()
    {
        EditorUtility.SetDirty(this);
    }

    [ButtonEx("Eng")]
    protected void Chs() { LanguageManager.SetLanguage(0); }

    protected void Eng() { LanguageManager.SetLanguage(1); }

    private int nPreview;
    private bool isPreview;
    [ButtonEx("Current")]
    protected void Preview()
    {
        nPreview = nPreview == 0 ? 1 : 0;
        SwitchLanguage(nPreview);
        Refresh();
    }

    public virtual void Refresh()
    {
        isPreview = true;
        enabled = false;
        enabled = true;
        isPreview = false;
    }

    protected void Current()
    {
        nPreview = LanguageManager.nLanguage;
        SwitchLanguage(nPreview);
        Refresh();
    }
#endif
}

//语言管理器
public static class LanguageManager
{
    internal static readonly string[] strLanguageKey = {"", "Eng"};
    //多语言切换对象列表
    private static readonly List<ILanguage> listILanguages = new List<ILanguage>(50);
    internal static readonly int nLanguageCount = strLanguageKey.Length;
    public static int nLanguage;

    public static void Add(ILanguage language)
    {
        listILanguages.Add(language);
        language.SwitchLanguage(nLanguage);
    }

    public static void SetLanguage(int nNewLanguage)
    {
        if (nLanguage != nNewLanguage)
        {
            nLanguage = nNewLanguage;
            for (int i = 0; i < listILanguages.Count; i++)
            {
                listILanguages[i].SwitchLanguage(nLanguage);
#if UNITY_EDITOR
                listILanguages[i].Refresh();
#endif
            }
        }
    }

    public static void Remove(ILanguage language) { listILanguages.Remove(language); }

    public static bool TryGetAudioClip(string key, int nLanguageIndex, out AudioClip audioClip)
    {
	    if (!string.IsNullOrEmpty(key) && LanguageAudioClipData.datas.ContainsKey(key))
	    {
		    audioClip = LanguageAudioClipData.datas[key][nLanguageIndex];
		    return true;
	    }

	    #region UNITY_EDITOR
#if UNITY_EDITOR
	    Debug.LogWarning($"Key有问题：[{key}]");
#endif
	    #endregion

	    audioClip = null;
	    return false;
    }

    public static bool TryGetAudioClip(string key, out AudioClip audioClip)
    {
	    return TryGetAudioClip(key, nLanguage, out audioClip);
    }


    public static string GetText(string key, int nLanguageIndex)
    {
        if (!string.IsNullOrEmpty(key) && LanguageStringData.datas.ContainsKey(key))
        {
            return LanguageStringData.datas[key][nLanguageIndex];
        }

        #region UNITY_EDITOR
#if UNITY_EDITOR
        Debug.LogWarning($"Key有问题：[{key}]");
#endif
        #endregion
        return string.Empty;
    }

    public static string GetText(string key)
    {
        return GetText(key, nLanguage);
    }

    public static string GetString(string str)
    {
        string[] strs = str.Split('\n');
        return nLanguage >= strs.Length ? string.Empty : strs[nLanguage];
    }

    public static T GetElementByLanguage<T>(this T[] contents)
    {
        return nLanguage >= contents.Length ? default : contents[nLanguage];
    }
}