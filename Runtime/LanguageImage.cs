// /////'''''''''''''''''''''''''''''''''''''''''''''''''\\\\\
//       Copyright (c) 2018 Pixel Cat Games
//       Date: 2020-08-03 17:41
// \\\\\,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,/////

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LanguageImage : MonoBehaviourLanguage
{
    public bool isSetNativeSize = true;
    public Image imageSelf; //控件
    public Sprite[] sprites; //图片

    public override void SwitchLanguage(int nLanguageIndex)
    {
        if (sprites.Length == 0 || sprites.Length <= nLanguageIndex)
        {
            Debug.Log(this.Debug("切换语言错误：" + nLanguageIndex));
            return;
        }

#if UNITY_EDITOR
        if (!imageSelf)
        {
            Debug.LogWarning(this.Debug("缺少图片"));
        }
#endif

        imageSelf.sprite = sprites[nLanguageIndex]; //如果数组元素不对，故意留着报错
        if (isSetNativeSize)
        {
            imageSelf.SetNativeSize();
        }
    }

#if UNITY_EDITOR
    private void Reset() { Setup(); }

    protected override void Setup()
    {
        if (!imageSelf)
        {
            imageSelf = GetComponent<Image>();
        }

        if (imageSelf)
        {
            sprites = new Sprite[2]; //如果数组元素不对，故意留着报错
            Sprite spriteNow = imageSelf.sprite;
            string str = AssetDatabase.GetAssetPath(spriteNow.texture);
            if (spriteNow.texture.name.Contains("eng"))
            {
                sprites[1] = spriteNow;
                str = str.Replace("eng", string.Empty);
                Sprite sprite = AssetDatabase.LoadAssetAtPath(str, typeof(Sprite)) as Sprite;
                sprites[0] = sprite ? sprite : spriteNow;
                imageSelf.SetNativeSize();
                return;
            }

            sprites[0] = spriteNow;
            str = str.Insert(str.Length - 4, "eng");
            Sprite spriteEng = AssetDatabase.LoadAssetAtPath(str, typeof(Sprite)) as Sprite;
            sprites[1] = spriteEng ? spriteEng : spriteNow;
            imageSelf.SetNativeSize();
            base.Setup();
        }
    }
#endif
}