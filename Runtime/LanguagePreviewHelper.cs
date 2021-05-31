public class LanguagePreviewHelper : MonoBehaviourLanguage
{
    private MonoBehaviourLanguage[] languages;

    public override void SwitchLanguage(int nLanguage)
    {
        languages = GetComponentsInChildren<MonoBehaviourLanguage>();
        for (int i = 1; i < languages.Length; i++)
        {
            MonoBehaviourLanguage monoBehaviourLanguage = languages[i];
            monoBehaviourLanguage.SwitchLanguage(nLanguage);
        }
    }
}
