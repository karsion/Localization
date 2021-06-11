using UnityEngine;
public class LanguageGameObject : MonoBehaviourLanguage
{
    public GameObject[] gos;
    public override void SwitchLanguage(int nLanguageIndex)
    {
        if (gos.Length == 0 || gos.Length <= nLanguageIndex)
        {
            Debug.Log(this.Debug(" 切换语言错误：" + nLanguageIndex));
            return;
        }

        gos.ForEach(go => go.SetActive(false));
        gos[nLanguageIndex].SetActive(true);//如果数组元素不对，故意留着报错
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
