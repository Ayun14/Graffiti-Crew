using AH.UI.Events;
using System.Collections.Generic;
using UnityEngine;

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
            DialougeEvent.ShowDialougeViewEvent?.Invoke(true);
            NPCSO dialogue = _dialogueList[_dialogueNum];
            _dialogueNum++;
            _dialogueUIController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
        }
    }

    private void DialogueEnd()
    {
        // FadeIn
        DialougeEvent.ShowDialougeViewEvent?.Invoke(false);
    }
}
