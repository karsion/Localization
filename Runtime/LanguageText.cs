using UnityEngine;
using UnityEngine.UI;

public class LanguageText : MonoBehaviourLanguage
{
    [Multiline]
    public string[] strText;
    //[SerializeField]
    public Text textSelf;
    public override void SwitchLanguage(int nLanguageIndex)
    {
        if (nLanguageIndex >= strText.Length)
        {
            Debug.LogError(" SwitchLanguage Error, GameObject：" + transform.GetPath());
            return;
        }

        textSelf.text = strText[nLanguageIndex];
    }

    //自动建立数组，并引用Text中的text
    private void Reset()
    {
        strText = new string[LanguageManager.nLanguageCount];
        textSelf = GetComponent<Text>();
        if (!textSelf)
        {
            return;
        }

        strText[0] = textSelf.text;
    }

#if UNITY_EDITOR
    protected override void Setup()
    {
        Reset();
        base.Setup();
    }
#endif
}