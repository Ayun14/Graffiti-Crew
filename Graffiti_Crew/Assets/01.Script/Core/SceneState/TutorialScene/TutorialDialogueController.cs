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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {

            PresentationEvents.SetFadeEvent(true);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {

            PresentationEvents.FadeInOutEvent(true);
        }         
    }
    public async override void NotifyHandle()
    {
        if (mySubject.GameState == GameState.Dialogue)
        {
            if(_dialogueNum == 0)
            {
                PresentationEvents.SetFadeEvent(true);
                await Task.Delay(100);
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
            mySubject.ChangeGameState(GameState.Timeline); // CountDown¿Ãø¥¿Ω
        }
        else if (_dialogueNum == 2)
        {
            NPCSO dialogue = _dialogueList[_dialogueNum];

            await Task.Delay(500);
            PresentationEvents.FadeInOutEvent(false);
            await Task.Delay(500);
            PresentationEvents.FadeInOutEvent(true);

            _dialogueUIController.ChangeDialogueUI?.Invoke(true);
            _dialogueUIController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
        }
        else
        {
            PresentationEvents.FadeInOutEvent(false);
            SceneManager.LoadScene("HangOutScene");
        }

    }
}
