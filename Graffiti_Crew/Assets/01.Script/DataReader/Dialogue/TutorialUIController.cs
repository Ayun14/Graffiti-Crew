using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIController : MonoBehaviour
{
    [SerializeField] private BoolSaveDataSO _tutorialCheckData;
    [SerializeField] private GameObject _computerLight;

    [Header("Dialogue Data")]
    [SerializeField] private DialogueSO _dialogueUIData;
    [HideInInspector] public DialogueDataReader dialogueDataReader;
    public DialogueDataReader dialogueDataReader_KR;
    public DialogueDataReader dialogueDataReader_EN;

    [Header("Dialogue Data")]
    [SerializeField] private LanguageSO _languageSO;

    [Header("Typing Effect")]
    [SerializeField] private float _typingSpeed = 0.05f;

    [SerializeField] private Collider _computerCollider;

    private Coroutine _typingCoroutine;
    private bool _isTyping = false;
    private bool _isDialogue = false;

    private int _currentDialogueIndex = 0;

    private Action _onDialogueComplete;

    private List<DialogueData> _dialogueList;

    private void Awake()
    {
        dialogueDataReader = dialogueDataReader_KR;
    }

    private void Start()
    {
        _dialogueUIData.ResetData();

        if (LanguageSystem.GetLanguageType() == LanguageType.English)
        {
            dialogueDataReader = dialogueDataReader_EN;
        }
        else
        {
            dialogueDataReader = dialogueDataReader_KR;
        }
        LanguageSystem.LanguageChangedEvent += HandleChangeLangauge;

        if (_tutorialCheckData.data)
        {
            this.enabled = false;
        }
        else
        {
            _computerLight.SetActive(false);
            _computerCollider.enabled = false;
            StartDialogue();
        }
    }

    private void OnDisable()
    {
        LanguageSystem.LanguageChangedEvent -= HandleChangeLangauge;
    }

    private void HandleChangeLangauge(LanguageType type)
    {
        if (type == LanguageType.English)
        {
            _languageSO.title = "Language";
            _languageSO.languageTypes[0] = "Korea";
            _languageSO.languageTypes[1] = "English";
            dialogueDataReader = dialogueDataReader_EN;
        }
        else
        {
            _languageSO.title = "언어";
            _languageSO.languageTypes[0] = "한글";
            _languageSO.languageTypes[1] = "영어";
            dialogueDataReader = dialogueDataReader_KR;
        }
    }

    public void StartDialogue(Action onComplete = null)
    {
        _isDialogue = true;

        if (dialogueDataReader == null || dialogueDataReader.DialogueList.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        _dialogueList = dialogueDataReader.DialogueList;

        if (_dialogueList.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        DialougeEvent.ShowMiniDialougeViewEvent?.Invoke(true);

        _currentDialogueIndex = 0;
        _onDialogueComplete = onComplete;

        ShowDialogue(_currentDialogueIndex);
    }

    private void Update()
    {
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
                        CompleteTyping();
                    else
                        ShowNextDialogue();
                }
            }
        }
    }

    private void ShowDialogue(int index)
    {
        if (index < 0 || index >= _dialogueList.Count) return;

        DialogueData dialogue = _dialogueList[index];
        _dialogueUIData.characterName = dialogue.characterName;

        Sprite sprite = Resources.Load<Sprite>($"Sprite/{dialogue.spriteName}");
        if (sprite != null)
            _dialogueUIData.profil = sprite;

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _typingCoroutine = StartCoroutine(TypingEffect(dialogue.context));
    }

    private void ShowNextDialogue()
    {
        _currentDialogueIndex++;

        if (_currentDialogueIndex >= _dialogueList.Count)
        {
            _isDialogue = false;

            EndDialogue();

            DialougeEvent.ShowMiniDialougeViewEvent?.Invoke(false);
            _onDialogueComplete?.Invoke();

            return;
        }

        ShowDialogue(_currentDialogueIndex);
    }

    private void EndDialogue()
    {
        _tutorialCheckData.data = true;
        _computerCollider.enabled = true;
        _computerLight.SetActive(true);
    }

    private IEnumerator TypingEffect(string fullText)
    {
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
    }

    private void CompleteTyping()
    {
        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _dialogueUIData.dialogue = _dialogueList[_currentDialogueIndex].context;
        _isTyping = false;
    }
}
