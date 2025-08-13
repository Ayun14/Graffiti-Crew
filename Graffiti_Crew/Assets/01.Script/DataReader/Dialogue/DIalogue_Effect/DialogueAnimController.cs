using System;
using UnityEngine;

public class DialogueAnimController : MonoBehaviour
{
    private BubbleController _bubbleManager; 

    private int _animValue = 0;
    private AnimationEnum _curAnim;

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
            case "����":
                _animValue = 1;
                break;
            case "����":
                _animValue = 2;
                break;
            case "����":
                _animValue = 3;
                break;
            case "���":
                _animValue = 4;
                break;
            case "����":
                _animValue = 5;
                break;
            case "�ȳ�":
                _animValue = 6;
                break;
            case "����1":
                _animValue = 10;
                break;
            case "����2":
                _animValue = 11;
                break;
            case "����3":
                _animValue = 12;
                break;
            case "����4":
                _animValue = 13;
                break;
            case "":
                _animValue = 0;
                break;
        }

        if (_animValue != 0)
        {
            _curAnim = (AnimationEnum)Enum.Parse(typeof(AnimationEnum), dialogue.animName);
            AnimationEvent.SetAnimation?.Invoke(_animValue, _curAnim);
        }
    }

    private void HandleDialogueEnd()
    {
        if (_curAnim == AnimationEnum.Call)
            return;

        if(_animValue >= 10)
            AnimationEvent.SetAnimation?.Invoke(_animValue, AnimationEnum.People_Idle);
        else
            AnimationEvent.SetAnimation?.Invoke(_animValue, AnimationEnum.Idle);
    }

    private void HideBubble()
    {
        if (_bubbleManager != null)
        {
            _bubbleManager.HideSpeechBubble();
        }
    }
}