using AH.UI.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "SO/UI/Dialouge/DialogueSO")]
public class DialogueSO : ScriptableObject {
    public string characterName;
    public string dialogue;
    public Sprite profile;
    public Sprite background; // юс╫ц

    public void SetProfile(Sprite profile) {
        this.profile = profile;
        //sDialougeEvent.ChangeOpponentEvent?.Invoke(profile);
    }
    public void ResetData() {
        this.characterName = "";
        this.dialogue = "";
        this.profile = null;
        this.background = null;
    }
}
