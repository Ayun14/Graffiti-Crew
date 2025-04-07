using AH.UI.Events;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueController : Observer<GameStateController>
{
    [SerializeField] private DialogueUIController _dialogueUIController;

    [SerializeField] private NPCSO _startDialogue;
    [SerializeField] private NPCSO _endDialogue;
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
        if(mySubject.GameState == GameState.Talk)
        {
            AnimationEvent.SetAnimation?.Invoke(10, AnimationEnum.Talk);
            AnimationEvent.SetAnimation?.Invoke(1, AnimationEnum.Talk);

            _dialogueUIController.ChangeDialogueUI?.Invoke(true);
            if (_dialogueNum == 0)
                _dialogueUIController.StartDialogue(_startDialogue.startIndex, _startDialogue.endIndex, DialogueEnd);
            else
                EndDialogue();
        }
    }

    private async void EndDialogue()
    {
        await Task.Delay(1000);
        _dialogueUIController.StartDialogue(_endDialogue.startIndex, _endDialogue.endIndex, DialogueEnd);
    }

    private async void DialogueEnd()
    {
        _dialogueNum++;
        if (_dialogueNum == 1)
        {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(2100);
            PresentationEvents.FadeInOutEvent?.Invoke(true);

            mySubject.ChangeGameState(GameState.Graffiti);
        }
        else if (_dialogueNum == 2)
        {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(1100);
            SaveDataEvents.SaveGameEvent?.Invoke("ComputerScene");
        }

    }
}
