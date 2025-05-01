using AH.UI.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialDialogueController : Observer<GameStateController>
{
    [SerializeField] private DialogueController _dialogueUIController;
    [SerializeField] private DialogueUIController _dialogueController;

    [SerializeField] private StoryDialogueSO _dialogueList;
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
            _dialogueController.ChangeDialogueUI?.Invoke(true);
            StageEvent.SetActiveFightViewEvent?.Invoke(false);

            NPCSO dialogue = _dialogueList.storyList[_dialogueNum];
            _dialogueController.ChangeDialogueUI?.Invoke(true);
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
