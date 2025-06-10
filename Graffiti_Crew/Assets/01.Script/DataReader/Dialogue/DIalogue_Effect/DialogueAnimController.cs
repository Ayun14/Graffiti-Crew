using System;
using UnityEngine;

public class DialogueAnimController : MonoBehaviour
{
    [SerializeField] private DialogueController _dialogueController;
    private AnimationManager _animManager;
    private BubbleController _bubbleManager; 

    private int _animValue = 0;

    private void Awake()
    {
        _animManager = GetComponent<AnimationManager>();
        _bubbleManager = GetComponent<BubbleController>();
    }

    private void Start()
    {
        AnimationEvent.SetDialogueAnimation += HandleDialoguePlay;
        AnimationEvent.EndDialogueAnimation += HandleDialogueEnd;
    }

    private void OnDisable()
    {
        AnimationEvent.SetDialogueAnimation -= HandleDialoguePlay;
        AnimationEvent.EndDialogueAnimation -= HandleDialogueEnd;
    }

    private void HandleDialoguePlay(DialogueData dialogue)
    {
        _animValue = 0;

        switch (dialogue.characterName)
        {
            case "����":
                _animValue = 1;
                break;
            case "����":
                _animValue = 2;
                break;
            case "����":
                _animValue = 3;
                break;
            case "":
                _animValue = 0;
                break;
        }

        if (_animValue != 0)
        {
            AnimationEvent.SetAnimation?.Invoke(_animValue, (AnimationEnum)Enum.Parse(typeof(AnimationEnum), dialogue.animName));

            if (DialogueCharacterController.Instance != null)
            {
                Transform targetTrm = DialogueCharacterController.Instance.GetCharacterTransform(dialogue.characterName);
                if (targetTrm != null && _bubbleManager != null)
                {
                    _bubbleManager.ShowSpeechBubble(targetTrm);
                }
            }
        }
    }

    private void HandleDialogueEnd()
    {
        AnimationEvent.SetAnimation?.Invoke(_animValue, AnimationEnum.Idle);

        if (_bubbleManager != null)
        {
            _bubbleManager.HideSpeechBubble();
        }
    }
}