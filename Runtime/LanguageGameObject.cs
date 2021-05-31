using UnityEngine;
public class LanguageGameObject : MonoBehaviourLanguage
{
    public GameObject[] gos;
    public override void SwitchLanguage(int nLanguage)
    {
        if (gos.Length == 0 || gos.Length <= nLanguage)
        {
            Debug.Log(this.Debug(" 切换语言错误：" + nLanguage));
            return;
        }

        gos.ForEach(go => go.SetActive(false));
        gos[nLanguage].SetActive(true);//如果数组元素不对，故意留着报错
    }

    //拖放脚本的时候，自动引用子物体
    private void Reset()
    {
        gos = new GameObject[LanguageManager.nLanguageCount];
        for (int i = 0; i < LanguageManager.nLanguageCount; i++)
        {
            Transform t = transform.GetChild(i);
            if (t) { gos[i] = t.gameObject; }
        }
    }

#if UNITY_EDITOR
    protected override void Setup()
    {
        Reset();
        base.Setup();
    }
#endif
}
