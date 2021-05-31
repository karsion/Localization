// /////'''''''''''''''''''''''''''''''''''''''''''''''''\\\\\
//       Copyright (c) 2018 Pixel Cat Games
//       Date: 2020-08-03 17:41
// \\\\\,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,/////

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class LanguageSharedMaterial : MonoBehaviourLanguage
{
    public Renderer rendererSelf; //控件
    //public int materialIndex;
    public Material[] materials; //图片

    public override void SwitchLanguage(int nLanguage)
    {
        if (materials.Length == 0 || materials.Length <= nLanguage)
        {
            Debug.Log(this.Debug(" 切换语言错误：" + nLanguage));
            return;
        }

        rendererSelf.sharedMaterial = materials[nLanguage]; //如果数组元素不对，故意留着报错
    }

#if UNITY_EDITOR
    private void Reset() { Setup(); }

    protected override void Setup()
    {
        if (!rendererSelf)
        {
            rendererSelf = GetComponent<Renderer>();
        }

        if (rendererSelf)
        {
            materials = new Material[LanguageManager.nLanguageCount];
            string str = string.Empty;
            Material matNow = rendererSelf.sharedMaterial;
            if (matNow.name.Contains("eng"))
            {
                materials[1] = matNow;
                str = AssetDatabase.GetAssetPath(matNow);
                str = str.Replace("eng", string.Empty);
                Material mat= AssetDatabase.LoadAssetAtPath(str, typeof(Material)) as Material;
                materials[0] = mat?mat:matNow;
                return;
            }

            materials[0] = matNow;
            str = AssetDatabase.GetAssetPath(matNow);
            str = str.Insert(str.Length - 4, "eng");
            Material matEng = AssetDatabase.LoadAssetAtPath(str, typeof(Material)) as Material;
            materials[1] = matEng?matEng:matNow;
            base.Setup();
        }
    }
#endif
}