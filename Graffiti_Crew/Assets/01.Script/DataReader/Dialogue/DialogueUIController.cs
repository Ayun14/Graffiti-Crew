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
    [Header("Dialogue UI Data")]
    [SerializeField] private DialogueSO _bigDialogueUIData;
    [SerializeField] private DialogueSO _miniDialogueUIData;
    private DialogueSO _dialogueUIData;

    [Header("Dialogue Data")]
    [SerializeField] private LanguageSO _languageSO;
    [HideInInspector] public DialogueDataReader dialogueDataReader;

    [Header("Typing Effect")]
    [SerializeField] private float _typingSpeed = 0.05f;

    [Header("Dialogue Camera")]
    [SerializeField] private GameObject _defaultCam;

    private DialogueEffectController _effectController;

    private bool _isBigUIdata => _dialogueUIData == _bigDialogueUIData;
    private bool _isHangoutScene =>
        SceneManager.GetSceneByName("HangOutScene") == SceneManager.GetActiveScene();

    private Coroutine _showDialogueCoroutine;
    private Coroutine _typingCoroutine;

    private bool _isTyping = false;
    private bool _isDialogue = false;
    public bool IsDialogue => _isDialogue;

    private Action _onDialogueComplete;
    public Action<bool> ChangeDialogueUI;

    [HideInInspector] public int currentDialogueIndex = 0;
    [HideInInspector] public List<DialogueData> filteredDialogueList;


    private void Awake()
    {
        _effectController = GetComponent<DialogueEffectController>();
    }

    private void Start()
    {
        _dialogueUIData = _bigDialogueUIData;
        _dialogueUIData.ResetData();
    }

    private void OnEnable()
    {
        ChangeDialogueUI += HandleDialogueUIData;
        DialogueEvent.DialogueSkipEvent += DialogueSkip;
    }

    private void OnDisable()
    {
        ChangeDialogueUI -= HandleDialogueUIData;
        DialogueEvent.DialogueSkipEvent -= DialogueSkip;
    }

    private void Update()
    {
        if (!_isBigUIdata && !_isHangoutScene) return;
        if (!_isDialogue) return;

        if (dialogueDataReader.readMode == ReadMode.Auto)
        {
            if (!_isTyping)
                ShowNextDialogue();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DialogueSkip();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Text_Typing);
                currentDialogueIndex = filteredDialogueList.Count;
                ShowNextDialogue();
            }
        }
    }

    #region Handle Event
    private void HandleDialogueUIData(bool isBig)
    {
        _dialogueUIData = isBig ? _bigDialogueUIData : _miniDialogueUIData;
    }

    private void DialogueSkip()
    {
        if(!_isTyping)
        {
            AnimationEvent.EndDialogueAnimation?.Invoke();
            ShowNextDialogue();
        }
    }

    #endregion

    public void StartDialogue(int startID, int endID, Action onComplete = null)
    {
        _isDialogue = true;

        if (dialogueDataReader == null || dialogueDataReader.DialogueList.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        filteredDialogueList = dialogueDataReader.DialogueList.FindAll(dialogue =>
            dialogue.id >= startID && dialogue.id <= endID);

        if (filteredDialogueList.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        currentDialogueIndex = 0;
        _onDialogueComplete = onComplete;

        StartCoroutine(DialogueRoutine());
    }

    private IEnumerator DialogueRoutine()
    {
        if (_isBigUIdata && _defaultCam != null)
        {
            _defaultCam.SetActive(false);
            yield return new WaitForSeconds(1.5f);
        }

        string name = filteredDialogueList[0].characterName;
        if (name == "지아")
            DialogueEvent.SetDialogueEvent?.Invoke(DialougeCharacter.Jia);
        else if (string.IsNullOrEmpty(name))
            DialogueEvent.SetDialogueEvent?.Invoke(DialougeCharacter.Felling);
        else
            DialogueEvent.SetDialogueEvent?.Invoke(DialougeCharacter.Other);

        if (_isBigUIdata)
        {
            DialogueEvent.ShowMiniDialougeViewEvent?.Invoke(false);
            DialogueEvent.ShowDialougeViewEvent?.Invoke(true);
        }
        else
        {
            DialogueEvent.ShowDialougeViewEvent?.Invoke(false);
            DialogueEvent.ShowMiniDialougeViewEvent?.Invoke(true);
        }

        StartShowDialogue(currentDialogueIndex);
    }

    #region UIController
    public void StartShowDialogue(int index)
    {
        if (_showDialogueCoroutine != null)
            StopCoroutine(_showDialogueCoroutine);

        _showDialogueCoroutine = StartCoroutine(ShowDialogue(index));
    }

    private IEnumerator ShowDialogue(int index)
    {
        if (index < 0 || index >= filteredDialogueList.Count) yield break;

        DialogueData dialogue = filteredDialogueList[index];

        yield return StartCoroutine(_effectController.SetBGType(dialogue));

        string name = dialogue.characterName;
        if (name == "지아") 
            DialogueEvent.SetDialogueEvent?.Invoke(DialougeCharacter.Jia);
        else if (string.IsNullOrEmpty(name)) 
            DialogueEvent.SetDialogueEvent?.Invoke(DialougeCharacter.Felling);
        else 
            DialogueEvent.SetDialogueEvent?.Invoke(DialougeCharacter.Other);

        _dialogueUIData.characterName = name;

        Sprite sprite = Resources.Load<Sprite>($"Sprite/Character/{dialogue.spriteName}");
        if (sprite != null)
            _dialogueUIData.SetProfile(sprite);

        if (!string.IsNullOrEmpty(dialogue.soundName))
            GameManager.Instance.SoundSystemCompo.PlaySound(dialogue.soundName);

        if (dialogue.bgType == BGType.None)
            AnimationEvent.SetDialogueAnimation?.Invoke(dialogue);

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _typingCoroutine = StartCoroutine(TypingEffect(dialogue.context));
    }

    private void ShowNextDialogue()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex >= filteredDialogueList.Count)
        {
            _isDialogue = false;
            DialogueEvent.ShowDialougeViewEvent?.Invoke(false);
            _defaultCam?.SetActive(true);
            _onDialogueComplete?.Invoke();
            return;
        }

        StartShowDialogue(currentDialogueIndex);
    }

    private IEnumerator TypingEffect(string fullText)
    {
        GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Text_Typing);
        _isTyping = true;
        _dialogueUIData.dialogue = "";

        bool black = false, red = false, cyan = false;
        bool ignore = false;

        foreach (char letter in fullText)
        {
            switch (letter)
            {
                case 'ⓑ': black = true; red = false; cyan = false; ignore = true; break;
                case 'ⓡ': black = false; red = true; cyan = false; ignore = true; break;
                case 'ⓒ': black = false; red = false; cyan = true; ignore = true; break;

            }
            string t_letter = letter.ToString();
            if (!ignore)
            {
                if (black) t_letter = "<color=#000000>" + t_letter + "</color>";
                else if (red) t_letter = "<color=#FF000B>" + t_letter + "</color>";
                else if (cyan) t_letter = "<color=#0038FF>" + t_letter + "</color>";
                _dialogueUIData.dialogue += t_letter;
            }
            ignore = false;
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

        if (_showDialogueCoroutine != null)
            StopCoroutine(_showDialogueCoroutine);

        _dialogueUIData.dialogue = filteredDialogueList[currentDialogueIndex].context;
        _isTyping = false;
    }
    #endregion
}
