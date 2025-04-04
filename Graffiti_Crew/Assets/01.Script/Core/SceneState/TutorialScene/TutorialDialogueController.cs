using AH.UI.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialDialogueController : Observer<GameStateController>
{
    [SerializeField] private DialogueUIController _dialogueUIController;

    [SerializeField] private List<NPCSO> _dialogueList;
    private int _dialogueNum = 0;


    private void Awake()
    {
        Attach();
    }

    private void OnDestroy()
    {
        Detach();
    }
    public override void NotifyHandle()
    {
        if (mySubject.GameState == GameState.Dialogue)
        {
            _dialogueUIController.ChangeDialogueUI?.Invoke(true);
            StageEvent.SetActiveFightViewEvent?.Invoke(false);

            DialogueEvent.SetCharacterEvent?.Invoke(DialougeCharacter.Jia);
            DialogueEvent.ShowDialougeViewEvent?.Invoke(true);

            NPCSO dialogue = _dialogueList[_dialogueNum];
            _dialogueUIController.ChangeDialogueUI?.Invoke(true);
            _dialogueUIController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
        }
    }

    private async void DialogueEnd()
    {
        DialogueEvent.ShowDialougeViewEvent?.Invoke(false);
        _dialogueNum++;
        if (_dialogueNum == 1)
        {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(2100);
            _dialogueUIController._dialogueBG.SetActive(false);
            PresentationEvents.FadeInOutEvent?.Invoke(true);

            mySubject.ChangeGameState(GameState.Tutorial);
        }
        else if (_dialogueNum == 2)
        {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(1100);
            SaveDataEvents.SaveGameEvent?.Invoke("HangOutScene");
        }

    }
}
