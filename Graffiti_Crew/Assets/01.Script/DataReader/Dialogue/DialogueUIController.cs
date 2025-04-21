using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueUIController : MonoBehaviour
{
    [Header("Tutorial Data(Only HangOut)")]
    [SerializeField] private BoolSaveDataSO _tutorialCheckData;
    [SerializeField] private GameObject _computerLight;
    [SerializeField] private Collider _computerCollider;

    [Header("Dialogue UI Data")]
    [SerializeField] private DialogueSO _bigDialogueUIData;
    [SerializeField] private DialogueSO _miniDialogueUIData;
    public GameObject _dialogueBG;
    private DialogueSO _dialogueUIData;

    [Header("Dialogue Data")]
    [SerializeField] private LanguageSO _languageSO;
    [SerializeField] private DialogueDataReader _tutorialDialogue;
    public DialogueDataReader dialogueDataReader_KR;
    public DialogueDataReader dialogueDataReader_EN;

    [HideInInspector] public DialogueDataReader dialogueDataReader;

    [Header("Typing Effect")]
    [SerializeField] private float _typingSpeed = 0.05f;

    [Header("Dialogue Camera")]
    [SerializeField] private GameObject _defaultCam;

    private SplashController _splashController;
    private CutSceneController _cutSceneController;

    private bool _isBigUIdata => _dialogueUIData == _bigDialogueUIData;
    private bool _isHangoutScene =>
        SceneManager.GetSceneByName("HangOutScene") == SceneManager.GetActiveScene();

    private Coroutine _typingCoroutine;
    private bool _isTyping = false;
    private bool _isDialogue = false;
    public bool IsDialogue => _isDialogue;

    private int _currentDialogueIndex = 0;
    private int _dialogueStartID = 0;
    private int _dialogueEndID = 0;

    private Action _onDialogueComplete;
    public Action<bool> ChangeDialogueUI;

    private List<DialogueData> _filteredDialogueList;

    private void Awake()
    {
        _splashController = GetComponent<SplashController>();
        _cutSceneController = GetComponent<CutSceneController>();

        dialogueDataReader = dialogueDataReader_KR;
    }

    private void Start()
    {
        //LanguageSystem.LanguageChangedEvent += HandleChangeLangauge;
        SaveDataEvents.LoadEndEvent += HandleTutorialDialogue;
        ChangeDialogueUI += HandleDialogueUIData;

        if(_dialogueBG!=null)
            _dialogueBG.SetActive(false);
        _dialogueUIData = _bigDialogueUIData;
        _dialogueUIData.ResetData();

        //SetLanguageType();
    }

    private void OnDisable()
    {
        //LanguageSystem.LanguageChangedEvent -= HandleChangeLangauge;
        SaveDataEvents.LoadEndEvent -= HandleTutorialDialogue;
        ChangeDialogueUI -= HandleDialogueUIData;
    }

    private void Update()
    {
        if (!_isBigUIdata && !_isHangoutScene)
            return;

        if (_isDialogue)
        {
            if (dialogueDataReader.readMode == ReadMode.Auto)
            {
                if (!_isTyping)
                    ShowNextDialogue();
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (_isTyping)
                    {
                        GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Text_Typing);
                        CompleteTyping();
                    }
                    else
                    {
                        AnimationEvent.EndDialogueAnimation?.Invoke();
                        ShowNextDialogue();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Text_Typing);
                _currentDialogueIndex = _filteredDialogueList.Count;
                ShowNextDialogue();
            }
        }
    }

    #region Handle Event
    private void HandleTutorialDialogue()
    {
        Debug.Log("Start tutorial");
        if (_tutorialCheckData != null && !_tutorialCheckData.data && _isHangoutScene)
        {
            ChangeDialogueUI?.Invoke(false);
            _computerLight.SetActive(false);
            _computerCollider.enabled = false;
            StartDialogue(1, 1);
        }
    }

    private void HandleDialogueUIData(bool isBig)
    {
        if (isBig)
        {
            //SetLanguageType();
            _dialogueUIData = _bigDialogueUIData;
        }
        else
        {
            dialogueDataReader = _tutorialDialogue;
            _dialogueUIData = _miniDialogueUIData;
        }
    }

    //private void HandleChangeLangauge(LanguageType type)
    //{
    //    if (type == LanguageType.English)
    //    {
    //        _languageSO.title = "Language";
    //        _languageSO.languageTypes[0] = "Korea";
    //        _languageSO.languageTypes[1] = "English";
    //        dialogueDataReader = dialogueDataReader_EN;
    //    }
    //    else
    //    {
    //        _languageSO.title = "언어";
    //        _languageSO.languageTypes[0] = "한글";
    //        _languageSO.languageTypes[1] = "영어";
    //        dialogueDataReader = dialogueDataReader_KR;
    //    }
    //}

    #endregion

    //private void SetLanguageType()
    //{
    //    if (LanguageSystem.GetLanguageType() == LanguageType.English)
    //    {
    //        dialogueDataReader = dialogueDataReader_EN;
    //    }
    //    else
    //    {
    //        dialogueDataReader = dialogueDataReader_KR;
    //    }
    //}

    public void StartDialogue(int startID, int endID, Action onComplete = null)
    {
        _isDialogue = true;

        if (dialogueDataReader == null || dialogueDataReader.DialogueList.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        _dialogueStartID = startID;
        _dialogueEndID = endID;

        _filteredDialogueList = dialogueDataReader.DialogueList.FindAll(dialogue =>
            dialogue.id >= _dialogueStartID && dialogue.id <= _dialogueEndID);

        if (_filteredDialogueList.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        _currentDialogueIndex = 0;
        _onDialogueComplete = onComplete;

        StartCoroutine(DialogueRoutine());
    }

    private IEnumerator DialogueRoutine()
    {

        if (_isBigUIdata)
        {
            if (!_isHangoutScene && _dialogueBG !=null)
                _dialogueBG.SetActive(true);

            if (_defaultCam != null)
                _defaultCam.SetActive(false);

            yield return new WaitForSeconds(1.5f);

            DialogueEvent.ShowMiniDialougeViewEvent?.Invoke(false);
            DialogueEvent.SetDialogueEvent?.Invoke(DialougeCharacter.Jia);
            DialogueEvent.ShowDialougeViewEvent?.Invoke(true);
        }
        else
        {
            DialogueEvent.ShowDialougeViewEvent?.Invoke(false);
            DialogueEvent.ShowMiniDialougeViewEvent?.Invoke(true);
        }

        StartCoroutine(SetBGType());

        ShowDialogue(_currentDialogueIndex);
    }

    private IEnumerator SetBGType()
    {
        switch (_filteredDialogueList[_currentDialogueIndex].bgType)
        {
            case BGType.FadeIn:
                SplashController.isfinished = false;
                StartCoroutine(_splashController.FadeIn(false, true));
                yield return new WaitUntil(() => SplashController.isfinished);
                break;
            case BGType.FadeOut:
                SplashController.isfinished = false; 
                StartCoroutine(_splashController.FadeOut(false, true)); 
                yield return new WaitUntil(() => SplashController.isfinished); 
                break;
            case BGType.FlashIn: 
                SplashController.isfinished = false; 
                StartCoroutine(_splashController.Splash()); 
                yield return new WaitUntil(() => SplashController.isfinished); 
                break;
            case BGType.FlashOut: 
                SplashController.isfinished = false; 
                StartCoroutine(_splashController.Splash()); 
                yield return new WaitUntil(() => SplashController.isfinished); 
                break;
            case BGType.ShowCutScene: 
                CutSceneController.isFinished = false; 
                StartCoroutine(_cutSceneController.CutSceneRoutine(_filteredDialogueList[_currentDialogueIndex].spriteName, true)); 
                yield return new WaitUntil(() => CutSceneController.isFinished); 
                break;
            case BGType.HideCutScene:
                CutSceneController.isFinished = false; 
                StartCoroutine(_cutSceneController.CutSceneRoutine(null, false)); 
                yield return new WaitUntil(() => CutSceneController.isFinished); 
                break;
        }
    }

    #region DialogueController
    private void ShowDialogue(int index)
    {
        if (index < 0 || index >= _filteredDialogueList.Count) return;
        DialogueData dialogue = _filteredDialogueList[index];

        if ((_dialogueUIData.characterName == "지아" && dialogue.characterName != "지아")
            || (_dialogueUIData.characterName != "지아" && dialogue.characterName == "지아"))
            DialogueEvent.ChangeCharacterEvent?.Invoke();

        if (_dialogueUIData.characterName == null)
            _dialogueUIData.characterName = "";
        else
            _dialogueUIData.characterName = dialogue.characterName;

        Sprite sprite = Resources.Load<Sprite>($"Sprite/Character/{dialogue.spriteName}");
        if (sprite != null)
            _dialogueUIData.SetProfile(sprite);

        string sound = dialogue.soundName;
        if (sound != null)
            GameManager.Instance.SoundSystemCompo.PlaySound(sound);

        AnimationEvent.SetDialogueAnimation?.Invoke(dialogue);

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _typingCoroutine = StartCoroutine(TypingEffect(dialogue.context));
    }

    private void ShowNextDialogue()
    {
        _currentDialogueIndex++;

        if (_currentDialogueIndex >= _filteredDialogueList.Count)
        {
            _isDialogue = false;

            if (!_isBigUIdata && _tutorialCheckData != null)
                EndMiniDialogue();
            else
                DialogueEvent.ShowDialougeViewEvent?.Invoke(false);
            if(_defaultCam != null)
                _defaultCam.SetActive(true);
            _onDialogueComplete?.Invoke();

            return;
        }

        ShowDialogue(_currentDialogueIndex);
    }

    private void EndMiniDialogue()
    {
        _tutorialCheckData.data = true;
        _computerCollider.enabled = true;
        _computerLight.SetActive(true);

        DialogueEvent.ShowMiniDialougeViewEvent?.Invoke(false);
    }

    private IEnumerator TypingEffect(string fullText)
    {
        GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Text_Typing);
        _isTyping = true;
        _dialogueUIData.dialogue = "";

        foreach (char letter in fullText)
        {
            _dialogueUIData.dialogue += letter;
            yield return new WaitForSeconds(_typingSpeed);
        }

        if (dialogueDataReader.readMode == ReadMode.Auto)
            yield return new WaitForSeconds(1.5f);

        _isTyping = false;
        GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Text_Typing);
    }

    private void CompleteTyping()
    {
        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _dialogueUIData.dialogue = _filteredDialogueList[_currentDialogueIndex].context;
        _isTyping = false;
    }
    #endregion
}
