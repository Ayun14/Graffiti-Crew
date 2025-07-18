using AH.UI.Events;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class TutorialController : Observer<GameStateController>, INeedLoding
{
    [Header("Levels")]
    [SerializeField] private GameObject _level;
    [SerializeField] private GameObject _hangout;
    [SerializeField] private GameObject _mainCam;

    [Header("Dialogue")]
    [SerializeField] private DialogueController _dialogueController;
    [SerializeField] private DialogueUIController _dialogueUIController;
    [SerializeField] private StoryDialogueSO _tutorialDialogueSO;
    [SerializeField] private SplashController _splashController;

    [Header("UI")]
    [SerializeField] private GameObject _explainImg;

    private int _dialogueNum = 0;
    private int _clearNode = 0;

    private int _tutorialStartIndex = 81;


    private void Awake()
    {
        Attach();
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Dialogue)
            {
                if (_dialogueNum == 0)
                    GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Rain);

                _dialogueUIController.ChangeDialogueUI?.Invoke(true);
                StageEvent.SetActiveFightViewEvent?.Invoke(false);

                NPCSO dialogue = _tutorialDialogueSO.storyList[_dialogueNum];
                _dialogueController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);

                // Cursor
                GameManager.Instance.SetCursor(CursorType.Normal);
            }

            if (mySubject.GameState == GameState.Countdown)
            {
                _dialogueUIController.ChangeDialogueUI?.Invoke(false);
                DialogueEvent.ShowDialougeViewEvent?.Invoke(false);

                DialogueEvent.ShowMiniDialougeViewEvent?.Invoke(true);
                StageEvent.SetActiveFightViewEvent?.Invoke(true);

                _explainImg.SetActive(true);

                _dialogueController.StartDialogue(_tutorialStartIndex + 9, _tutorialStartIndex + 12, ExplainEnd);
            }

            if (mySubject.GameState == GameState.Tutorial)
            {
                DialogueEvent.ShowMiniDialougeViewEvent?.Invoke(true);

                _dialogueController.CanSkip = false;
                _dialogueController.StartDialogue(_tutorialStartIndex, _tutorialStartIndex + 8, null);

                // Cursor
                GameManager.Instance.SetCursor(CursorType.Spray);
            }
        }
    }

    private void ExplainEnd()
    {
        StartCoroutine(ExplainEndRoutine());
    }

    private IEnumerator ExplainEndRoutine()
    {
        StageEvent.SetActiveFightViewEvent?.Invoke(false);
        DialogueEvent.ShowMiniDialougeViewEvent?.Invoke(false);

        yield return StartCoroutine(Fade(false));
        _explainImg.SetActive(false);
        yield return StartCoroutine(Fade(true));

        mySubject.ChangeGameState(GameState.Tutorial);
    }

    private void DialogueEnd()
    {
        StartCoroutine(HandleDialogueTransition());
    }

    private IEnumerator HandleDialogueTransition()
    {

        if (_dialogueNum == _tutorialDialogueSO.storyList.Count)
            _hangout.SetActive(false);

        yield return StartCoroutine(Fade(false));

        _dialogueNum++;

        if (_dialogueNum == 1)
        {
            NPCSO dialogue = _tutorialDialogueSO.storyList[_dialogueNum];

            yield return StartCoroutine(Fade(true));

            GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Rain);
            GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Tutorial);

            _dialogueController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
        }
        else if (_dialogueNum == 2)
        {
            yield return StartCoroutine(Fade(false));
            _level.SetActive(true);
            yield return StartCoroutine(Fade(true));

            mySubject.ChangeGameState(GameState.Countdown);
        }
        else if (_dialogueNum == 3)
        {
            _dialogueController.CanSkip = true;

            _mainCam.SetActive(false);
            NPCSO dialogue = _tutorialDialogueSO.storyList[_dialogueNum];

            _level.SetActive(false);
            _hangout.SetActive(true);

            yield return StartCoroutine(Fade(true));

            _dialogueController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
        }
        else if (_dialogueNum == _tutorialDialogueSO.storyList.Count)
        {
            _splashController.SetFade();
            GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Tutorial);

            SaveDataEvents.SaveGameEvent?.Invoke("HangOutScene");
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        _splashController.isFinished = false;

        if (isFadeIn)
            yield return _splashController.FadeIn(false, true);
        else
            yield return _splashController.FadeOut(false, true);
    }

    public async void CheckClearNode()
    {
        _clearNode++;

        if (_clearNode == 9)
        {
            await Task.Delay(1100);
            DialogueEvent.ShowMiniDialougeViewEvent?.Invoke(false);
            PresentationEvents.FadeInOutEvent?.Invoke(true);
        }

        _dialogueController.DialogueSkip();
        _dialogueUIController.OnEndTyping(false);
    }

    public void LodingHandle(DataController dataController)
    {
        //dataController.stageData.stageSaveData.stageState = StageState.Clear;

        _dialogueController.dialogueDataReader = dataController.stageData.dialogueData_KR;
        //_dialogueUIController.dialogueDataReader_KR = dataController.stageData.dialogueData_KR;
        //_dialogueUIController.dialogueDataReader_EN = dataController.stageData.dialogueData_EN;

        dataController.SuccessGiveData();
    }
}
