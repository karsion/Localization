// Copyright: Pixel Cat Games
// Date: 2020-01-19
// Time: 14:09
// Author: Karsion

using System;
using UnityEngine;

public class LanguageSprite : MonoBehaviourLanguage
{
    public SpriteRenderer spriteRenderer; //控件
    public Sprite[] sprites; //图片

    public override void SwitchLanguage(int nLanguageIndex)
    {
        if (sprites.Length == 0 || sprites.Length <= nLanguageIndex)
        {
            Debug.Log(this.Debug(" 切换语言错误：" + nLanguageIndex));
            return;
        }

        spriteRenderer.sprite = sprites[nLanguageIndex]; //如果数组元素不对，故意留着报错
    }

#if UNITY_EDITOR
    private void Reset()
    {
        Setup();
    }

    protected override void Setup()
    {
        if (!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer)
        {
            sprites = new Sprite[2]; //如果数组元素不对，故意留着报错
            string str = String.Empty;
            if (spriteRenderer.sprite.texture.name.Contains("eng"))
            {
                sprites[1] = spriteRenderer.sprite;
                str = UnityEditor.AssetDatabase.GetAssetPath(spriteRenderer.sprite.texture);
                str = str.Replace("eng", string.Empty);
                Sprite texture = UnityEditor.AssetDatabase.LoadAssetAtPath(str, typeof(Sprite)) as Sprite;
                sprites[0] = texture;
                //spriteRenderer.SetNativeSize();

                if (!sprites[0])
                {
                    sprites[0] = spriteRenderer.sprite;
                }

                return;
            }

            sprites[0] = spriteRenderer.sprite;
            str = UnityEditor.AssetDatabase.GetAssetPath(spriteRenderer.sprite.texture);
            str = str.Insert(str.Length - 4, "eng");
            Sprite textureEng = UnityEditor.AssetDatabase.LoadAssetAtPath(str, typeof(Sprite)) as Sprite;
            sprites[1] = textureEng;
            //spriteRenderer.SetNativeSize();

            if (!sprites[1])
            {
                sprites[1] = spriteRenderer.sprite;
            }

            base.Setup();
        }
    }
#endif
}