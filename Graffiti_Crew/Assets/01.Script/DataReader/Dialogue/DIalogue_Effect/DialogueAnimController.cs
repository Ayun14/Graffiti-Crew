using System;
using UnityEngine;

public class DialogueAnimController : MonoBehaviour
{
    [SerializeField] private DialogueController _dialogueController;
    private BubbleController _bubbleManager; 

    private int _animValue = 0;

    private void Awake()
    {
        _bubbleManager = GetComponent<BubbleController>();
    }

    private void Start()
    {
        AnimationEvent.SetDialogueAnimation += HandleDialoguePlay;
        AnimationEvent.EndDialogueAnimation += HandleDialogueEnd;

        AnimationEvent.SetBubble += HideBubble;
    }


    private void OnDisable()
    {
        AnimationEvent.SetDialogueAnimation -= HandleDialoguePlay;
        AnimationEvent.EndDialogueAnimation -= HandleDialogueEnd;

        AnimationEvent.SetBubble -= HideBubble;
    }

    private void HandleDialoguePlay(DialogueData dialogue)
    {
        _animValue = 0;

        switch (dialogue.characterName)
        {
            case "지아":
                _animValue = 1;
                break;
            case "엘라":
                _animValue = 2;
                break;
            case "비비안":
                _animValue = 3;
                break;
            case "":
                _animValue = 0;
                break;
        }

        if (_animValue != 0)
        {
            AnimationEvent.SetAnimation?.Invoke(_animValue, (AnimationEnum)Enum.Parse(typeof(AnimationEnum), dialogue.animName));

            //if (DialogueCharacterController.Instance != null)
            //{
            //    Transform targetTrm = DialogueCharacterController.Instance.GetCharacterTransform(dialogue.characterName);
            //    if (targetTrm != null && _bubbleManager != null)
            //    {
            //        _bubbleManager.ShowSpeechBubble(targetTrm);
            //    }
            //}
        }
    }

    private void HandleDialogueEnd()
    {
        AnimationEvent.SetAnimation?.Invoke(_animValue, AnimationEnum.Idle);

        //if (_bubbleManager != null)
        //{
        //    _bubbleManager.HideSpeechBubble();
        //}
    }

    private void HideBubble()
    {
        if (_bubbleManager != null)
        {
            _bubbleManager.HideSpeechBubble();
        }
    }
}