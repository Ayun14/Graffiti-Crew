using AH.UI.Events;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueController : Observer<GameStateController>
{
    private DialogueUIController _dialogueUIController;

    private void Awake()
    {
        Attach();

        _dialogueUIController = GetComponentInChildren<DialogueUIController>();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if(mySubject.GameState == GameState.Talk)
        {
            List<DialogueData> list = _dialogueUIController.dialogueDataReader.DialogueList;
            _dialogueUIController.StartDialogue(1, list[list.Count-1].id,ChangeGameState);
        }
    }

    private void ChangeGameState()
    {
        mySubject.ChangeGameState(GameState.Graffiti);
    }
}
