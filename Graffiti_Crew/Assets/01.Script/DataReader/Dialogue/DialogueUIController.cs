using AH.UI.Events;
using System;
using System.Collections;
using UnityEngine;

public class DialogueUIController : MonoBehaviour
{
    [Header("Dialogue UI Data")]
    [SerializeField] private DialogueSO _bigDialogueUIData;
    [SerializeField] private DialogueSO _miniDialogueUIData;
    private DialogueSO _dialogueUIData;

    [Header("Typing Effect")]
    [SerializeField] private float _typingSpeed = 0.05f;

    private DialogueController _dialogueController;
    private DialogueEffectController _effectController;

    public bool IsBigUIdata => _dialogueUIData == _bigDialogueUIData;
    public bool IsTyping = false;

    private Coroutine _typingCoroutine;
    private Coroutine _showDialogueCoroutine;

    public Action<bool> ChangeDialogueUI;

    private DialougeCharacter _curCharacter;
    private DialougeCharacter _preCharacter;

    private void Awake()
    {
        _dialogueController = GetComponent<DialogueController>();
        _effectController = GetComponent<DialogueEffectController>();
    }

    private void Start()
    {
        _curCharacter = DialougeCharacter.Felling;
        DialogueEvent.SetDialogueEvent?.Invoke(DialougeCharacter.Felling);

        _dialogueUIData = _bigDialogueUIData;
        _dialogueUIData.ResetData();
    }

    private void OnEnable()
    {
        ChangeDialogueUI += HandleDialogueUIData;
    }

    private void OnDisable()
    {
        ChangeDialogueUI -= HandleDialogueUIData;
    }

    private void HandleDialogueUIData(bool isBig)
    {
        _dialogueUIData = isBig ? _bigDialogueUIData : _miniDialogueUIData;
    }

    public void SetDialogueUI()
    {
        if (IsBigUIdata)
        {
            DialogueEvent.ShowMiniDialougeViewEvent?.Invoke(false);
            DialogueEvent.ShowDialougeViewEvent?.Invoke(true);
        }
        else
        {
            DialogueEvent.ShowDialougeViewEvent?.Invoke(false);
            DialogueEvent.ShowMiniDialougeViewEvent?.Invoke(true);
        }
    }

    public void StartShowDialogue(int index)
    {
        if (_showDialogueCoroutine != null)
            StopCoroutine(_showDialogueCoroutine);

        _showDialogueCoroutine = StartCoroutine(ShowDialogue(index));
    }

    private IEnumerator ShowDialogue(int index)
    {
        if (index < 0 || index >= _dialogueController.filteredDialogueList.Count) yield break;

        DialogueData dialogue = _dialogueController.filteredDialogueList[index];

        if (dialogue.bgType != BGType.None && dialogue.bgType != BGType.Animation)
        {
            DialogueEvent.ShowDialougeViewEvent?.Invoke(false);
            yield return StartCoroutine(_effectController.SetBGType(dialogue.bgType));
            DialogueEvent.ShowDialougeViewEvent?.Invoke(true);
        }
        else if(dialogue.bgType == BGType.Animation)
        {
            AnimationEvent.SetDialogueAnimation?.Invoke(dialogue);
        }
        //else if(dialogue.bgType != BGType.Animation)
        //{
        //    AnimationEvent.SetBubble?.Invoke();
        //}

        if(dialogue.actionType != ActionType.None)
        {
            _effectController.SetActionType(dialogue.actionType);
        }

        string name = dialogue.characterName;
        if (string.IsNullOrEmpty(name))
            _preCharacter = DialougeCharacter.Felling;
        else
            _preCharacter = DialougeCharacter.Text;
        if(_curCharacter != _preCharacter)
        {
            _curCharacter = _preCharacter;
            DialogueEvent.SetDialogueEvent?.Invoke(_curCharacter);
        }
        _dialogueUIData.characterName = name;

        Sprite sprite = Resources.Load<Sprite>($"Sprite/Character/{dialogue.spriteName}");
        if (sprite != null)
            _dialogueUIData.SetProfile(sprite);

        if (!string.IsNullOrEmpty(dialogue.soundName))
        {
            if (Enum.TryParse(dialogue.soundName, true, out SoundType soundType))
            {
                if (soundType >= SoundType.Title_Front && soundType <= SoundType.Tutorial)
                {
                    GameManager.Instance.SoundSystemCompo.AllBGMStop(null);
                    GameManager.Instance.SoundSystemCompo.PlayBGM(soundType);
                }
                else
                {
                    GameManager.Instance.SoundSystemCompo.PlaySFX(soundType);
                }
            }
        }

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _typingCoroutine = StartCoroutine(TypingEffect(dialogue.context));
    }

    private IEnumerator TypingEffect(string fullText)
    {
        GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Text_Typing);
        IsTyping = true;
        _dialogueUIData.dialogue = "";

        bool black = false, red = false, cyan = false, green = false, yellow = false;
        bool ignore = false;

        foreach (char letter in fullText)
        {
            switch (letter)
            {
                case 'ⓑ': black = true; red = false; cyan = false; green = false; yellow = false; ignore = true; break;
                case 'ⓡ': black = false; red = true; cyan = false; green = false; yellow = false; ignore = true; break;
                case 'ⓒ': black = false; red = false; cyan = true; green = false; yellow = false; ignore = true; break;
                case 'ⓖ': black = false; red = false; cyan = false; green = true; yellow = false; ignore = true; break;
                case 'ⓨ': black = false; red = false; cyan = false; green = false; yellow = true; ignore = true; break;

            }
            string t_letter = letter.ToString();
            if (!ignore)
            {
                if (black) t_letter = "<color=#000000>" + t_letter + "</color>";
                else if (red) t_letter = "<color=#FF5C63>" + t_letter + "</color>";
                else if (cyan) t_letter = "<color=#5C88FF>" + t_letter + "</color>"; 
                else if (green) t_letter = "<color=#5EFF5C>" + t_letter + "</color>";
                else if (yellow) t_letter = "<color=#FFFA5C>" + t_letter + "</color>";
                _dialogueUIData.dialogue += t_letter;
            }
            ignore = false;
            yield return new WaitForSeconds(_typingSpeed);
        }

        if (_dialogueController.dialogueDataReader.readMode == ReadMode.Auto)
            yield return new WaitForSeconds(1.5f);

        IsTyping = false;
        GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Text_Typing);
    }

    public void CompleteTyping() //타이핑을 끝내고 싶은 때 호출
    {
        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        if (_showDialogueCoroutine != null)
            StopCoroutine(_showDialogueCoroutine);

        _dialogueUIData.dialogue = _dialogueController.filteredDialogueList[_dialogueController.currentDialogueIndex].context;
        GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Text_Typing);
        IsTyping = false;
    }

    public void ChangeTypingSpeed(float typingSpeed)
    {
        _typingSpeed = typingSpeed;
    }
}
