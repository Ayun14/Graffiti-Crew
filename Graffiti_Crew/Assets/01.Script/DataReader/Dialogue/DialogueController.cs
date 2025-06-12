using AH.UI.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{
    [Header("Dialogue Data")]
    [SerializeField] private LanguageSO _languageSO;
    public DialogueDataReader dialogueDataReader;


    [Header("Dialogue Camera")]
    [SerializeField] private GameObject _defaultCam;

    private DialogueUIController _uiController;
    private DialogueEffectController _effectController;

    private bool _isHangoutScene =>
        SceneManager.GetSceneByName("HangOutScene") == SceneManager.GetActiveScene();
    private bool _isDialogue = false;
    public bool IsDialogue => _isDialogue;

    private Action _onDialogueComplete;

    [HideInInspector] public int currentDialogueIndex = 0;
    [HideInInspector] public List<DialogueData> filteredDialogueList;

    private void Awake()
    {
        _uiController = GetComponent<DialogueUIController>();
        _effectController = GetComponent<DialogueEffectController>();
    }

    private void Update()
    {
        if (!_uiController.IsBigUIdata && !_isHangoutScene) return;
        if (!_isDialogue) return;

        if (dialogueDataReader.readMode == ReadMode.Auto)
        {
            if (!_uiController.IsTyping)
                ShowNextDialogue();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
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

    public void DialogueSkip()
    {
        if (!_uiController.IsTyping)
        {
            AnimationEvent.EndDialogueAnimation?.Invoke();
            ShowNextDialogue();
        }
        else
        {
            //_uiController.CompleteTyping();
            _uiController.ChangeTypingSpeed(0.01f);
        }
    }

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
        if (_uiController.IsBigUIdata && _defaultCam != null)
        {
            _defaultCam.SetActive(false);
            yield return new WaitForSeconds(1.5f);
        }

        _uiController.SetDialogueUI();

        _uiController.StartShowDialogue(currentDialogueIndex);
    }

    private void ShowNextDialogue()
    {
        _uiController.ChangeTypingSpeed(0.05f);

        currentDialogueIndex++;

        if (currentDialogueIndex >= filteredDialogueList.Count)
        {
            _isDialogue = false;

            DialogueEvent.ShowDialougeViewEvent?.Invoke(false);

            if(_defaultCam!=null)
                _defaultCam.SetActive(true);

            StartCoroutine(_effectController.SetBGType(BGType.HideCutScene));
            _onDialogueComplete?.Invoke();

            return;
        }

        _uiController.StartShowDialogue(currentDialogueIndex);
    }
}
