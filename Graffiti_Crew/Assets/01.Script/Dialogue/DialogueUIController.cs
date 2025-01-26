using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class DialogueUIController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject _dialogueUI;
    [SerializeField] private CanvasGroup _dialogueCanvas;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _contextText;
    [SerializeField] private Image _characterImage;
    private float _fadeDuration = 0.3f;

    [Header("Dialogue Data")]
    [SerializeField] private DialogueDataReader _dialogueDataReader;

    [Header("Typing Effect")]
    [SerializeField] private float _typingSpeed = 0.05f;

    private Coroutine _typingCoroutine;
    private bool _isTyping = false;

    private int _currentDialogueIndex = 0;
    private int _dialogueStartID = 0;
    private int _dialogueEndID = 0;

    private Action _onDialogueComplete;

    private List<DialogueData> _filteredDialogueList;

    private void Start()
    {
        _dialogueUI.SetActive(false);
        _nameText.text = "";
        _contextText.text = "";
        _characterImage.sprite = null;

        _dialogueCanvas.alpha = 0;
        _dialogueCanvas.interactable = false;
        _dialogueCanvas.blocksRaycasts = false;
    }

    public void StartDialogue(int startID, int endID, Action onComplete)
    {
        if (_dialogueDataReader == null || _dialogueDataReader.DialogueList.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        _dialogueStartID = startID;
        _dialogueEndID = endID;

        _filteredDialogueList = _dialogueDataReader.DialogueList.FindAll(dialogue =>
            dialogue.id >= _dialogueStartID && dialogue.id <= _dialogueEndID);

        if (_filteredDialogueList.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        _dialogueUI.SetActive(true);
        _currentDialogueIndex = 0;
        _onDialogueComplete = onComplete;

        ShowDialogue(_currentDialogueIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isTyping)
                CompleteTyping();
            else
                ShowNextDialogue();
        }
    }

    private void ShowDialogue(int index)
    {
        FadeIn();

        if (index < 0 || index >= _filteredDialogueList.Count) return;

        DialogueData dialogue = _filteredDialogueList[index];
        _nameText.text = dialogue.characterName;

        Sprite sprite = Resources.Load<Sprite>($"Sprite/{dialogue.spriteName}");
        if (sprite != null)
            _characterImage.sprite = sprite;

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _typingCoroutine = StartCoroutine(TypingEffect(dialogue.context));
    }

    private void ShowNextDialogue()
    {
        _currentDialogueIndex++;

        if (_currentDialogueIndex >= _filteredDialogueList.Count)
        {
            FadeOut();
            return;
        }

        ShowDialogue(_currentDialogueIndex);
    }

    private IEnumerator TypingEffect(string fullText)
    {
        _isTyping = true;
        _contextText.text = "";

        foreach (char letter in fullText)
        {
            _contextText.text += letter;
            yield return new WaitForSeconds(_typingSpeed);
        }

        _isTyping = false;
    }

    private void CompleteTyping()
    {
        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _contextText.text = _filteredDialogueList[_currentDialogueIndex].context;
        _isTyping = false;
    }

    public void FadeIn()
    {
        _dialogueCanvas.DOFade(1f, _fadeDuration).OnStart(() =>
        {
            _dialogueCanvas.interactable = true;
            _dialogueCanvas.blocksRaycasts = true;

        });
    }

    public void FadeOut()
    {
        _dialogueCanvas.DOFade(0f, _fadeDuration).OnComplete(() =>
        {
            _dialogueCanvas.interactable = false;
            _dialogueCanvas.blocksRaycasts = false;
            _dialogueUI.SetActive(false);
            _onDialogueComplete?.Invoke();
        });
    }
}
