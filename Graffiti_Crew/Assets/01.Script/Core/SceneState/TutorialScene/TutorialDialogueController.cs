using AH.UI.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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
            // FadeOut
            _dialogueUIController.ChangeDialogueUI?.Invoke(true);
            StageEvent.SetActiveFightViewEvent?.Invoke(false);
            DialougeEvent.ShowDialougeViewEvent?.Invoke(true);

            NPCSO dialogue = _dialogueList[_dialogueNum];
            _dialogueUIController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
        }
    }

    private void DialogueEnd()
    {
        // FadeIn
        DialougeEvent.ShowDialougeViewEvent?.Invoke(false);
        _dialogueNum++;
        if (_dialogueNum == 1)
        {
            mySubject.ChangeGameState(GameState.CountDown);
        }
        else if (_dialogueNum == 2)
        {
            NPCSO dialogue = _dialogueList[_dialogueNum];
            _dialogueUIController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
        }
        else
            SceneManager.LoadScene("HangOutScene");

    }
}
