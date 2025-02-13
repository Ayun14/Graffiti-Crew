using UnityEngine;

[CreateAssetMenu(fileName = "LanguageSO", menuName = "SO/UI/LanguageSO")]
public class LanguageSO : ScriptableObject
{
    public string title = "언어";
    public string[] languageTypes = { "한글", "영어" };
}
