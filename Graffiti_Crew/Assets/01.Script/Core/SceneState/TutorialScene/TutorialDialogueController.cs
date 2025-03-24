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
    public async override void NotifyHandle()
    {
        if (mySubject.GameState == GameState.Dialogue)
        {
            if(_dialogueNum == 0)
            {
                PresentationEvents.SetFadeEvent(true);
                await Task.Delay(1100);
                PresentationEvents.FadeInOutEvent(true);
            }

            _dialogueUIController.ChangeDialogueUI?.Invoke(true);
            StageEvent.SetActiveFightViewEvent?.Invoke(false);
            DialougeEvent.ShowDialougeViewEvent?.Invoke(true);

            NPCSO dialogue = _dialogueList[_dialogueNum];
            _dialogueUIController.ChangeDialogueUI?.Invoke(true);
            _dialogueUIController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
        }
    }

    private async void DialogueEnd()
    {
        DialougeEvent.ShowDialougeViewEvent?.Invoke(false);
        _dialogueNum++;
        if (_dialogueNum == 1)
        {
            mySubject.ChangeGameState(GameState.Tutorial); // CountDown¿Ãø¥¿Ω
        }
        else if (_dialogueNum == 2)
        {
            NPCSO dialogue = _dialogueList[_dialogueNum];

            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(1100);
            PresentationEvents.FadeInOutEvent?.Invoke(true);
            await Task.Delay(1100);

            _dialogueUIController.ChangeDialogueUI?.Invoke(true);
            _dialogueUIController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
        }
        else
        {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(1100);
            GameEvents.SaveGameEvent?.Invoke();
            SceneManager.LoadScene("HangOutScene");
        }

    }
}
