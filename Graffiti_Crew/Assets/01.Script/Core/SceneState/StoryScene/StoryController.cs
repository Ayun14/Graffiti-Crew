using AH.UI.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryController : Observer<GameStateController>, INeedLoding
{
    [SerializeField] private DialogueController _dialogueController;
    [SerializeField] private DialogueUIController _dialogueUIController;

    [SerializeField] private SplashController _splashController;

    private List<GameObject> _levelPrefabs = new List<GameObject>();

    private StoryDialogueSO _storyDialogueSO;
    private int _dialogueNum = 0;

    private Image _loadingPanel;


    private void Awake()
    {
        Attach();

        Transform canvas = transform.Find("Canvas");
        _loadingPanel = canvas.Find("Panel_Loading").GetComponent<Image>();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            _loadingPanel.gameObject.SetActive(mySubject.GameState == GameState.Loding);

            if (mySubject.GameState == GameState.Dialogue)
            {
                _dialogueUIController.ChangeDialogueUI?.Invoke(true);
                StageEvent.SetActiveFightViewEvent?.Invoke(false);

                NPCSO dialogue = _storyDialogueSO.storyList[_dialogueNum];
                _dialogueUIController.ChangeDialogueUI?.Invoke(true);
                _dialogueController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
            }
        }
    }

    private void DialogueEnd()
    {
        StartCoroutine(HandleDialogueTransition());
    }

    private IEnumerator HandleDialogueTransition()
    {
        yield return StartCoroutine(Fade(false));

        DialogueEvent.ShowDialougeViewEvent?.Invoke(false);

        _levelPrefabs[_dialogueNum].SetActive(false);

        _dialogueNum++;

        if (_dialogueNum <= _levelPrefabs.Count - 1)
            _levelPrefabs[_dialogueNum].SetActive(true);

        if (_dialogueNum == _storyDialogueSO.storyList.Count)
        {
            yield return StartCoroutine(Fade(false));

            GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Fight_After);
            mySubject.ChangeGameState(GameState.NextStage);
            yield break;
        }

        NPCSO dialogue = _storyDialogueSO.storyList[_dialogueNum];

        yield return StartCoroutine(Fade(true));

        _dialogueController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
    }


    private IEnumerator Fade(bool isFadeIn)
    {
        _splashController.isFinished = false;

        if (isFadeIn)
            yield return _splashController.FadeIn(false, true);
        else
            yield return _splashController.FadeOut(false, true);
    }


    public void LodingHandle(DataController dataController)
    {
        //dataController.stageData.stageSaveData.stageState = StageState.Clear;

        GameObject map = Instantiate(dataController.stageData.mapPrefab, Vector3.zero, Quaternion.identity);

        Transform parent = map.transform;
        foreach (Transform child in parent)
        {
            _levelPrefabs.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
        _levelPrefabs[_dialogueNum].SetActive(true);

        _storyDialogueSO = dataController.stageData.storyDialogue;

        _dialogueController.dialogueDataReader = dataController.stageData.dialogueData_KR;
        //_dialogueUIController.dialogueDataReader_KR = dataController.stageData.dialogueData_KR;
        //_dialogueUIController.dialogueDataReader_EN = dataController.stageData.dialogueData_EN;

        dataController.SuccessGiveData();
    }
}
