using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "SO/UI/Dialouge/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public string characterName;
    public string dialogue;
    public Sprite profil;

    public void ResetData() {
        this.characterName = "";
        this.dialogue = "";
        this.profil = null;
    }
}
